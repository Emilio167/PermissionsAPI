using Application.CQRS.Commands;
using Application.CQRS.Handlers;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using Infrastructure.Search;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios e inyección de dependencias
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PermissionsDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionService,PermissionService>();
builder.Services.AddScoped<IKafkaProducer,KafkaProducer>();
builder.Services.AddTransient<IRequestHandler<RequestPermissionCommand, Unit>, RequestPermissionHandler>();
builder.Services.AddScoped<ElasticsearchService>();
builder.Services.AddMediatR((typeof(RequestPermissionCommand).Assembly));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
