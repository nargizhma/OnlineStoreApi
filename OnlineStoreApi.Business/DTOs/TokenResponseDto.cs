namespace OnlineStoreApi.Business.DTOs
{
    public class TokenResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
