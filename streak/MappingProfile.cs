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
            // ------------------------------------------------
            // org
            CreateMap<Organization, OrganizationDto>()
                .ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x
                    .Address, x.Country)));

            // participant
            CreateMap<Participant, ParticipantDto>();

            // dto -> entity
            // ------------------------------------------------
            // org
            CreateMap<OrgForCreationDto, Organization>();
            CreateMap<OrgForUpdateDto, Organization>();
            
            // participant
            CreateMap<ParticipantForCreationDto, Participant>();
            CreateMap<ParticipantForUpdateDto, Participant>();
        }
    }
}