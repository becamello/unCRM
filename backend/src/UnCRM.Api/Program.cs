using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UnCRM.Api.AutoMapper;
using UnCRM.Api.Contract.Pessoa;
using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Classes;
using UnCRM.Api.Domain.Repository.Interfaces;
using UnCRM.Api.Domain.Services.Classes;
using UnCRM.Api.Domain.Services.Interfaces;
using UnCRM.Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);

ConfigurarServices(builder);
ConfigurarInjecaoDeDependencia(builder);

var app = builder.Build();

ConfigurarAplicacao(app);

app.Run();

static void ConfigurarInjecaoDeDependencia(WebApplicationBuilder builder)
{
    string connectionString = builder.Configuration.GetConnectionString("PADRAO");

    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseNpgsql(connectionString),
        ServiceLifetime.Transient);

    builder.Services.AddAutoMapper(typeof(UsuarioProfile), typeof(PessoaProfile));

    builder.Services.AddHttpContextAccessor();

    builder.Services
        .AddSingleton(builder.Configuration)
        .AddSingleton(builder.Environment)
        .AddScoped<ApplicationContext>()
        .AddScoped<TokenService>()
        .AddScoped<IUsuarioService, UsuarioService>()
        .AddScoped<AtendimentoService>()
        .AddScoped<IService<PessoaRequestContract, PessoaResponseContract, long>, PessoaService>()
        .AddScoped<IUsuarioRepository, UsuarioRepository>()
        .AddScoped<IAtendimentoRepository, AtendimentoRepository>()
        .AddScoped<IRepository<TipoAtendimento, long>, TipoAtendimentoRepository>()
        .AddScoped<IPessoaRepository, PessoaRepository>();
}

static void ConfigurarServices(WebApplicationBuilder builder)
{
    builder.Services
        .AddCors()
        .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "UnCRM.Api",
            Version = "v1",
            Description = "Documentação da API do unCRM."
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header usando o esquema Bearer (Ex: 'Bearer 12345abcdef')",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration["KeySecret"])
                ),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}

static void ConfigurarAplicacao(WebApplication app)
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.ContentType = "application/json";

            var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            if (exception is ValidationResultException validationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = validationException.Errors.Select(e => new
                {
                    Campo = e.PropertyName,
                    Mensagem = e.ErrorMessage
                });

                var response = new
                {
                    Message = validationException.Message,
                    Errors = errors
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    Message = "Ocorreu um erro interno no servidor."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        });
    });

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UnCRM.Api v1");
    });

    app.UseRouting();

    app.UseCors(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}