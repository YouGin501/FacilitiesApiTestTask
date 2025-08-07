namespace Api.Middleware;

public class ApiKeyMiddleware
{
	private const string HeaderName = "X-API-KEY";
	private readonly RequestDelegate _next;
	private readonly ILogger<ApiKeyMiddleware> _logger;
	private readonly string _validApiKey;

	public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IConfiguration config)
	{
		_next = next;
		_logger = logger;
		_validApiKey = config.GetValue<string>("ApiKey") ?? throw new InvalidOperationException("API key not configured.");
	}

	public async Task Invoke(HttpContext context)
	{
		if (!context.Request.Headers.TryGetValue(HeaderName, out var extractedApiKey))
		{
			_logger.LogWarning("Missing API key");
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			await context.Response.WriteAsJsonAsync(new { errors = new[] { "API key is missing" } });
			return;
		}

		if (!_validApiKey.Equals(extractedApiKey))
		{
			_logger.LogWarning("Invalid API key: {Key}", extractedApiKey);
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			await context.Response.WriteAsJsonAsync(new { errors = new[] { "API key is invalid" } });
			return;
		}

		await _next(context);
	}
}