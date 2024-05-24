using frontendnet.Models;

namespace frontendnet.Services;

public class UsersClientService(HttpClient client)
{
    public async Task<List<User>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<User>>("api/users");
    }

    public async Task<User?> GetAsync(string email)
    {
        return await client.GetFromJsonAsync<User>($"api/users/{email}");
    }

    public async Task<bool> PostAsync(UserPwd user)
    {
        var response = await client.PostAsJsonAsync($"api/users", user); return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync(User user)
    {
        var response = await client.PutAsJsonAsync($"api/users/{user.Email}", user); return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteAsync(string email)
    {
        var response = await client.DeleteAsync($"api/users/{email}"); return response.IsSuccessStatusCode;
    }
}