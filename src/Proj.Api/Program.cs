using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Proj.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Services

builder.Services.AddDbContext<MeuDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();
builder.Services.ResolveDependencies();

var app = builder.Build();

// Configure Services

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
