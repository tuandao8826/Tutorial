using Microsoft.EntityFrameworkCore;
using Tutorial.Commons.Extentions;
using Tutorial.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect Database
builder.Services.AddDbContext<ProductDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

// Add Repository Extention
builder.Services.AddRepositoryExtention();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
