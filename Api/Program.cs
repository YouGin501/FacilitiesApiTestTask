using Api.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// -------------------- Logging --------------------
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.CreateLogger();
builder.Host.UseSerilog();

// -------------------- Repositories & Services --------------------
builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration);

// -------------------- Cors --------------------
builder.Services.AddCors();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	await dbContext.Database.EnsureCreatedAsync();
}

// -------------------- Middleware --------------------
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseCors(options =>
{
	options.AllowAnyHeader();
	options.AllowAnyMethod();
	options.AllowAnyOrigin();
});

app.MapControllers();

app.Run();