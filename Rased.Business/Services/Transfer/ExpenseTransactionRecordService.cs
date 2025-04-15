using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Infrastructure.UnitsOfWork;
using Rased.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Rased.Business.Services.Transfer
{
    public class ExpenseTransactionRecordService : IExpenseTransactionRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseTransactionRecordService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateExpenseTransactionRecordAsync(ExpenseTransactionRecordDtos dto)
        {
            var record = new ExpenseTransactionRecord
            {
                TransactionId = dto.TransactionId,
                ExpenseId = dto.ExpenseId,
                CreatedAt = dto.CreatedAt
            };

            await _unitOfWork.ExpenseTransactionRecords.AddAsync(record);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "ExpenseTransactionRecord created successfully");
        }

        public async Task<ApiResponse<IQueryable<GetExpenseTransactionRecordDto>>> GetExpenseTransactionRecordsByUserAndWalletAsync(string userId, int walletId)
        {
            var records =  _unitOfWork.ExpenseTransactionRecords
                .FindAll(r => r.Transaction.SenderId == userId && r.Transaction.SenderWalletId == walletId && !r.isDeleted);


            var recordsDto = records.Select(r => new GetExpenseTransactionRecordDto
            {
                ExpenseTrasactionRecordId = r.ExpenseTrasactionRecordId,
                TransactionId = r.TransactionId,
                ExpenseId = r.ExpenseId,
                isDeleted = r.isDeleted,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ExpenseSpecificData = r.ExpenseSpecificData
            });

            return new ApiResponse<IQueryable<GetExpenseTransactionRecordDto>>(recordsDto);
        }


        public async Task<ApiResponse<string>> UpdateExpenseTransactionRecordAsync(int id, UpdateExpenseTransactionRecordDto dto)
        {
            var record = await _unitOfWork.ExpenseTransactionRecords.GetByIdAsync(id);
            if (record == null || record.isDeleted)
            {
                return new ApiResponse<string>("Record not found or already deleted");
            }

            if (dto.ExpenseId.HasValue)
            {
                record.ExpenseId = dto.ExpenseId.Value;
            }

            if (dto.ExpenseSpecificData != null)
            {
                record.ExpenseSpecificData = dto.ExpenseSpecificData;
            }

            record.UpdatedAt = DateTime.UtcNow; 

            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "ExpenseTransactionRecord updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteExpenseTransactionRecordAsync(int id)
        {
            var record = await _unitOfWork.ExpenseTransactionRecords.GetByIdAsync(id);
            if (record == null || record.isDeleted)
            {
                return new ApiResponse<string>("Record not found or already deleted");
            }

            record.isDeleted = true;
            await _unitOfWork.CommitChangesAsync();
            return new ApiResponse<string>(null, "ExpenseTransactionRecord deleted successfully");
        }
    }


}
