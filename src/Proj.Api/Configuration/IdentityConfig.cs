using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proj.Api.Data;
using Proj.Api.Extensions;
using System.Text;

namespace Proj.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Usando um contexto diferente apenas para o Identity, mas usando a mesma connection string do DB do sistema, portanto o mesmo DB
            services.AddDbContext<AplicationDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Adicionando o serviço do Identity
            //Pode ser necessário o package 'Microsoft.AspNetCore.Identity.UI'
            //É possível adicionar qualquer classe que extenda as interfaces corretas no lugar dessas classes genéricas do Identity
            services.AddDefaultIdentity<IdentityUser>()
                //tabela de claims dos users
                .AddRoles<IdentityRole>()
                //define o uso do EF Core                                           
                .AddEntityFrameworkStores<AplicationDbContext>()
                //Classe com novas msgs de erros
                .AddErrorDescriber<IdentityMsgsPtBr>()
                //Recurso que gera tokens, mas não é necessário
                .AddDefaultTokenProviders();


            //Configs do JWT

            //Pegando os parâmetros do Token do arquivo settings
            var appSettingsSection = configuration.GetSection("AppSettingsToken");
            //Add configuração do JWT passando a classe 'AppSettings' já preenchida com os dados preenchidos acima
            services.Configure<AppSettings>(appSettingsSection);

            //Pegando os dados preenchidos
            var appSettings = appSettingsSection.Get<AppSettings>();
            //Criando a key com os dados já codificada
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Add o JWT como esquema geral de autenticação. Vai gerar 1 token para cada User e também validar o token de 
            //entrada de cada user que logar.
            //Necessário add o pacote 'Microsoft.AspNetCore.Authentication.JwtBearer'
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //configs específicas do JWT
            }).AddJwtBearer(opt => 
            {
                //App só pode trabalhar com HTTPS
                opt.RequireHttpsMetadata = true;
                //Salva o token no sistema. Agiliza validações do user
                opt.SaveToken = true;

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//valida se o emissor do token é que recebe-o (aplicação back nos 2)
                    IssuerSigningKey = new SymmetricSecurityKey(key),//criptografa a chave
                    
                    ValidateIssuer = true,//definido abaixo
                    ValidateAudience = true,//definido abaixo
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });


            return services;
        }
    }
}
