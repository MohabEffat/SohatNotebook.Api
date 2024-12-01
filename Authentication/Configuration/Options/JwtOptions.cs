namespace SohatNotebook.Authentication.Configuration.Options
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}
