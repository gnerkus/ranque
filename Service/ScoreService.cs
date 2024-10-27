using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Shared;

namespace Service
{
    public class ScoreService : IScoreService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly IScoreLinks _scoreLinks;

        public ScoreService(IRepositoryManager repository, ILogger<IApiService> logger, IMapper
            mapper, IScoreLinks scoreLinks)
        {
            _repository = repository;
            _mapper = mapper;
            _scoreLinks = scoreLinks;
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllScoresAsync(
            ScoreLinkParams parameters, bool trackChanges)
        {
            var scores = await _repository.Score.GetAllScoresAsync(parameters.ScoreParameters,
                trackChanges);
            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            var links = _scoreLinks.TryGenerateLinks(scoreDtos, parameters.ScoreParameters
                .Fields!, parameters.Context);

            return (linkResponse: links, metaData: scores.MetaData);
        }

        public async Task<ScoreDto> GetScoreAsync(Guid scoreId, bool trackChanges)
        {
            var scoreDb = await IsScoreExist(scoreId, trackChanges);
            var score = _mapper.Map<ScoreDto>(scoreDb);
            return score;
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)>
            GetParticipantScoresAsync(Guid participantId, ScoreLinkParams parameters,
                bool trackChanges)
        {
            var scores = await _repository.Score.GetParticipantScoresAsync(participantId,
                parameters.ScoreParameters,
                trackChanges);
            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            var links = _scoreLinks.TryGenerateLinks(scoreDtos, parameters.ScoreParameters
                .Fields!, parameters.Context);

            return (linkResponse: links, metaData: scores.MetaData);
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)>
            GetLeaderboardScoresAsync(Guid leaderboardId, ScoreLinkParams parameters,
                bool trackChanges)
        {
            var scores = await _repository.Score.GetLeaderboardScoresAsync(leaderboardId,
                parameters
                    .ScoreParameters,
                trackChanges);
            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            var links = _scoreLinks.TryGenerateLinks(scoreDtos, parameters.ScoreParameters
                .Fields!, parameters.Context);

            return (linkResponse: links, metaData: scores.MetaData);
        }

        public async Task<bool> CheckScoreOrg(Guid leaderboardId, Guid participantId,
            bool trackChanges)
        {
            var participant = await IsParticipantExist(participantId, trackChanges);
            var leaderboard = await IsLeaderboardExist(leaderboardId, trackChanges);

            return participant.OrganizationId.Equals(leaderboard.OrganizationId);
        }

        public async Task<ScoreDto> CreateScoreAsync(Guid leaderboardId, Guid participantId,
            ScoreForCreationDto scoreForCreationDto, bool trackChanges)
        {
            var score = _mapper.Map<Score>(scoreForCreationDto);
            _repository.Score.CreateScore(leaderboardId, participantId, score);
            await _repository.SaveAsync();

            return _mapper.Map<ScoreDto>(score);
        }

        public async Task DeleteScoreAsync(Guid scoreId, bool trackChanges)
        {
            var scoreDb = await IsScoreExist(scoreId, trackChanges);
            _repository.Score.DeleteScore(scoreDb);
            await _repository.SaveAsync();
        }

        public async Task UpdateScoreAsync(Guid scoreId, ScoreForManipulationDto scoreForUpdateDto,
            bool trackChanges)
        {
            var scoreDb = await IsScoreExist(scoreId, trackChanges);
            _mapper.Map(scoreForUpdateDto, scoreDb);
            await _repository.SaveAsync();
        }

        private async Task<Score> IsScoreExist(Guid scoreId,
            bool trackChanges)
        {
            var scoreDb = await _repository.Score.GetScoreAsync(scoreId,
                trackChanges);
            if (scoreDb is null)
                throw new ScoreNotFoundException(scoreId);

            return scoreDb;
        }

        private async Task<Participant> IsParticipantExist(Guid participantId, bool trackChanges)
        {
            var pcpt = await _repository.Participant.GetParticipantAsync(participantId,
                trackChanges);
            if (pcpt is null) throw new ParticipantNotFoundException(participantId);
            return pcpt;
        }

        private async Task<Leaderboard> IsLeaderboardExist(Guid leaderboardId, bool trackChanges)
        {
            var leaderboard = await _repository.Leaderboard.GetLeaderboardAsync(leaderboardId,
                trackChanges);
            if (leaderboard is null) throw new LeaderboardNotFoundException(leaderboardId);
            return leaderboard;
        }
    }
}