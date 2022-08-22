namespace API.DTOs
{
    public class LikeDto
    {
        public int Id { get; set; }        
        public string Username { get; set; } = string.Empty;
        public int Age { get; set; }
        public string KnownAs { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}