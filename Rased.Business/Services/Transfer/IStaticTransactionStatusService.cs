using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface IStaticTransactionStatusService
    {
        Task<ApiResponse<string>> AddAsync(AddStaticTransactionStatusDto dto);
        Task<ApiResponse<string>> UpdateAsync(UpdateStaticTransactionStatusDto dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
        Task<ApiResponse<IQueryable<ReadStaticTransactionStatusDto>>> GetAllAsync();
        Task<ApiResponse<ReadStaticTransactionStatusDto?>> GetByIdAsync(int id);

    }

}
