using DKGST.Core.Interfaces.Accounting;
using DKGST.Core.Interfaces.Inventory;
using DKGST.Core.Services.Accounting;
using DKGST.Core.Services.Inventory;
using Microsoft.Extensions.DependencyInjection;

namespace DKGST.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAccountingServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IGstReportService, GstReportService>();
        services.AddScoped<IJournalEntryService, JournalEntryService>();
        return services;
    }

    public static IServiceCollection AddInventoryServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStockService, StockService>();
        return services;
    }
}
