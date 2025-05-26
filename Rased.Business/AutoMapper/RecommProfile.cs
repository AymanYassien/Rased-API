using AutoMapper;
using Rased.Business.Dtos.Recomm;
using Rased.Infrastructure.Models.Recomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.AutoMapper
{
    public class RecommProfile : Profile
    {
        public RecommProfile()
        {
            CreateMap<BudgetRecommendation, BudgetRecommendationDto>().ReverseMap();

        }

    }
}
