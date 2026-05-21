using DKGST.Core.Interfaces;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services;

public class LanguageService : ILanguageService
{
    private readonly MasterDbContext _context;

    public LanguageService(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<string> GetTranslationAsync(string key, string language)
    {
        // Implementation depends on how you store translations
        // This is a placeholder
        return await Task.FromResult(key);
    }

    public async Task<Dictionary<string, string>> GetLanguageAsync(string language)
    {
        // Implementation for loading all translations for a language
        return await Task.FromResult(new Dictionary<string, string>());
    }

    public async Task SetUserLanguageAsync(Guid userId, string language)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            user.PreferredLanguage = language;
            await _context.SaveChangesAsync();
        }
    }
}
