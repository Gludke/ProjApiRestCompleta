using DevIO.Data.Context;
using Proj.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();
builder.Services.ResolveDependencies();

builder.Services.AddDbContext<MeuDbContext>(options =>
{

});

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
