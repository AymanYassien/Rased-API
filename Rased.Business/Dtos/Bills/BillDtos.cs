using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Bills
{
    public class BillDtos
    {
        [JsonProperty("total_price")] 
        public decimal TotalPrice { get; set; }

        [JsonProperty("description")] 
        public string? Description { get; set; }

        [JsonProperty("date")] 
        public DateTime? Date { get; set; }


        public void SetDateFromString(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                if (DateTime.TryParse(dateString, out DateTime parsedDate))
                {
                    Date = parsedDate;  
                }
                else
                {
                    Date = null;  
                }
            }
        }
    }

    public class BillScanRequestDto
    {
        public IFormFile ImageFile { get; set; }  
    }

    // ---------------------------------------------------------

    public class SaveBillDraftDto
    {
    
        public IFormFile ImageFile { get; set; } 

        public int? WalletId { get; set; } 

        public int? SharedWalletId { get; set; } 
    }



    public class BillDraftDto
    {
        public int BillDraftId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public List<AttachmenttDto> Attachments { get; set; }
    }


    public class AttachmenttDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
    }





    public class AddBillExpenseDto
    {
 
        public int BillDraftId { get; set; }

      
        public int? WalletId { get; set; }

        
        public int? SharedWalletId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

    
        public decimal Amount { get; set; }

     
        public int? SubCategoryId { get; set; }

   
        public string? CategoryName { get; set; }


        public DateTime Date { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? RelatedBudgetId { get; set; }

     
    }

}








