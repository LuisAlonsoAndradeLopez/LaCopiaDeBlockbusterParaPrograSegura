using frontendnet.Models;

namespace frontendnet.Services;

public class MoviesClientService(HttpClient client)
{
    public async Task<List<Movie>?> GetAsync(string? search)
    {
        return await client.GetFromJsonAsync<List<Movie>>($"api/movies?s={search}");
    }
    public async Task<Movie?> GetAsync(int id)
    {
        return await client.GetFromJsonAsync<Movie>($"api/movies/{id}");
    }
    public async Task<bool> PostAsync(Movie movie)
    {
        var response = await client.PostAsJsonAsync($"api/movies", movie); 
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> PutAsync(Movie movie)
    {
        var response = await client.PutAsJsonAsync($"api/movies/{movie.MovieId}", movie); 
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var response = await client.DeleteAsync($"api/movies/{id}"); 
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> PostAsync(int id, int categoryid)
    {
        var response = await client.PostAsJsonAsync($"api/movies/{id}/category", new { categoryid }); 
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id, int categoryid)
    {
        var response = await client.DeleteAsync($"api/movies/{id}/category/{categoryid}");
        return response.IsSuccessStatusCode;
    }
}