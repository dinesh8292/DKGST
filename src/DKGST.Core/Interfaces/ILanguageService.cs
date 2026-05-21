namespace DKGST.Core.Interfaces;

public interface ILanguageService
{
    Task<string> GetTranslationAsync(string key, string language);
    Task<Dictionary<string, string>> GetLanguageAsync(string language);
    Task SetUserLanguageAsync(Guid userId, string language);
}
