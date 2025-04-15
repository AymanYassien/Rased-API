using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface IExpenseTransactionRecordService
    {
        Task<ApiResponse<string>> CreateExpenseTransactionRecordAsync(ExpenseTransactionRecordDtos dto);
        Task<ApiResponse<IQueryable<GetExpenseTransactionRecordDto>>> GetExpenseTransactionRecordsByUserAndWalletAsync(string userId, int walletId);
        Task<ApiResponse<string>> UpdateExpenseTransactionRecordAsync(int id, UpdateExpenseTransactionRecordDto dto);
        Task<ApiResponse<string>> DeleteExpenseTransactionRecordAsync(int id);
      }
}
