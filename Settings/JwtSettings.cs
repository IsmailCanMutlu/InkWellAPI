namespace RestApiChallenge.Settings;


    public class JwtSettings
    {
        public const string ConfigName = "JwtSettings";
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string IssuerSigningKey { get; init; }
        public required int TokenExpiredTimeAsMinute { get; init; }
    }
