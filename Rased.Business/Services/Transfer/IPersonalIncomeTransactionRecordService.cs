using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface IPersonalIncomeTransactionRecordService
    {

        Task<ApiResponse<string>> CreatePersonalIncomeTransactionRecordAsync(AddPersonalIncomeTransactionRecordDto dto);
        Task<ApiResponse<IQueryable<GetPersonalIncomeTransactionRecordDto>>> GetPersonalIncomeTransactionRecordsByUserAndWalletAsync(string userId , int walletId);
        Task<ApiResponse<string>> UpdatePersonalIncomeTransactionRecordAsync(int id, UpdatePersonalIncomeTransactionRecordDto dto);
        Task<ApiResponse<string>> DeletePersonalIncomeTransactionRecordAsync(int id);
    }
}
