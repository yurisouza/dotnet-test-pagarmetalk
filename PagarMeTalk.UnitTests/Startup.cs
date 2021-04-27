using Microsoft.Extensions.DependencyInjection;
using PagarMeTalk.Api;

namespace PagarMeTalk.UnitTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            StartupIoC.Register(services);
        }
    }
}
