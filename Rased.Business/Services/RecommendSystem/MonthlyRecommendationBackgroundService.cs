using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.RecommendSystem
{
    public class MonthlyRecommendationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MonthlyRecommendationBackgroundService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromDays(1); //  يومي

        public MonthlyRecommendationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<MonthlyRecommendationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;

                    // تشغيل في أول يوم من كل شهر الساعة 2 صباحاً
                    if (now.Day == 1 && now.Hour == 2 && now.Minute < 5)
                    {
                        _logger.LogInformation("Starting monthly recommendations generation");

                        using var scope = _serviceProvider.CreateScope();
                        var monthlyService = scope.ServiceProvider.GetRequiredService<IRecommendationService>();

                        await monthlyService.GenerateMonthlyRecommendationsAsync();
                    }

                    await Task.Delay(_period, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in monthly recommendation background service");
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // إعادة المحاولة بعد ساعة
                }
            }
        }
    }
}