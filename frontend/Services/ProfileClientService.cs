namespace frontendnet.Services;

public class ProfileClientService(HttpClient client)
{
    public async Task<string> ObtainTimeAsync()
    {
        return await client.GetStringAsync($"api/auth/time");
    }
}