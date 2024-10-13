using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Shared;

namespace Service
{
    internal sealed class ParticipantService : IParticipantService
    {
        private readonly IMapper _mapper;
        private readonly IParticipantLinks _participantLinks;
        private readonly IRepositoryManager _repository;

        public ParticipantService(IRepositoryManager repository, ILoggerManager logger, IMapper
            mapper, IParticipantLinks participantLinks)
        {
            _repository = repository;
            _mapper = mapper;
            _participantLinks = participantLinks;
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)>
            GetParticipantsAsync(Guid orgId,
                LinkParameters parameters, bool trackChanges)
        {
            if (!parameters.ParticipantParameters.ValidAgeRange)
                throw new
                    MaxAgeBadRequestException();

            await IsOrgExist(orgId, trackChanges);

            var participants =
                await _repository.Participant.GetParticipantsAsync(orgId,
                    parameters.ParticipantParameters,
                    trackChanges);
            var participantDtos = _mapper.Map<IEnumerable<ParticipantDto>>(participants);
            var links = _participantLinks.TryGenerateLinks(participantDtos, parameters
                .ParticipantParameters.Fields!, orgId, parameters.Context);

            return (linkResponse: links, metaData: participants.MetaData);
        }

        public async Task<ParticipantDto> GetParticipantAsync(Guid orgId, Guid pcptId, bool
            trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);
            var participantDb = await IsParticipantExist(orgId, pcptId, trackChanges);

            var participant = _mapper.Map<ParticipantDto>(participantDb);
            return participant;
        }

        public async Task<IEnumerable<LeaderboardDto>> GetLeaderboardsAsync(Guid participantId, bool trackChanges)
        {
            var leaderboards =
                await _repository.Participant.GetLeaderboardsAsync(participantId, trackChanges);

            return _mapper.Map<IEnumerable<LeaderboardDto>>(leaderboards);
        }

        public async Task<ParticipantDto> CreateParticipantForOrgAsync(Guid orgId,
            ParticipantForCreationDto participantForCreationDto, bool trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);

            var participant = _mapper.Map<Participant>(participantForCreationDto);

            _repository.Participant.CreateParticipant(orgId, participant);
            await _repository.SaveAsync();

            return _mapper.Map<ParticipantDto>(participant);
        }

        public async Task DeleteParticipantForOrgAsync(Guid orgId, Guid participantId, bool
            trackChanges)
        {
            await IsOrgExist(orgId, trackChanges);

            var participantDb = await IsParticipantExist(orgId, participantId, trackChanges);

            _repository.Participant.DeleteParticipant(participantDb);
            await _repository.SaveAsync();
        }

        public async Task UpdateParticipantForOrgAsync(Guid orgId, Guid participantId,
            ParticipantForUpdateDto participantForUpdateDto, bool orgTrackChanges,
            bool pcptTrackChanges)
        {
            await IsOrgExist(orgId, orgTrackChanges);

            var participantDb = await IsParticipantExist(orgId, participantId, pcptTrackChanges);

            _mapper.Map(participantForUpdateDto, participantDb);
            await _repository.SaveAsync();
        }

        public async Task<(ParticipantForUpdateDto participantToPatch, Participant participant)>
            GetParticipantForPatchAsync(
                Guid orgId, Guid participantId, bool orgTrackChanges, bool participantTrackChanges)
        {
            await IsOrgExist(orgId, orgTrackChanges);

            var participantDb =
                await IsParticipantExist(orgId, participantId, participantTrackChanges);

            var participantToPatch = _mapper.Map<ParticipantForUpdateDto>(participantDb);
            return (participantToPatch, participantDb);
        }

        public async Task SaveChangesForPatchAsync(ParticipantForUpdateDto participantToPatch,
            Participant participant)
        {
            _mapper.Map(participantToPatch, participant);
            await _repository.SaveAsync();
        }

        private async Task IsOrgExist(Guid orgId, bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
            if (org is null) throw new OrgNotFoundException(orgId);
        }

        private async Task<Participant> IsParticipantExist(Guid orgId, Guid participantId,
            bool trackChanges)
        {
            var participantDb = await _repository.Participant.GetParticipantForOrgAsync(orgId,
                participantId,
                trackChanges);
            if (participantDb is null)
                throw new ParticipantNotFoundException(participantId);

            return participantDb;
        }
    }
}