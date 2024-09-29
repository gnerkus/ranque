using AutoMapper;
using Contracts;
using Entities;
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

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetLeaderboardsAsync(Guid orgId, LinkParameters parameters, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaderboardDto> GetLeaderboardAsync(Guid orgId, Guid leaderboardId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaderboardDto> CreateLeaderboardForOrgAsync(Guid orgId, LeaderboardForCreationDto leaderboardForCreationDto,
            bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLeaderboardForOrgAsync(Guid orgId, Guid leaderboardId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateLeaderboardForOrgAsync(Guid orgId, Guid leaderboardId,
            LeaderboardForUpdateDto leaderboardForUpdateDto, bool orgTrackChanges,
            bool leaderboardTrackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task<(LeaderboardForUpdateDto leaderboardToPatch, Leaderboard leaderboard)> GetLeaderboardForPatchAsync(Guid orgId, Guid leaderboardId, bool orgTrackChanges,
            bool leaderboardTrackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesForPatchAsync(LeaderboardForUpdateDto leaderboardToPatch, Leaderboard leaderboard)
        {
            throw new NotImplementedException();
        }
    }
}