﻿using Api.Cache;
using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Shared;

namespace Service
{
    internal sealed class LeaderboardService : ILeaderboardService, IApiService
    {
        private readonly ILeaderboardLinks _leaderboardLinks;
        private readonly IMapper _mapper;
        private readonly IRedisService _redis;
        private readonly IRepositoryManager _repository;

        public LeaderboardService(IRepositoryManager repository, ILogger<IApiService> logger,
            IMapper mapper, ILeaderboardLinks leaderboardLinks, IRedisService redisService)
        {
            _repository = repository;
            _mapper = mapper;
            _leaderboardLinks = leaderboardLinks;
            _redis = redisService;
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetLeaderboardsAsync
            (string userId, Guid orgId, LeaderboardLinkParams parameters, bool trackChanges)
        {
            await IsOrgExist(userId, orgId, trackChanges);

            var leaderboards =
                await _repository.Leaderboard.GetAllLeaderboardsAsync(orgId,
                    parameters.LeaderboardParameters,
                    trackChanges);
            var leaderboardDtos = _mapper.Map<IEnumerable<LeaderboardDto>>(leaderboards);
            var links = _leaderboardLinks.TryGenerateLinks(leaderboardDtos, parameters
                .LeaderboardParameters.Fields!, orgId, parameters.Context);

            return (linkResponse: links, metaData: leaderboards.MetaData);
        }

        public async Task<RankedLeaderboardDto> GetLeaderboardAsync(string userId, Guid orgId, Guid
                leaderboardId,
            bool trackChanges)
        {
            await IsOrgExist(userId, orgId, trackChanges);

            var leaderboardDb =
                await IsLeaderboardExist(orgId, leaderboardId, trackChanges);

            var rankedParticipantsDto = _redis.GetLeaderboard(leaderboardId);

            return new RankedLeaderboardDto
            {
                Id = leaderboardDb.Id,
                Name = leaderboardDb.Name,
                LuaScript = leaderboardDb.LuaScript,
                Participants = rankedParticipantsDto
            };
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipantsAsync(Guid leaderboardId,
            bool trackChanges)
        {
            var participants =
                await _repository.Leaderboard.GetParticipantsAsync(leaderboardId, trackChanges);

            return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
        }

        public async Task<LeaderboardDto> CreateLeaderboardForOrgAsync(string userId, Guid orgId,
            LeaderboardForCreationDto leaderboardForCreationDto,
            bool trackChanges)
        {
            await IsOrgExist(userId, orgId, trackChanges);

            var leaderboard = _mapper.Map<Leaderboard>(leaderboardForCreationDto);
            _repository.Leaderboard.CreateLeaderboard(orgId, leaderboard);
            await _repository.SaveAsync();

            return _mapper.Map<LeaderboardDto>(leaderboard);
        }

        public async Task DeleteLeaderboardForOrgAsync(string userId, Guid orgId,
            Guid leaderboardId,
            bool trackChanges)
        {
            await IsOrgExist(userId, orgId, trackChanges);

            var leaderboardDb = await IsLeaderboardExist(orgId, leaderboardId, trackChanges);
            _repository.Leaderboard.DeleteLeaderboard(leaderboardDb);
            await _repository.SaveAsync();
        }

        public async Task UpdateLeaderboardForOrgAsync(string userId, Guid orgId,
            Guid leaderboardId,
            LeaderboardForUpdateDto leaderboardForUpdateDto, bool orgTrackChanges,
            bool leaderboardTrackChanges)
        {
            await IsOrgExist(userId, orgId, orgTrackChanges);

            var leaderboardDb =
                await IsLeaderboardExist(orgId, leaderboardId, leaderboardTrackChanges);

            _mapper.Map(leaderboardForUpdateDto, leaderboardDb);
            await _repository.SaveAsync();
        }

        public async Task<(LeaderboardForUpdateDto leaderboardToPatch, Leaderboard leaderboard)>
            GetLeaderboardForPatchAsync(string userId, Guid orgId, Guid leaderboardId, bool
                    orgTrackChanges,
                bool leaderboardTrackChanges)
        {
            await IsOrgExist(userId, orgId, orgTrackChanges);

            var leaderboardDb =
                await IsLeaderboardExist(orgId, leaderboardId, leaderboardTrackChanges);

            var leaderboardToPatch = _mapper.Map<LeaderboardForUpdateDto>(leaderboardDb);

            return (leaderboardToPatch, leaderboardDb);
        }

        public async Task SaveChangesForPatchAsync(LeaderboardForUpdateDto leaderboardToPatch,
            Leaderboard leaderboard)
        {
            _mapper.Map(leaderboardToPatch, leaderboard);
            await _repository.SaveAsync();
        }

        private async Task IsOrgExist(string userId, Guid orgId, bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(userId, orgId,
                trackChanges);
            if (org is null) throw new OrgNotFoundException(orgId);
        }

        private async Task<Leaderboard> IsLeaderboardExist(Guid orgId, Guid leaderboardId,
            bool trackChanges)
        {
            var leaderboardDb = await _repository.Leaderboard.GetLeaderboardForOrgAsync(orgId,
                leaderboardId,
                trackChanges);
            if (leaderboardDb is null)
                throw new LeaderboardNotFoundException(leaderboardId);

            return leaderboardDb;
        }
    }
}