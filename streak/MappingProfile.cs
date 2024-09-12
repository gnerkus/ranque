using AutoMapper;
using Entities;
using Shared;

namespace streak
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // entity -> dto
            CreateMap<Organization, OrganizationDto>()
                .ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x
                    .Address, x.Country)));

            CreateMap<Participant, ParticipantDto>();

            // dto -> entity
            CreateMap<OrgForCreationDto, Organization>();
        }
    }
}