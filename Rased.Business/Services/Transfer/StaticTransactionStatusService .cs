using AutoMapper;
using AutoMapper.QueryableExtensions;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public class StaticTransactionStatusService : IStaticTransactionStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StaticTransactionStatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> AddAsync(AddStaticTransactionStatusDto dto)
        {
            await _unitOfWork.StaticTransactionStatus.AddAsync(_mapper.Map<StaticTransactionStatusData>(dto));
            await _unitOfWork.CommitChangesAsync();
            return new ApiResponse<string>(null, "Transaction Status added successfully");
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateStaticTransactionStatusDto dto)
        {
            var existing = await _unitOfWork.StaticTransactionStatus.GetByIdAsync(dto.Id);
            if (existing == null)
                return new ApiResponse<string>("Transaction Status not found");

            existing.Name = dto.Name;
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction Status updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var existing = await _unitOfWork.StaticTransactionStatus.GetByIdAsync(id);
            if (existing == null)
                return new ApiResponse<string>("Transaction Status not found");

            await _unitOfWork.StaticTransactionStatus.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction Status deleted successfully");
        }

        public async Task<ApiResponse<IQueryable<ReadStaticTransactionStatusDto>>> GetAllAsync()
        {
            var data = _unitOfWork.StaticTransactionStatus
                .GetAll()
                .ProjectTo<ReadStaticTransactionStatusDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadStaticTransactionStatusDto>>(data);
        }

        public async Task<ApiResponse<ReadStaticTransactionStatusDto?>> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.StaticTransactionStatus.GetByIdAsync(id);
            if (item == null)
                return new ApiResponse<ReadStaticTransactionStatusDto?>("Transaction status not found");

            return new ApiResponse<ReadStaticTransactionStatusDto?>(_mapper.Map<ReadStaticTransactionStatusDto>(item));
        }

    }

}
