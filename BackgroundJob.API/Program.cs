using BackgroundJob.API.Services;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Hangfire Service
builder.Services.AddHangfire(configuration =>
{
    configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});

// Add processing server as IHostedServer
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IJobTestService, JobTestService>();

var app = builder.Build();

// Kích hoạt giao diện quản lý, theo giõi và kiểm soát các Background Job 
app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapHangfireDashboard();   
}

app.UseAuthorization();

app.MapControllers();

app.Run();
