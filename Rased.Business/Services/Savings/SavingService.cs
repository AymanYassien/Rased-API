using AutoMapper;
using AutoMapper.QueryableExtensions;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Savings
{
    public class SavingService : ISavingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SavingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllSavingAsync()
        {
            var savings = _unitOfWork.SavingRepository.GetAll()
                .ProjectTo<ReadSavingDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadSavingDto>>(savings);
        }

        public async Task<ApiResponse<ReadSavingDto>> GetSavingByIdAsync(int id)
        {
            var saving = await _unitOfWork.SavingRepository.GetByIdAsync(id);
            if (saving == null)
                return new ApiResponse<ReadSavingDto>("Saving not found.");

            return new ApiResponse<ReadSavingDto>(_mapper.Map<ReadSavingDto>(saving));
        }

        public async Task<ApiResponse<string>> AddSavingAsync(AddSavingDto addSavingDto)
        {
            await _unitOfWork.SavingRepository.AddAsync(_mapper.Map<Saving>(addSavingDto));
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Saving added successfully.");
        }

        public async Task<ApiResponse<string>> UpdateSavingAsync(UpdateSavingDto updateSavingDto)
        {
            var saving = await _unitOfWork.SavingRepository.GetByIdAsync(updateSavingDto.Id);
            if (saving == null)
                return new ApiResponse<string>("Saving not found.");

            _mapper.Map(updateSavingDto, saving);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Saving Updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteSavingAsync(int id)
        {
            var saving = await _unitOfWork.SavingRepository.GetByIdAsync(id);
            if (saving == null)
                return new ApiResponse<string>("Saving not found.");

            await _unitOfWork.SavingRepository.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Saving Deleted successfully.");
        }
    }

}
