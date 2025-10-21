using Microsoft.EntityFrameworkCore;
using Swachify.Infrastructure.Data;
using Swachify.Application;
using Swachify.Application.Interfaces;
using Swachify.Application.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(opt =>
{
opt.AddPolicy("spa", p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

// Add services
builder.Services.AddControllers().AddJsonOptions(opts =>
{
     //opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
     //opts.JsonSerializerOptions.MaxDepth = 64;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICleaningService, CleaningService>();
builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IOtpService, OtpService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("spa");
app.UseAuthorization();

app.MapControllers();

app.Run();
