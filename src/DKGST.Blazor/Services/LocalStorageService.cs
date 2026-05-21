using System.Text.Json;

namespace DKGST.Blazor.Services;

public class LocalStorageService : ILocalStorageService
{
    private readonly HttpClient _httpClient;

    public LocalStorageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        // In a real Blazor WebAssembly app, use JS interop for localStorage
        // This is a placeholder implementation
        await Task.CompletedTask;
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        // In a real Blazor WebAssembly app, use JS interop for localStorage
        return await Task.FromResult(default(T?));
    }

    public async Task RemoveItemAsync(string key)
    {
        // In a real Blazor WebAssembly app, use JS interop for localStorage
        await Task.CompletedTask;
    }
}
