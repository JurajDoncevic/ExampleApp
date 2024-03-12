using System.Text.Json.Serialization;
using ExampleApp.DataAccess.Sqlite.Data;
using ExampleApp.Repositories;
using ExampleApp.Repositories.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// create a configuration (app settings) from the appsettings file, depending on the ENV
IConfiguration configuration = builder.Environment.IsDevelopment()
    ? builder.Configuration.AddJsonFile("appsettings.Development.json").Build()
    : builder.Configuration.AddJsonFile("appsettings.json").Build();

// add controllers and configure JSON serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // ignore cycles in JSON
    });

// add the DbContext to the DI container
builder.Services.AddDbContext<ExampleDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteExampleDb"));
           //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // enable this to disable change tracking
});

// add the repositories to the DI container - no interface???
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IPersonRepository, PersonRepository>();



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

app.UseAuthorization();

app.MapControllers();

app.Run();
