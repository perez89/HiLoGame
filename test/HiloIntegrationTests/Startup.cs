//namespace HiloIntegrationTests.Setup;

//public class Startup
//{
//    private readonly IHostBuilder _defaultBuilder;
//    private IServiceProvider _services;
//    private bool _built = false;

//    public Startup()
//    {
//        _defaultBuilder = Host.CreateDefaultBuilder();
//    }

//    public IServiceProvider Services => _services ?? Build();

//    private IServiceProvider Build()
//    {
//        Console.WriteLine("setup build");
//        if (_built)
//            throw new InvalidOperationException("Build can only be called once.");
//        _built = true;

//        _defaultBuilder.ConfigureServices((context, services) =>
//        {
//            services.AddHttpClient();
//            //services.AddSingleton<IService, ServiceImpl>();
//            // where ServiceImpl implements IService
//            // ... add other services when needed
//        });

//        _services = _defaultBuilder.Build().Services;
//        return _services;
//    }
//}
