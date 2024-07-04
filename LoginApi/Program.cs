using LoginApi.Data;
using Microsoft.OpenApi.Models;
using LoginApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("System")));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("system",
         policy => policy.RequireRole("system"));
    options.AddPolicy("admin",
        policy => policy.RequireRole("admin"));
    options.AddPolicy("user",
        policy => policy.RequireRole("user"));
    options.AddPolicy("seller",
        policy => policy.RequireRole("seller"));
    options.AddPolicy("high",
        policy => policy.RequireRole("system", "admin"));
    options.AddPolicy("low",
        policy => policy.RequireRole("admin", "user", "seller"));
    options.AddPolicy("all",
        policy => policy.RequireRole("system", "admin", "user", "seller"));
});


builder.Services.AddIdentityApiEndpoints<IdentityUser>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.SignOutAsync().ConfigureAwait(false);
})
.RequireAuthorization();

app.MapIdentityApi<IdentityUser>();

app.UseAuthorization();

app.MapControllers();

// create the roles and the first system user if not available yet
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;
    var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;

    await DatabaseInitializer.SeedDateAsync(userManager, roleManager);
}

app.MapCategoryEndpoints();

app.Run();
