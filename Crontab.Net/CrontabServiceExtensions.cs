using Crontab.Net.Cron;
using Microsoft.Extensions.DependencyInjection;

namespace Crontab.Net;

public static class CrontabServiceExtensions
{
    public static IServiceCollection AddCrontabNet(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CrontabList>());
        serviceCollection.AddSingleton<ICrontabWriter, CrontabWriter>();
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddCrontabNetFake(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CrontabList>());
        serviceCollection.AddSingleton<ICrontabWriter, FakeCrontabWriter>();
        
        return serviceCollection;
    }
}