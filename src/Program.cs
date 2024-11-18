using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using UserAuthenticationWebApi2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyAllowSpecificOrigins", builder =>
        {
            builder.WithOrigins("https://teenest.netlify.app",
            "http://localhost:5173"
                    )
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });


DotNetEnv.Env.Load();

// Get JWT settings from environment variables
var jwtKey = Environment.GetEnvironmentVariable("Jwt__KEY") ?? throw new InvalidOperationException("JWT Key is missing in environment variables.");
System.Console.WriteLine($"JwtKey {jwtKey}");
var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__ISSUER") ?? throw new InvalidOperationException("JWT Issuer is missing in environment variables.");
var jwtAudience = Environment.GetEnvironmentVariable("Jwt__AUDIENCE") ?? throw new InvalidOperationException("JWT Audience is missing in environment variables.");

var defaultConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? throw new InvalidOperationException("Default Connection is missing in environment variables.");

builder.Services.AddDbContext<AppDBContext>(options =>
  options.UseNpgsql(defaultConnection));


var Configuration = builder.Configuration;


var dataSourceBuilder = new NpgsqlDataSourceBuilder(Configuration.GetConnectionString("Local"));
dataSourceBuilder.MapEnum<PaymentMethod>();
dataSourceBuilder.MapEnum<Material>();
dataSourceBuilder.MapEnum<Status>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserAuthentication, UserAuthenticationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();

var key = Encoding.ASCII.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services Controller Configuration with JsonIgnore
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
        options.SerializerSettings.MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Ignore;
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true });
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
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


var app = builder.Build();

app.MapGet("", (HttpRequest request) =>
{
    return Results.Ok("Welcome to TeeNest Api");

});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    var clientIp = context.Connection.RemoteIpAddress?.ToString();
    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine($"[{DateTime.UtcNow}] [Request] " +
                      $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString} " +
                      $"from {clientIp}");

    await next.Invoke();
    stopwatch.Stop();
    Console.WriteLine($"Time Taken: {stopwatch.ElapsedMilliseconds}");
});


app.UseCors("MyAllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();