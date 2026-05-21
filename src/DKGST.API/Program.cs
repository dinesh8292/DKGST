using DKGST.Infrastructure;
using DKGST.Infrastructure.Data;
using DKGST.Infrastructure.Extensions;
using DKGST.Infrastructure.Seeders;
using DKGST.Core.Services;
using DKGST.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "PostgreSQL";

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7001", "http://localhost:5002")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var masterConnectionString = builder.Configuration.GetConnectionString("MasterDb");
var companyConnectionString = builder.Configuration.GetConnectionString("CompanyDb");

if (databaseProvider == "SqlServer")
{
    builder.Services.AddDbContext<MasterDbContext>(options =>
        options.UseSqlServer(masterConnectionString));
    builder.Services.AddDbContext<CompanyDbContext>(options =>
        options.UseSqlServer(companyConnectionString));
}
else
{
    builder.Services.AddDbContext<MasterDbContext>(options =>
        options.UseNpgsql(masterConnectionString));
    builder.Services.AddDbContext<CompanyDbContext>(options =>
        options.UseNpgsql(companyConnectionString));
}

var secret = jwtSettings["Secret"];
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Register Core Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();

// Register Module Services
builder.Services.AddAccountingServices();
builder.Services.AddInventoryServices();

builder.Services.AddLocalization();
var supportedCultures = new[] { "en", "es", "hi" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DKGST API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DKGST API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();

// Apply Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var masterDbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    var companyDbContext = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();

    // Apply migrations
    await masterDbContext.Database.MigrateAsync();
    await companyDbContext.Database.MigrateAsync();

    // Seed master data
    await DatabaseSeeder.SeedMasterDataAsync(masterDbContext);

    // Seed company data (for all existing companies)
    var companies = await masterDbContext.Companies.ToListAsync();
    foreach (var company in companies)
    {
        await DatabaseSeeder.SeedCompanyDataAsync(companyDbContext, company.Id);
    }
}

app.MapControllers();

app.Run();
