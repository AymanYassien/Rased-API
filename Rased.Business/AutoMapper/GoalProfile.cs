using AutoMapper;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.AutoMapper
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, ReadGoalDto>().ReverseMap();
            CreateMap<Goal, AddGoalDto>().ReverseMap();
            CreateMap<Goal, UpdateGoalDto>().ReverseMap();
        }
    }
}
