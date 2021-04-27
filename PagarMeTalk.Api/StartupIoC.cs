using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PagarMeTalk.Api.AutoMapper;
using PagarMeTalk.Api.Repositories;
using PagarMeTalk.Api.Services;
using PagarMeTalk.Api.Shared;

namespace PagarMeTalk.Api
{
    public class StartupIoC
    {
        public static void Register(IServiceCollection services)
        {
            //Repositories
            services.AddSingleton<IOrderRepository, OrderRepository>();

            //Services
            services.AddScoped<IOrderService, OrderService>();

            //Mappers
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderProfile>();
            });

            var builder = services.BuildServiceProvider();
            GlobalMapper.Mapper = builder.GetService<IMapper>();
        }
    }
}
