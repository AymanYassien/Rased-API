using AutoMapper;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Savings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.AutoMapper
{
    public class SavingProfile :Profile
    {
        public SavingProfile()
        {
            CreateMap<Saving , ReadSavingDto>().ReverseMap();
            CreateMap<Saving, AddSavingDto>().ReverseMap();
            CreateMap<Saving, UpdateSavingDto>().ReverseMap();
        }

    }
}
