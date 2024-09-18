namespace Shared
{
    public abstract class RequestParameters
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? OrderBy { get; set; }
        public string? Fields { get; set; }
    }

    public class ParticipantParameters : RequestParameters
    {
        public ParticipantParameters()
        {
            OrderBy = "name";
        }

        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;

        public bool ValidAgeRange => MaxAge > MinAge;

        public string? SearchTerm { get; set; }
    }
}