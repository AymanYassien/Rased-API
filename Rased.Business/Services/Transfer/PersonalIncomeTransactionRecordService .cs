using Rased.Business.Dtos.Transfer;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure.UnitsOfWork;
using Rased.Infrastructure;
using System.Threading.Tasks;
using AutoMapper;

namespace Rased.Business.Services.Transfer
{
    public class PersonalIncomeTransactionRecordService : IPersonalIncomeTransactionRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersonalIncomeTransactionRecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreatePersonalIncomeTransactionRecordAsync(AddPersonalIncomeTransactionRecordDto dto)
        {
            var record = new PersonalIncomeTrasactionRecord
            {
                TransactionId = dto.TransactionId,
                IncomeId = dto.IncomeId,
                ApprovalId = dto.ApprovalId,
                CreatedAt = dto.CreatedAt
            };

            await _unitOfWork.PersonalIncomeTrasactionRecords.AddAsync(record);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "PersonalIncomeTransactionRecord created successfully");
        }

        public async Task<ApiResponse<IQueryable<GetPersonalIncomeTransactionRecordDto>>> GetPersonalIncomeTransactionRecordsByUserAndWalletAsync(string userId, int walletId)
        {
            var records = _unitOfWork.PersonalIncomeTrasactionRecords
                .FindAll(r => r.Transaction.SenderId == userId && r.Transaction.SenderWalletId == walletId && !r.isDeleted);

            var recordsDto = records.Select(r => new GetPersonalIncomeTransactionRecordDto
            {
                PersonalIncomeTrasactionRecordId = r.PersonalIncomeTrasactionRecordId,
                TransactionId = r.TransactionId,
                IncomeId = r.IncomeId,
                ApprovalId = r.ApprovalId,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IncomeSpecificData = r.IncomeSpecificData
            });

            return new ApiResponse<IQueryable<GetPersonalIncomeTransactionRecordDto>>(recordsDto);
        }


        public async Task<ApiResponse<string>> UpdatePersonalIncomeTransactionRecordAsync(int id, UpdatePersonalIncomeTransactionRecordDto dto)
        {
            var record = await _unitOfWork.PersonalIncomeTrasactionRecords.GetByIdAsync(id);
            if (record == null || record.isDeleted)
            {
                return new ApiResponse<string>("Record not found or already deleted");
            }

          

            if (dto.IncomeSpecificData != null)
            {
                record.IncomeSpecificData = dto.IncomeSpecificData;
            }

            record.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "PersonalIncomeTransactionRecord updated successfully");
        }

        public async Task<ApiResponse<string>> DeletePersonalIncomeTransactionRecordAsync(int id)
        {
            var record = await _unitOfWork.PersonalIncomeTrasactionRecords.GetByIdAsync(id);
            if (record == null || record.isDeleted)
            {
                return new ApiResponse<string>("Record not found or already deleted");
            }

            record.isDeleted = true;
            await _unitOfWork.CommitChangesAsync();
            return new ApiResponse<string>(null, "PersonalIncomeTransactionRecord deleted successfully");
        }
    }
}
