using Contracts;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ILeaderboardRepository> _leaderboardRepository;
        private readonly Lazy<IOrganizationRepository> _organizationRepository;
        private readonly Lazy<IParticipantRepository> _participantRepository;
        private readonly Lazy<IScoreRepository> _scoreRepository;
        private readonly RepositoryContext _repositoryContext;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _leaderboardRepository = new Lazy<ILeaderboardRepository>(() => new
                LeaderboardRepository(repositoryContext));
            _organizationRepository = new Lazy<IOrganizationRepository>(() => new
                OrganizationRepository(repositoryContext));
            _participantRepository = new Lazy<IParticipantRepository>(() => new
                ParticipantRepository(repositoryContext));
            _scoreRepository =
                new Lazy<IScoreRepository>(() => new ScoreRepository(repositoryContext));
        }

        public IOrganizationRepository Organization => _organizationRepository.Value;
        public ILeaderboardRepository Leaderboard => _leaderboardRepository.Value;
        public IParticipantRepository Participant => _participantRepository.Value;
        public IScoreRepository Score => _scoreRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}