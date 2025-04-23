using Microsoft.AspNetCore.Http;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Bills;
using Rased.Business.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Bills
{
    public interface IBillService
    {

        Task<ApiResponse<BillDtos>> ExtractBillDataFromImageAsync(IFormFile imageFile);
        Task<ApiResponse<int>> SaveBillDraftAsync(SaveBillDraftDto draftDto);
        Task<ApiResponse<BillDraftDto>> GetDraftBillByIdAsync(int id);
        Task<ApiResponse<int>> AddBillExpenseAsync(AddBillExpenseDto dto);
        
    }


}
