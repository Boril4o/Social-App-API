using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialApp.infrastructure.Data;
using SocialApp.infrastructure.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null )
{
    throw new Exception("connectionString is null");
}

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
