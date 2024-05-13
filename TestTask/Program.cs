using Application;
using Application.Services;
using Core.Configurations;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFlatAdsService, FlatAdsService>();
builder.Services.AddScoped<IKufarApiAccessor, KufarApiAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.Configure<KufarApiRequestConfiguration>(
    builder.Configuration.GetSection(nameof(KufarApiRequestConfiguration)));

builder.Services.Configure<CacheConfiguration>(
    builder.Configuration.GetSection(nameof(CacheConfiguration)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
