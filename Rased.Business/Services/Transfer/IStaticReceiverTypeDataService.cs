using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface IStaticReceiverTypeDataService
    {
        Task<ApiResponse<string>> AddReceiverTypeAsync(AddStaticReceiverTypeDataDto dto);
        Task<ApiResponse<string>> UpdateReceiverTypeAsync(UpdateStaticReceiverTypeDataDto dto);
        Task<ApiResponse<string>> DeleteReceiverTypeAsync(int id);
        Task<ApiResponse<IQueryable<ReadStaticReceiverTypeDataDto>>> GetAllAsync();
        Task<ApiResponse<ReadStaticReceiverTypeDataDto?>> GetByIdAsync(int id);



    }
}
