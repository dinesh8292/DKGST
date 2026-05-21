using System.Net.Http.Json;
using DKGST.Core.Models;

namespace DKGST.Blazor.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private UserDto? _currentUser;
    private Guid _currentCompanyId;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var request = new LoginRequest { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        var result = await response.Content.ReadAsAsync<LoginResponse>();

        if (result.Success && result.Token != null)
        {
            await _localStorage.SetItemAsync("authToken", result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
            _currentUser = result.User;
            if (result.User?.Companies.FirstOrDefault(c => c.IsDefault) is CompanyDto defaultCompany)
            {
                _currentCompanyId = defaultCompany.Id;
            }
        }

        return result;
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("api/auth/logout", null);
        await _localStorage.RemoveItemAsync("authToken");
        _currentUser = null;
        _currentCompanyId = Guid.Empty;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            // Parse JWT to get user info (simplified)
            _currentUser = new UserDto(); // Implement proper JWT parsing
        }

        return _currentUser;
    }

    public async Task SetCurrentCompanyAsync(Guid companyId)
    {
        _currentCompanyId = companyId;
        await _localStorage.SetItemAsync("currentCompanyId", companyId.ToString());
    }

    public async Task<Guid> GetCurrentCompanyIdAsync()
    {
        if (_currentCompanyId != Guid.Empty)
            return _currentCompanyId;

        var companyIdStr = await _localStorage.GetItemAsync<string>("currentCompanyId");
        if (!string.IsNullOrEmpty(companyIdStr) && Guid.TryParse(companyIdStr, out var companyId))
        {
            _currentCompanyId = companyId;
        }

        return _currentCompanyId;
    }

    public async Task<CompanyDto?> GetCurrentCompanyAsync()
    {
        if (_currentUser == null)
            return null;

        var companyId = await GetCurrentCompanyIdAsync();
        return _currentUser.Companies.FirstOrDefault(c => c.Id == companyId);
    }
}
