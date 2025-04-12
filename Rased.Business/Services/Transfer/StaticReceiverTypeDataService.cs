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
    public class StaticReceiverTypeDataService : IStaticReceiverTypeDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StaticReceiverTypeDataService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> AddReceiverTypeAsync(AddStaticReceiverTypeDataDto dto)
        {
            var receiverType = new StaticReceiverTypeData
            {
                Name = dto.ReceiverTypeName
            };

            await _unitOfWork.StaticReceiverTypes.AddAsync(receiverType);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "ReceiverType added successfully");
        }

        public async Task<ApiResponse<string>> UpdateReceiverTypeAsync(UpdateStaticReceiverTypeDataDto dto)
        {
            var receiverType = await _unitOfWork.StaticReceiverTypes.GetByIdAsync(dto.ReceiverTypeId);
            if (receiverType == null)
                return new ApiResponse<string>("Receiver Type not found");

            receiverType.Name = dto.ReceiverTypeName;

            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "ReceiverType updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteReceiverTypeAsync(int id)
        {
            var receiverType = await _unitOfWork.StaticReceiverTypes.GetByIdAsync(id);
            if (receiverType == null)
                return new ApiResponse<string>("Receiver Type not found");

            await _unitOfWork.StaticReceiverTypes.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "ReceiverType deleted successfully");
        }


        public async Task<ApiResponse<IQueryable<ReadStaticReceiverTypeDataDto>>> GetAllAsync()
        {
            var data = _unitOfWork.StaticReceiverTypes
                .GetAll()
                .ProjectTo<ReadStaticReceiverTypeDataDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadStaticReceiverTypeDataDto>>(data);
        }

        public async Task<ApiResponse<ReadStaticReceiverTypeDataDto?>> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.StaticReceiverTypes.GetByIdAsync(id);
            if (item == null)
                return new ApiResponse<ReadStaticReceiverTypeDataDto?>("Receiver Type not found");

            return new ApiResponse<ReadStaticReceiverTypeDataDto?>(_mapper.Map<ReadStaticReceiverTypeDataDto>(item));
        }
    }

}
