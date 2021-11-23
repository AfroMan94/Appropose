using Appropose.Core.Domain.Entities;
using Appropose.Functions.Commands;
using Appropose.Functions.Queries;
using AutoMapper;

namespace Appropose.Functions.MappingProfiles
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<UserEntity, UserLoginCommandResponse>();
            CreateMap<PostEntity, GetAllPostsQueryResponse>();
            CreateMap<SolutionEntity, GetSolutionForPostQueryResponse>();
        }
    }
}
