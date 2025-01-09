using Asp.Versioning.ApiExplorer;
using AspNetCoreRateLimit;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Data.AppDbContext;
using ModularPatternTraining.Modules.BookModule.Services;
using ModularPatternTraining.Modules.LibraryModule.DataAccess;
using ModularPatternTraining.Modules.LibraryModule.Services;
using Serilog;
using ModularPatternTraining.Middlewares;
using ModularPatternTraining.Shared.DataAccess;
using Microsoft.AspNetCore.Identity;
using ModularPatternTraining.Modules.Authentication.DataAccess;
using ModularPatternTraining.Modules.Authentication.Services;
using ModularPatternTraining.Modules.BookModule.DataAccess;
using ModularPatternTraining.Modules.BookModule.minimal;
using ModularPatternTraining.Modules.UserManagement.DataAccess;
using ModularPatternTraining.Modules.UserManagement.Model;
using ModularPatternTraining.Modules.UserManagement.Service;
using ModularPatternTraining.Shared.Services;
using ModularPatternTraining.Shared.Services.CacheService;
using ModularPatternTraining.Shared.Services.JWTService;
using ModularPatternTraining.Modules.RoleManagement.DataAccess;
using ModularPatternTraining.Modules.RoleManagement.Service;
using ModularPatternTraining.Shared.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region AddDbcontext

var connectionString = builder.Configuration.GetConnectionString("ModularTraining");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


#endregion

#region Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();



//Dependency Injection

#region MainRepository & Services
builder.Services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
#endregion

#region Book

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
#endregion

#region Library

builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();


#endregion

#region UserManagement

builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();

#endregion

#region Auth

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion

#region Role
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
#endregion

//Versioning    
builder.Services.AddApiVersioningConfiguration();

//JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    }
    
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field. Example: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});
OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
{
    var info = new OpenApiInfo()
    {
        Title = "My API",
        Version = description.ApiVersion.ToString()
    };
    return info;
}

//Logger
SerilogConfiguration.ConfigureLogging();  
builder.Host.UseSerilog(); 
builder.Services.AddControllersWithViews();

//AuthorizationPolicies
builder.Services.AddAuthorization(AuthorizationPolicies.AddCustomPolicies);

//Rate Limit Configuration
builder.Services.AddControllersWithViews();

builder.Services.AddRateLimitingServices(builder.Configuration);
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();


var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseIpRateLimiting();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapBooksEndpoints();

app.Run();
