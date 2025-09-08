using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

// Secrets Fly.io
var mysqlConnection = builder.Configuration["MYSQL_CONNECTION"];
var mongoConnection = builder.Configuration["MONGODB_CONNECTION"];
var mailjetApiKey = builder.Configuration["MAILJET_APIKEY"];
var mailjetApiSecret = builder.Configuration["MAILJET_APISECRET"];

if (string.IsNullOrEmpty(mysqlConnection))
    throw new Exception("MYSQL_CONNECTION non défini.");

if (string.IsNullOrEmpty(mongoConnection))
    throw new Exception("MONGODB_CONNECTION non défini.");

// Convertir URI Railway en connection string MySQL classique
var uri = new Uri(mysqlConnection);
var userInfo = uri.UserInfo.Split(':');
var mysqlConnectionString = $"Server={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Uid={userInfo[0]};Pwd={userInfo[1]};SslMode=Preferred;";

// Import SQL uniquement si base vide
try
{
    var seeder = new DatabaseSeeder(mysqlConnectionString);
    var dataPath = Path.Combine(AppContext.BaseDirectory, "Data"); // /app/Data dans le conteneur
    seeder.ImportSqlDumpIfEmpty(dataPath);    
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur import dump: {ex.Message}");
}

// DbContext
builder.Services.AddDbContext<CinephoriaContext>(options =>
    options.UseMySql(mysqlConnectionString, new MySqlServerVersion(new Version(8, 0, 32))));

// MongoDB
builder.Services.Configure<MongoDbSettings>(config =>
{
    config.ConnectionString = mongoConnection;
    config.DatabaseName = builder.Configuration["MongoDbSettings:DatabaseName"] ?? "reservation";
});
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddScoped<IMongoDatabase>(s =>
{
    var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = s.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Mailjet
builder.Services.Configure<MailjetSettings>(config =>
{
    config.ApiKey = mailjetApiKey;
    config.ApiSecret = mailjetApiSecret;
    config.SenderEmail = "jl.quazuguel@gmail.com";
    config.SenderName = "Cinephoria";
});

// Services métier
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ReviewsService>();
builder.Services.AddScoped<MailService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", corsBuilder =>
        corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Controllers / Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");
app.UseAuthorization();
app.MapControllers();

app.Run();
