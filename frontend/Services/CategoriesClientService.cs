using frontendnet.Models;

namespace frontendnet.Services;

public class CategoriesClientService(HttpClient client)
{
    public async Task<List<Category>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<Category>>("api/categories");
    }

    public async Task<Category?> GetAsync(int id)
    {
        return await client.GetFromJsonAsync<Category>($"api/categories/{id}");
    }

    public async Task<bool> PostAsync(Category category)
    {
        var response = await client.PostAsJsonAsync($"api/categories", category);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync(Category category)
    {
        var response = await client.PutAsJsonAsync($"api/categories/{category.CategoryId}", category);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await client.DeleteAsync($"api/categories/{id}"); 
        return response.IsSuccessStatusCode;
    }
}