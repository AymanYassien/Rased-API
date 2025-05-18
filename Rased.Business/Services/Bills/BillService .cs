using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Bills;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.Models.Bills;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Rased.Business.Services.Bills
{
    public class BillService : IBillService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _geminiApiKey;

        public BillService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _geminiApiKey = configuration["Gemini:ApiKey"];
        }

        public async Task<ApiResponse<BillDtos>> ExtractBillDataFromImageAsync(IFormFile imageFile)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiApiKey}";

            string base64Image;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                base64Image = Convert.ToBase64String(memoryStream.ToArray());
            }

            string prompt = @"
            Analyze the following bill image and extract:
            1. total_price (as a number),
            2. date (YYYY-MM-DD or similar),
            3. description (e.g., restaurant, salon, etc.).
            Return result as pure JSON (no extra text or formatting).
        ";

            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new object[]
                    {
                        new
                        {
                            inlineData = new
                            {
                                mimeType = "image/jpeg",
                                data = base64Image
                            }
                        },
                        new { text = prompt.Trim() }
                    }
                }
            }
            };

            var requestJson = JsonConvert.SerializeObject(requestBody);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, requestContent);
            if (!response.IsSuccessStatusCode)
                return new ApiResponse<BillDtos>("Gemini API error: " + response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiApiResponse>(jsonResponse);
            var text = geminiResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text;

            if (string.IsNullOrWhiteSpace(text))
                return new ApiResponse<BillDtos>("No content returned from Gemini API.");

            var cleanedText = Regex.Replace(text, @"```(?:json)?|```", "").Trim();

            var jsonStart = cleanedText.IndexOf('{');
            var jsonEnd = cleanedText.LastIndexOf('}');
            if (jsonStart >= 0 && jsonEnd > jsonStart)
                cleanedText = cleanedText.Substring(jsonStart, jsonEnd - jsonStart + 1);

            try
            {
                var jObject = JObject.Parse(cleanedText);
                var normalized = jObject.Properties()
                                        .ToDictionary(p => p.Name.ToLowerInvariant(), p => p.Value);

                decimal totalPrice = normalized.ContainsKey("total_price") ? normalized["total_price"].Value<decimal>() : 0;
                string? description = normalized.ContainsKey("description") ? normalized["description"].Value<string>() : null;
                string? dateString = normalized.ContainsKey("date") ? normalized["date"].Value<string>() : null;

                DateTime? date = null;
                if (!string.IsNullOrWhiteSpace(dateString))
                {
                    string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd" };
                    if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        date = parsedDate;
                }

                var extracted = new BillDtos
                {
                    TotalPrice = totalPrice,
                    Description = description,
                    Date = date
                };

                return new ApiResponse<BillDtos>(extracted, "Bill data extracted successfully.");
            }
            catch (Exception)
            {
                return new ApiResponse<BillDtos>("Failed to parse bill data JSON.");
            }
        }

        public async Task<ApiResponse<int>> SaveBillDraftAsync(SaveBillDraftDto draftDto)
        {
            if (draftDto.ImageFile == null || draftDto.ImageFile.Length == 0)
                return new ApiResponse<int>("Please upload a valid image.");

            var extractResult = await ExtractBillDataFromImageAsync(draftDto.ImageFile);
            if (!extractResult.Succeeded || extractResult.Data == null)
                return new ApiResponse<int>("Failed to extract bill data from image.");

            var extracted = extractResult.Data;


            var billDraft = new BillDraft
            {
                Amount = (decimal?)(extracted.TotalPrice) ?? 0m,

                Date = extracted.Date ?? DateTime.Now,
                Description = extracted.Description ?? "No description available",
                WalletId = draftDto.WalletId,
                SharedWalletId = draftDto.SharedWalletId,
            };
            await _unitOfWork.BillDrafts.AddAsync(billDraft);
            await _unitOfWork.CommitChangesAsync(); 

            // Save Fatora and Connect With Draft
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(draftDto.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await draftDto.ImageFile.CopyToAsync(stream);
            }

            var attachment = new Attachment
            {
                FileName = draftDto.ImageFile.FileName,
                FileType = draftDto.ImageFile.ContentType,
                FilePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/"),
                UploadDate = DateTime.UtcNow,
                FileSize = draftDto.ImageFile.Length,
                BillDraftId = billDraft.BillDraftId,
                
            };

            await _unitOfWork.Attachments.AddAsync(attachment);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<int>(billDraft.BillDraftId, "Bill draft saved successfully.");
        }

        public async Task<ApiResponse<BillDraftDto>> GetDraftBillByIdAsync(int id)
        {
      
            var billDraft = await _unitOfWork.BillDrafts.GetByIdAsync(id);
            if (billDraft == null)
                return new ApiResponse<BillDraftDto>("Bill draft not found.");

            var billDraftDto = new BillDraftDto
            {
                BillDraftId = billDraft.BillDraftId,
                Amount = billDraft.Amount,
                Date = billDraft.Date,
                Description = billDraft.Description,
                WalletId = billDraft.WalletId,
                SharedWalletId = billDraft.SharedWalletId,
                Attachments = billDraft.Attachments.Select(a => new AttachmenttDto
                {
                    FileName = a.FileName,
                    FilePath = a.FilePath,
                    FileType = a.FileType
                }).ToList()
            };

            return new ApiResponse<BillDraftDto>(billDraftDto, "Bill draft retrieved successfully.");
        }

        public async Task<ApiResponse<int>> AddBillExpenseAsync(AddBillExpenseDto dto)
        {
            var billDraft = await _unitOfWork.BillDrafts.GetByIdAsync(dto.BillDraftId);
            if (billDraft == null)
                return new ApiResponse<int>("Bill draft not found.");

            var billExpense = new Expense
            {
                //From BillDraft
                Amount = billDraft.Amount != 0 ? billDraft.Amount : dto.Amount,
                Title =  dto.Title,
                Date = billDraft.Date != default ? billDraft.Date : dto.Date,
                Description = !string.IsNullOrEmpty(billDraft.Description) ? billDraft.Description : dto.Description,
                WalletId = billDraft.WalletId ?? dto.WalletId,
                SharedWalletId = billDraft.SharedWalletId ?? dto.SharedWalletId,

                // From User
                SubCategoryId = dto.SubCategoryId,
                CategoryName = dto.CategoryName,
                PaymentMethodId = dto.PaymentMethodId,
                RelatedBudgetId = dto.RelatedBudgetId
            };

            await _unitOfWork.Expenses.AddAsync(billExpense);
            await _unitOfWork.CommitChangesAsync();



            return new ApiResponse<int>(billExpense.ExpenseId, "Bill expense added successfully.");
        }

        







    }

}


