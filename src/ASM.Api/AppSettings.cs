namespace ASM.Api;

public class AppSettings
{
    public CorsSettings CorsSettings { get; set; } = new();
}

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = [];
}
