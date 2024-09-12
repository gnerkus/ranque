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
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ParticipantService(IRepositoryManager repository, ILoggerManager logger, IMapper 
        mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<ParticipantDto> GetParticipants(Guid orgId, bool trackChanges)
        {
            var org = _repository.Organization.GetOrganization(orgId, trackChanges);
            if (org is null)
            {
                throw new OrgNotFoundException(orgId);
            }

            var participants = _repository.Participant.GetParticipants(orgId, trackChanges);
            var participantDtos = _mapper.Map<IEnumerable<ParticipantDto>>(participants);
            return participantDtos;
        }

        public ParticipantDto GetParticipant(Guid orgId, Guid pcptId, bool trackChanges)
        {
            var organization = _repository.Organization.GetOrganization(orgId, trackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);
            var participantDb = _repository.Participant.GetParticipant(orgId, pcptId, 
            trackChanges);
            if (participantDb is null)
                throw new ParticipantNotFoundException(pcptId);
            var participant = _mapper.Map<ParticipantDto>(participantDb);
            return participant;
        }

        public ParticipantDto CreateParticipantForOrg(Guid orgId,
            ParticipantForCreationDto participantForCreationDto, bool trackChanges)
        {
            var organization = _repository.Organization.GetOrganization(orgId, trackChanges);
            if (organization is null)
                throw new OrgNotFoundException(orgId);

            var participant = _mapper.Map<Participant>(participantForCreationDto);
            
            _repository.Participant.CreateParticipant(orgId, participant);
            _repository.Save();
            
            return _mapper.Map<ParticipantDto>(participant);
        }
    }
}