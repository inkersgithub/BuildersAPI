using InkersCore.Domain;
using InkersCore.Domain.IRepositories;
using InkersCore.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InkersCore.Infrastructure;
using InkersCore.Domain.IServices;
using InkersCore.Services;
using Serilog;
using Serilog.Extensions.Hosting;
using InkersCore.Common;

Log.Logger = RegisterLoggerService();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
RegisterAuthenticationProvider(builder);
RegisterDatabaseContext(builder);
RegisterServices(builder);
RegisterExternalServices(builder);
RegisterDatabaseServices(builder);
ConfigureCors(builder);
var app = builder.Build();
JsonWebTokenHandler.AppSettingConfigure(app.Services.GetRequiredService<IConfiguration>());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyCorsPolicy");
app.UseHttpsRedirection();
//app.UseMiddleware<TokenInterceptorMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

//Register Services
void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<AuthManager, AuthManager>();
    builder.Services.AddScoped<PermissionManager, PermissionManager>();
    builder.Services.AddScoped<ServiceManager, ServiceManager>();
    builder.Services.AddScoped<CompanyManager, CompanyManager>();
    builder.Services.AddScoped<CustomerManager, CustomerManager>();
    builder.Services.AddScoped<SubscriptionManager, SubscriptionManager>();
    builder.Services.AddScoped<OrderManager, OrderManager>();
}

//Register External Services
void RegisterExternalServices(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<ITokenCacheService, RedisCacheService>();
    builder.Services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));
    builder.Services.AddSingleton<IEmailService, SendGridEmailService>();
    builder.Services.AddSingleton<IDatabaseService, MySqlDatabaseService>();
}

//Register Database Services
void RegisterDatabaseServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
    builder.Services.AddScoped<IAuditRepository, AuditRepository>();
    builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<ISubscriptionRepository, SubsciptionRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
}

//Register Database context
void RegisterDatabaseContext(WebApplicationBuilder builder)
{
    builder.Services.AddDbContextPool<AppDBContext>(options =>
    options.UseMySql(builder.Configuration["ConnectionStrings:MySql"],
    ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:MySql"])));
}

//Register Logger Service
ReloadableLogger RegisterLoggerService()
{
    return new LoggerConfiguration()
    .WriteTo.File(path: @"C:\log\InkersCore.log",
              rollingInterval: RollingInterval.Day,
              rollOnFileSizeLimit: true,
              fileSizeLimitBytes: 5000000)
    .CreateBootstrapLogger();
}

//Register Authentication Provider
void RegisterAuthenticationProvider(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidIssuer = "api.inkers.com",
            ValidAudience = "www.inkers.com",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("InkersSecretKey@123456"))
        };
    });
}

void ConfigureCors(WebApplicationBuilder builder)
{
    builder?.Services.AddCors(options =>
    {
        options.AddPolicy("MyCorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}