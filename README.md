# Affecto.Wcf.Monitoring
Generate shallow and/or deep monitoring endpoints dynamically for WCF services. Endpoints are generated for all service contracts hosted by a service host. Shallow endpoint simply returns message "OK". Deep endpoint uses a specific service interface to call a health check method.

NuGet: https://www.nuget.org/packages/Affecto.Wcf.Monitoring

#### Build status

[![Build status](https://ci.appveyor.com/api/projects/status/e2dm0x9gi7aks40s?svg=true)](https://ci.appveyor.com/project/affecto/dotnet-wcf-monitoring)


## Code examples

#### Implementing IHealthCheckService

```csharp
internal class HealthCheckService : IHealthCheckService
{
    private readonly IDataStore store;

    public HealthCheckService(IDataStore store)
    {
        if (store == null)
        {
            throw new ArgumentNullException(nameof(store));
        }
        this.store = store;
    }

    public void CheckHealth()
    {
        store.Ping();
    }
}
```

#### Registering health check service and monitoring endpoints with Autofac

```csharp
ContainerBuilder builder = new ContainerBuilder();

// Register health check service
builder.RegisterType<HealthCheckService>().As<IHealthCheckService>();

// Register other components into container here
// ...

IContainer container = builder.Build();
AutofacHostFactory.Container = container;

AutofacHostFactory.HostConfigurationAction = serviceHost =>
{
    serviceHost.Opening += (sender, args) =>
    {
        // Generate only shallow endpoints:
        serviceHost.GenerateShallowMonitoringEndpoints();

        // Generate only deep endpoints:
        serviceHost.GenerateDeepMonitoringEndpoints(container.Resolve<Func<IHealthCheckService>>());

        // Generate both shallow and deep endpoints:
        serviceHost.GenerateMonitoringEndpoints(container.Resolve<Func<IHealthCheckService>>());
    };
};
```

