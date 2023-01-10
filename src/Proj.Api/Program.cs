using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proj.Api.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ########## Add Services ##########

builder.Services.AddDbContext<MeuDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Impede que a API fa�a as valida��es autom�ticas dela, dessa forma apenas as nossas
//valida��es ser�o utilizadas, permitindo padroniza��o de erros.
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

//Pol�ticas de acesso � API
builder.Services.AddCors(options =>
{
    //Pol�ticas de nome 'Development': acesso total atribu�do nessas pol�ticas
    options.AddPolicy("Development",
        builder =>
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());


    //options.AddPolicy("Production",
    //    builder =>
    //        builder
    //            .WithMethods("GET")
    //            .WithOrigins("http://desenvolvedor.io")
    //            .SetIsOriginAllowedToAllowWildcardSubdomains()
    //            //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
    //            .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen();

builder.Services.ResolveDependencies();

var app = builder.Build();



// ########## Configure Services ##########

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Usando a pol�tica de acesso 'Development' configurada acima
app.UseCors("Development");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
