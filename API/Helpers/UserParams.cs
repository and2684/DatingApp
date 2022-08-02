namespace API.Helpers
{
    public class UserParams
    {
        public int PageNumber { get; set; } = 1;

        private const int maxPageSize = 50;
        private int _pageSize = 10;        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }     

        public string CurrentUsername { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
    }
}