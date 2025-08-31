namespace Library.Application.Configuration;

public class AppSettings
{
    public JwtSettings Jwt { get; set; } = new();
    public EmailSettings Email { get; set; } = new();
    public DatabaseSettings Database { get; set; } = new();
    public SecuritySettings Security { get; set; } = new();
    public RateLimitSettings RateLimit { get; set; } = new();
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; } = 3;
    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
}

public class SecuritySettings
{
    public bool RequireHttps { get; set; } = true;
    public int PasswordMinLength { get; set; } = 8;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireSpecialCharacter { get; set; } = true;
    public int MaxLoginAttempts { get; set; } = 5;
    public int LockoutDurationMinutes { get; set; } = 15;
}

public class RateLimitSettings
{
    public bool EnableRateLimiting { get; set; } = true;
    public int PerSecondLimit { get; set; } = 100;
    public int PerMinuteLimit { get; set; } = 1000;
    public int PerHourLimit { get; set; } = 10000;
}

