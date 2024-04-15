namespace starter_template.Helpers;

public class CustomHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public CustomHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    //You can start

}