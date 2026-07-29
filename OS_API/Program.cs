using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OS_API.Data;
using OS_API.Data.Seed;
using OS_API.Exceptionn;
using OS_API.Helpers.Constantes;
using OS_API.Helpers.UsuarioLogado;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Models;
using OS_API.Repositories;
using OS_API.Services;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Configura o Entity Framework para usar PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configura��o do ASP.NET Core Identity
builder.Services
    .AddIdentity<UsuarioModel, IdentityRole>(options =>
    {
        // Configuracoes de senha 
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        // Exigir e-mail �nico
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configuracao jwt
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userManager = context.HttpContext.RequestServices
                    .GetRequiredService<UserManager<UsuarioModel>>();

                var userId = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var stampNoToken = context.Principal?.FindFirstValue("SecurityStamp");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(stampNoToken))
                {
                    context.Fail("Token inválido.");
                    return;
                }

                var usuario = await userManager.FindByIdAsync(userId);

                if (usuario == null || usuario.SecurityStamp != stampNoToken)
                {
                    context.Fail("Sessão expirada. Faça login novamente.");
                }
            }
        };
    });

//chamar a classe de politicas permiss�o
builder.Services.AddAuthorization(options =>
{
    PoliticasPermissao.AdicionarPoliticas(options);
});

// Registrar depend�ncias
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnidadeTrabalho, UnidadeTrabalho>();
builder.Services.AddScoped<IPermissaoRepository, PermissaoRepository>();
builder.Services.AddScoped<IOsFuncionarioRepository, OsFuncionarioRepository>();
builder.Services.AddScoped<IOsFuncionarioService, OsFuncionarioService>();
builder.Services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
builder.Services.AddScoped<IOrdemServicoService, OrdemServicoService>();
builder.Services.AddScoped<ITipoAtendimentoRepository, TipoAtendimentoRepository>();
builder.Services.AddScoped<ITipoAtendimentoService, TipoAtendimentoService>();
builder.Services.AddScoped<IAssinaturaOsRepository, AssinaturaOsRepository>();

// Cliente + integracao com o ViaCEP
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddHttpClient<IViaCepService, ViaCepService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUsuarioLogado, UsuarioLogado>();

// Configuracao CORS para permitir o Front-end acessar a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEnd", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });


    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    options.OrderActionsBy(apiDesc =>
    {
        var controller = apiDesc.ActionDescriptor.RouteValues["controller"];

        var prioridadeVerbo = apiDesc.HttpMethod switch
        {
            "POST" => "1", 
            "GET" => "2", 
            "PUT" => "3", 
            "PATCH" => "4",
            "DELETE" => "5",
            _ => "6"
        };

        return $"{controller}_{prioridadeVerbo}";
    });
});

var app = builder.Build();

// CRIAR ROLES
using (var scope = app.Services.CreateScope())
{

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UsuarioModel>>();
    var usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();

    await RoleSeed.SeedRolesAsync(roleManager);
    await AdminUserSeed.SeedAdminAsync(userManager, usuarioRepository);
    await PermissaoSeed.SeedPermissoesAsync(context);
}

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Permitir requisi��es do Front-end
app.UseCors("FrontEnd");

// Autenticao
app.UseAuthentication();

// Autorizacao
app.UseAuthorization();

// Middleware de excecoes
app.UseMiddleware<ExceptionMiddleware>();

// Controllers
app.MapControllers();

app.Run();