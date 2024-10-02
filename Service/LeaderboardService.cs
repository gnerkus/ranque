using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Shared;

namespace Service
{
    internal sealed class LeaderboardService : ILeaderboardService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly ILeaderboardLinks _leaderboardLinks;

        public LeaderboardService(IRepositoryManager repository, ILoggerManager logger,
            IMapper mapper, ILeaderboardLinks leaderboardLinks)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _leaderboardLinks = leaderboardLinks;
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetLeaderboardsAsync
        (Guid orgId, LeaderboardLinkParams parameters, bool trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);

            var leaderboards =
                await _repository.Leaderboard.GetAllLeaderboardsAsync(orgId, parameters.LeaderboardParameters,
                    trackChanges);
            var leaderboardDtos = _mapper.Map<IEnumerable<LeaderboardDto>>(leaderboards);
            var links = _leaderboardLinks.TryGenerateLinks(leaderboardDtos, parameters
                .LeaderboardParameters.Fields, orgId, parameters.Context);
            
            return (linkResponse: links, metaData: leaderboards.MetaData);
        }

        public async Task<LeaderboardDto> GetLeaderboardAsync(Guid orgId, Guid leaderboardId, bool trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);

            var leaderboardDb =
                await IsLeaderboardExist(orgId, leaderboardId, trackChanges);

            var leaderboard = _mapper.Map<LeaderboardDto>(leaderboardDb);
            return leaderboard;
        }

        public async Task<LeaderboardDto> CreateLeaderboardForOrgAsync(Guid orgId, LeaderboardForCreationDto leaderboardForCreationDto,
            bool trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);

            var leaderboard = _mapper.Map<Leaderboard>(leaderboardForCreationDto);
            _repository.Leaderboard.CreateLeaderboard(orgId, leaderboard);
            await _repository.SaveAsync();

            return _mapper.Map<LeaderboardDto>(leaderboard);
        }

        public async Task DeleteLeaderboardForOrgAsync(Guid orgId, Guid leaderboardId, bool trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);

            var leaderboardDb = await IsLeaderboardExist(orgId, leaderboardId, trackChanges);
            _repository.Leaderboard.DeleteLeaderboard(leaderboardDb);
            await _repository.SaveAsync();
        }

        public async Task UpdateLeaderboardForOrgAsync(Guid orgId, Guid leaderboardId,
            LeaderboardForUpdateDto leaderboardForUpdateDto, bool orgTrackChanges,
            bool leaderboardTrackChanges)
        {
            await IsOrgExist(orgId, orgTrackChanges);

            var leaderboardDb =
                await IsLeaderboardExist(orgId, leaderboardId, leaderboardTrackChanges);

            _mapper.Map(leaderboardForUpdateDto, leaderboardDb);
            await _repository.SaveAsync();
        }

        public async Task<(LeaderboardForUpdateDto leaderboardToPatch, Leaderboard leaderboard)> GetLeaderboardForPatchAsync(Guid orgId, Guid leaderboardId, bool orgTrackChanges,
            bool leaderboardTrackChanges)
        {
            await IsOrgExist(orgId, orgTrackChanges);

            var leaderboardDb =
                await IsLeaderboardExist(orgId, leaderboardId, leaderboardTrackChanges);

            var leaderboardToPatch = _mapper.Map<LeaderboardForUpdateDto>(leaderboardDb);

            return (leaderboardToPatch, leaderboardDb);
        }

        public async Task SaveChangesForPatchAsync(LeaderboardForUpdateDto leaderboardToPatch, Leaderboard leaderboard)
        {
            _mapper.Map(leaderboardToPatch, leaderboard);
            await _repository.SaveAsync();
        }
        
        private async Task IsOrgExist(Guid orgId, bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
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