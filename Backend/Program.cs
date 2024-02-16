using Backend.Models;
using Backend.Services.JWT;
using Backend.Services.Tasks;
using Backend.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Registra el DbContext ToDoListContext en el contenedor de servicios, utilizando la cadena de conexión obtenida del archivo de configuración appsettings.json.
builder.Services.AddDbContext<ToDoListContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoListContext"));
});

// Acceder al token JWT desde appsettings.json
// Obtiene el token JWT del archivo de configuración appsettings.json.
string JwtToken = builder.Configuration["JwtSettings:Token"];

// Registra el servicio JWTServices en el contenedor de servicios, con alcance de ámbito.
// El servicio JWTServices se resuelve con el token JWT obtenido anteriormente.
builder.Services.AddScoped<JWTServices>(provider =>
{
    var jwtToken = builder.Configuration["JwtSettings:Token"];
    return new JWTServices(JwtToken);
});

// Authentication JWT
// Configura la autenticación JWT.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
    // Configura la clave de firma utilizando el token JWT obtenido anteriormente.
    var signiKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtToken));
    var signingCredentials = new SigningCredentials(signiKey, SecurityAlgorithms.HmacSha256);
    opt.RequireHttpsMetadata = false;

    // Configura los parámetros de validación del token.
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = signiKey,
    };
});

builder.Services.AddScoped<TasksService>();
builder.Services.AddScoped<AuthService>();
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
