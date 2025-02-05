using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using P7CreateRestApi;
using P7CreateRestApi.Data.Repositories;
using P7CreateRestApi.Data.Services;
using P7CreateRestApi.Mapping;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Please enter the token in the format: Bearer {your_token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


#region Authentifications & Logs

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        // Assure-toi que les types de claim utilisés sont ceux que tu as définis lors de la création du token
        NameClaimType = JwtRegisteredClaimNames.Sub, // ou "unique_name" selon ton token
        RoleClaimType = ClaimTypes.Role
    };
});



builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<LocalDbContext>()
    .AddDefaultTokenProviders();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext() // Conserve les propriétés du contexte de log, y compris Username
    .Enrich.With(new UsernameEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>()))
    .WriteTo.File(
        Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} (User: {Username}){NewLine}{Exception}"
    )
    .CreateLogger();



builder.Host.UseSerilog(); // Use Serilog for logging

builder.Services.AddLogging(builder =>
{
    builder.ClearProviders(); // Clear default providers
    builder.AddSerilog(dispose: true); // Use Serilog
});
#endregion

#region Services

builder.Services.AddScoped<IBidListService, BidListService>();
builder.Services.AddScoped<IBidListRepository, BidListRepository>();
builder.Services.AddScoped<ICurvePointService, CurvePointService>();
builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
builder.Services.AddScoped<IRuleNameService, RuleNameService>();
builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion


builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        // Ajoute l'utilisateur dans les logs HTTP
        var username = httpContext.User?.Identity?.Name;
        if (username != null)
        {
            diagnosticContext.Set("Username", username);
        }
        else
        {
            diagnosticContext.Set("Username", "Anonymous");
        }
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();
