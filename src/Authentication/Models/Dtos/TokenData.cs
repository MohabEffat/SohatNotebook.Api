namespace Authentication.Models.Dtos
{
    public class TokenData
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
