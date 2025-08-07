using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
	public class SomeBackgroundService : BackgroundService
	{
		private readonly ILogger<SomeBackgroundService> _logger;

		public SomeBackgroundService(ILogger<SomeBackgroundService> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await DoSomethingInBackgroundAsync(cancellationToken);
			}
		}

		public async Task DoSomethingInBackgroundAsync(CancellationToken cancellationToken)
		{
			try
			{
				await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
				_logger.LogInformation("Background service did something...");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while processing background task");
			}
		}
	}
}
