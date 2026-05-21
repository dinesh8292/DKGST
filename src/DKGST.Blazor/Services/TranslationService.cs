using System.Text.Json;

namespace DKGST.Blazor.Services;

public class TranslationService
{
    private readonly HttpClient _httpClient;
    private Dictionary<string, string> _currentTranslations = new();
    private string _currentLanguage = "en";

    public TranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SetLanguageAsync(string language)
    {
        _currentLanguage = language;
        // Load translations from API or embedded resource
        _currentTranslations = GetTranslations(language);
    }

    public string GetTranslation(string key)
    {
        return _currentTranslations.TryGetValue(key, out var value) ? value : key;
    }

    public string CurrentLanguage => _currentLanguage;

    private static Dictionary<string, string> GetTranslations(string language)
    {
        return language switch
        {
            "es" => GetSpanishTranslations(),
            "hi" => GetHindiTranslations(),
            _ => GetEnglishTranslations()
        };
    }

    private static Dictionary<string, string> GetEnglishTranslations() => new()
    {
        { "app_title", "DKGST - GST Accounting System" },
        { "login", "Login" },
        { "logout", "Logout" },
        { "accounting", "Accounting" },
        { "inventory", "Inventory" },
        { "invoices", "Invoices" },
        { "products", "Products" },
        { "stock", "Stock" }
    };

    private static Dictionary<string, string> GetSpanishTranslations() => new()
    {
        { "app_title", "DKGST - Sistema de Contabilidad GST" },
        { "login", "Iniciar sesión" },
        { "logout", "Cerrar sesión" },
        { "accounting", "Contabilidad" },
        { "inventory", "Inventario" },
        { "invoices", "Facturas" },
        { "products", "Productos" },
        { "stock", "Stock" }
    };

    private static Dictionary<string, string> GetHindiTranslations() => new()
    {
        { "app_title", "DKGST - जीएसटी लेखांकन प्रणाली" },
        { "login", "प्रवेश करें" },
        { "logout", "बाहर निकलें" },
        { "accounting", "लेखांकन" },
        { "inventory", "इन्वेंटरी" },
        { "invoices", "चालान" },
        { "products", "उत्पाद" },
        { "stock", "स्टॉक" }
    };
}
