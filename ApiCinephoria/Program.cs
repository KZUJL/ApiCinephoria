using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ApiCinephoria.Data;
using ApiCinephoria.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Chargement config
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddUserSecrets<Program>(optional: true)
            .AddEnvironmentVariables();

        // Récupération MySQL
        var mysqlConnection = builder.Configuration["MYSQL_CONNECTION"];
        if (string.IsNullOrEmpty(mysqlConnection))
        {
            throw new Exception("La chaîne de connexion MySQL n'est pas définie.");
        }
        // Liste des dumps à importer (dans Data/)
        var dumpFiles = new[]
        {
            "Data/cinephoria_users.sql",
            "Data/cinephoria_roles.sql",
            "Data/cinephoria_locations.sql",
            "Data/cinephoria_cinemas.sql",
            "Data/cinephoria_rooms.sql",
            "Data/cinephoria_movies.sql",
            "Data/cinephoria_movietimes.sql",
            "Data/cinephoria_cinema_schedule.sql",
            "Data/cinephoria_incident.sql"
        };

        try
        {
            var seeder = new DatabaseSeeder(mysqlConnection);

            foreach (var dumpFile in dumpFiles)
            {
                Console.WriteLine($" Import de {dumpFile} ...");
                seeder.ImportSqlDump(dumpFile);
            }

            Console.WriteLine(" Import terminé avec succès !");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Erreur import dump: {ex.Message}");
        }
        builder.Services.AddDbContext<CinephoriaContext>(options =>
            options.UseMySql(mysqlConnection, new MySqlServerVersion(new Version(8, 0, 32))));

        // Récupération MongoDB connection string
        var mongoConnection = builder.Configuration["MONGODB_CONNECTION"]
                              ?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION");

        if (string.IsNullOrEmpty(mongoConnection))
        {
            throw new Exception("La chaîne de connexion MongoDB n'est pas définie.");
        }

        // Injection en mémoire dans config
        var inMemorySettings = new Dictionary<string, string>
        {
            {"MongoDbSettings:ConnectionString", mongoConnection},
            {"MongoDbSettings:DatabaseName", builder.Configuration["MongoDbSettings:DatabaseName"] ?? "reservation"}
        };
        builder.Configuration.AddInMemoryCollection(inMemorySettings);

        // Binding section MongoDbSettings
        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

        // Injection MongoClient
        builder.Services.AddSingleton<IMongoClient>(s =>
        {
            var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        // Injection MongoDatabase
        builder.Services.AddScoped<IMongoDatabase>(s =>
        {
            var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = s.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });



        var mailjetApiKey = builder.Configuration["MAILJET_APIKEY"] ?? Environment.GetEnvironmentVariable("MAILJET_APIKEY");
        var mailjetApiSecret = builder.Configuration["MAILJET_APISECRET"] ?? Environment.GetEnvironmentVariable("MAILJET_APISECRET");

        var mailjetSettingsDict = new Dictionary<string, string>
{
            {"Mailjet:ApiKey", mailjetApiKey ?? string.Empty},
            {"Mailjet:ApiSecret", mailjetApiSecret ?? string.Empty},
            {"Mailjet:SenderEmail", "jl.quazuguel@gmail.com"},  
            {"Mailjet:SenderName", "Cinephoria"}
        };
        // Injecte dans la config
        builder.Configuration.AddInMemoryCollection(mailjetSettingsDict);

        // Mailjet Settings
        builder.Services.Configure<MailjetSettings>(builder.Configuration.GetSection("Mailjet"));

        // Services métier
        builder.Services.AddScoped<IReservationService, ReservationService>();
        builder.Services.AddScoped<ReviewsService>();
        builder.Services.AddScoped<MailService>();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost",
                corsBuilder => corsBuilder.AllowAnyOrigin()
                                         .AllowAnyMethod()
                                         .AllowAnyHeader());
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.Urls.Add("http://0.0.0.0:80"); //fly.io

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
    }
}
