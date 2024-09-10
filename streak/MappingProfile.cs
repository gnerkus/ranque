using AutoMapper;
using Entities;
using Shared;

namespace streak
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationDto>()
                .ForCtorParam("FullAddress", opt => opt.MapFrom(x => string.Join(' ', x.Address,
                 x.Country)));
        }
    }
}