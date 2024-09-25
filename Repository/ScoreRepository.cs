using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared;

namespace Repository
{
    public class ScoreRepository: RepositoryBase<Score>, IScoreRepository
    {
        public ScoreRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Score>> GetAllScoresAsync(ScoreParameters parameters, bool trackChanges)
        {
            var items = await FindAll(trackChanges)
                .FilterScores(parameters.LeaderboardId, parameters.ParticipantId)
                .Sort(parameters.OrderBy)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

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