using System.Net.Http.Json;
using DKGST.Core.Models;

namespace DKGST.Blazor.Services;

public class MenuService
{
    private readonly HttpClient _httpClient;

    public MenuService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Menu>> GetMenusAsync(Guid companyId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/menu/{companyId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Menu>>() ?? new();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return new();
    }
}
