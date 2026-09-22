using RadarWeb.Net.ApuracaoAssistida.Application.Interfaces;
using RadarWeb.Net.ApuracaoAssistida.Infrastructure.Data;
using RadarWeb.Net.ApuracaoAssistida.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient("ReceitaCbs");
builder.Services.AddDbContext<ApuracaoAssistidaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApuracaoAssistida")));

builder.Services.AddScoped<IApuracaoCbsRepository, ApuracaoCbsRepository>();
builder.Services.AddScoped<IRetornoCbsService, RetornoCbsService>();

var app = builder.Build();

app.MapControllers();

app.Run();
