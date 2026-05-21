using System.Net.Http.Json;
using DKGST.Core.Models;

namespace DKGST.Blazor.Services;

public class CompanyService
{
    private readonly HttpClient _httpClient;

    public CompanyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CompanyDto>> GetUserCompaniesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/company");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<CompanyDto>>() ?? new();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return new();
    }
}
