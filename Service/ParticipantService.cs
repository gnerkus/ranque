using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Shared;

namespace Service
{
    internal sealed class ParticipantService : IParticipantService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;

        public ParticipantService(IRepositoryManager repository, ILoggerManager logger, IMapper
            mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipantsAsync(Guid orgId, bool 
        trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
            if (org is null) throw new OrgNotFoundException(orgId);

            var participants = await _repository.Participant.GetParticipantsAsync(orgId, 
            trackChanges);
            var participantDtos = _mapper.Map<IEnumerable<ParticipantDto>>(participants);
            return participantDtos;
        }

        public async Task<ParticipantDto> GetParticipantAsync(Guid orgId, Guid pcptId, bool 
        trackChanges)
        {
            var organization = await _repository.Organization.GetOrganizationAsync(orgId, 
            trackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);
            var participantDb = await _repository.Participant.GetParticipantAsync(orgId, pcptId,
                trackChanges);
            if (participantDb is null)
                throw new ParticipantNotFoundException(pcptId);
            var participant = _mapper.Map<ParticipantDto>(participantDb);
            return participant;
        }

        public async Task<ParticipantDto> CreateParticipantForOrgAsync(Guid orgId,
            ParticipantForCreationDto participantForCreationDto, bool trackChanges)
        {
            var organization = await _repository.Organization.GetOrganizationAsync(orgId, 
            trackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);

            var participant = _mapper.Map<Participant>(participantForCreationDto);

            _repository.Participant.CreateParticipant(orgId, participant);
            await _repository.SaveAsync();

            return _mapper.Map<ParticipantDto>(participant);
        }

        public async Task DeleteParticipantForOrgAsync(Guid orgId, Guid participantId, bool 
        trackChanges)
        {
            var organization = await _repository.Organization.GetOrganizationAsync(orgId, 
            trackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);

            var participant =
                await _repository.Participant.GetParticipantAsync(orgId, participantId, 
                trackChanges);
            if (participant is null) throw new ParticipantNotFoundException(participantId);

            _repository.Participant.DeleteParticipant(participant);
            await _repository.SaveAsync();
        }

        public async Task UpdateParticipantForOrgAsync(Guid orgId, Guid participantId,
            ParticipantForUpdateDto participantForUpdateDto, bool orgTrackChanges,
            bool pcptTrackChanges)
        {
            var organization = await _repository.Organization.GetOrganizationAsync(orgId, 
            orgTrackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);

            var participant =
                await _repository.Participant.GetParticipantAsync(orgId, participantId, 
                pcptTrackChanges);
            if (participant is null) throw new ParticipantNotFoundException(participantId);

            _mapper.Map(participantForUpdateDto, participant);
            await _repository.SaveAsync();
        }

        public async Task<(ParticipantForUpdateDto participantToPatch, Participant participant)>
            GetParticipantForPatchAsync(
                Guid orgId, Guid participantId, bool orgTrackChanges, bool participantTrackChanges)
        {
            var organization = await _repository.Organization.GetOrganizationAsync(orgId, 
            orgTrackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);

            var participant =
                await _repository.Participant.GetParticipantAsync(orgId, participantId,
                    participantTrackChanges);
            if (participant is null) throw new ParticipantNotFoundException(participantId);

            var participantToPatch = _mapper.Map<ParticipantForUpdateDto>(participant);
            return (participantToPatch, participant);
        }

        public async Task SaveChangesForPatchAsync(ParticipantForUpdateDto participantToPatch,
            Participant participant)
        {
            _mapper.Map(participantToPatch, participant);
            await _repository.SaveAsync();
        }
    }
}