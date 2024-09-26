using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Shared;

namespace Service
{
    public class ScoreService: IScoreService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IScoreLinks _scoreLinks;

        public ScoreService(IRepositoryManager repository, ILoggerManager logger, IMapper 
        mapper, IScoreLinks scoreLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _scoreLinks = scoreLinks;
        }
        
        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllScoresAsync(Guid
         leaderboardId, Guid participantId, 
        ScoreLinkParams parameters, bool trackChanges)
        {
            var scores = await _repository.Score.GetAllScoresAsync(parameters.ScoreParameters, 
            trackChanges);
            var scoreDtos = _mapper.Map<IEnumerable<ScoreDto>>(scores);
            var links = _scoreLinks.TryGenerateLinks(scoreDtos, parameters.ScoreParameters
                .Fields, parameters.Context);
            
            return (linkResponse: links, metaData: scores.MetaData);
        }

        public async Task<ScoreDto> GetScoreAsync(Guid scoreId, bool trackChanges)
        {
            var scoreDb = await IsScoreExist(scoreId, trackChanges);
            var score = _mapper.Map<ScoreDto>(scoreDb);
            return score;
        }

        public async Task<ScoreDto> CreateScoreAsync(Guid leaderboardId, Guid participantId,
            ScoreForManipulationDto scoreForCreationDto, bool trackChanges)
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

        public async Task UpdateScoreAsync(Guid scoreId, ScoreForManipulationDto scoreForUpdateDto, bool trackChanges)
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
    }
}