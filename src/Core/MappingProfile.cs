﻿using AutoMapper;
using Entities.Models;
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

            // leaderboard
            CreateMap<Leaderboard, LeaderboardDto>();
            CreateMap<Leaderboard, RankedLeaderboardDto>();

            // score
            CreateMap<Score, ScoreDto>();

            // dto -> entity
            // ------------------------------------------------
            // org
            CreateMap<OrgForCreationDto, Organization>();
            CreateMap<OrgForUpdateDto, Organization>();

            // participant
            CreateMap<ParticipantForCreationDto, Participant>();
            CreateMap<ParticipantForUpdateDto, Participant>().ReverseMap();

            // leaderboard
            CreateMap<LeaderboardForCreationDto, Leaderboard>();
            CreateMap<LeaderboardForUpdateDto, Leaderboard>().ReverseMap();

            // score
            CreateMap<ScoreForManipulationDto, Score>();

            // user
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}