using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingApplication.Mapping;
using Ten.Services.BookingInfrastructure.Data;
using Ten.Services.BookingInfrastructure.Repositories;
using FluentValidation.AspNetCore;
using Ten.Services.BookingAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("Ten.Services.BookingApplication")));


// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(Assembly.Load("Ten.Services.BookingApplication"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Add services to the container.

builder.Services.AddControllers();
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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
