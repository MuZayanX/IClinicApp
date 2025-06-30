using IClinicApp.API.Configurations;
using System.Text;
using IClinicApp.API.Data;
using IClinicApp.API.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IClinicApp.API.Repos.Implementations;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IDtoMapper, DtoMapper>();
builder.Services.AddScoped<IHomePageService, HomePageService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<IGovernorateService, GovernorateService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();


// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;

    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// jwt configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

    if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
    {
        throw new InvalidOperationException("JWT settings are not properly configured in appsettings.json.");
    }

    var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

//Add Authorization
builder.Services.AddAuthorizationBuilder()
                       //Add Authorization
                       .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
                       //Add Authorization
                       .AddPolicy("UserOnly", policy => policy.RequireRole("User"))
                       .AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"))
                       //Add Authorization
                       .AddPolicy("UserorAdmin", policy => policy.RequireRole("Admin", "User"));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedRolesAsync(services);
    await DataSeeder.SeedAdminAsync(services);

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = false,
                    Code = 500,
                    Message = "An unexpected error occurred.",
                    Details = error.Error.Message
                });
            }
        });
    });

    app.UseAuthorization();

    app.UseCors("AllowAll");

    app.MapControllers();

    app.Run();
}