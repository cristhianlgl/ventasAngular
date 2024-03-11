using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.BLL.Servicios;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL;
using SistemaVentas.DAL.Repositorio;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.Utility;

namespace SistemaVentas.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration config ) 
        {
            services.AddDbContext<DbventaContext>(op => op.UseSqlServer(config.GetConnectionString("sql")));
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository,VentaRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<ICategoriaService,CategoriaService>();
            services.AddScoped<IDashBoardService,DashBoardService>();
            services.AddScoped<IMenuService,MenuService>();
            services.AddScoped<IProductoService,ProductoService>();
            services.AddScoped<IRolService,RolService>();
            services.AddScoped<IVentaService,VentaService>();
            services.AddScoped<IUsuarioService,UsuarioService>();
        }
    }
}
