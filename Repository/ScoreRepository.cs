using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared;

namespace Repository
{
    public class ScoreRepository : RepositoryBase<Score>, IScoreRepository
    {
        public ScoreRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Score>> GetAllScoresAsync(ScoreParameters parameters,
            bool trackChanges)
        {
            var items = await FindAll(trackChanges)
                .FilterScores(parameters.LeaderboardId, parameters.ParticipantId)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Score>(items, count, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Score>> GetParticipantScoresAsync(Guid participantId,
            ScoreParameters parameters, bool trackChanges)
        {
            var items = await FindByCondition(c => c.ParticipantId.Equals(participantId),
                    trackChanges)
                .FilterScores(Guid.Empty, parameters.ParticipantId)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(c => c.ParticipantId.Equals(participantId),
                trackChanges).CountAsync();

            return new PagedList<Score>(items, count, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Score>> GetLeaderboardScoresAsync(Guid leaderboardId,
            ScoreParameters parameters, bool trackChanges)
        {
            var items = await FindByCondition(c => c.LeaderboardId.Equals(leaderboardId),
                    trackChanges)
                .FilterScores(parameters.LeaderboardId, Guid.Empty)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(c => c.ParticipantId.Equals(leaderboardId),
                trackChanges).CountAsync();

            return new PagedList<Score>(items, count, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Score?> GetScoreAsync(Guid scoreId, bool trackChanges)
        {
            return await FindByCondition(
                    c => c.Id.Equals(scoreId),
                    trackChanges)
                .SingleOrDefaultAsync();
        }

        public void CreateScore(Guid leaderboardId, Guid participantId, Score score)
        {
            score.LeaderboardId = leaderboardId;
            score.ParticipantId = participantId;
            Create(score);
        }

        public void DeleteScore(Score score)
        {
            Delete(score);
        }
    }
}