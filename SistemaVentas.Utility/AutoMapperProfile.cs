using AutoMapper;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using System.Globalization;

namespace SistemaVentas.Utility
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            var cultureInfo = new CultureInfo("en-CO");

            CreateMap<Rol,RolDTO>().ReverseMap();
            CreateMap<Menu, MenuDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(o => o.IdRolNavigation.Nombre))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => Convert.ToInt32(o.EsActivo)));
            CreateMap<Usuario, SesionDTO>()
                .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(o => o.IdRolNavigation.Nombre));
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.IdRolNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => Convert.ToBoolean(o.EsActivo)));
            
            CreateMap<Producto, ProductoDTO>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(o => o.IdCategoriaNavigation.Nombre))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => Convert.ToInt32(o.EsActivo)))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToString(o.Precio, cultureInfo)));
            CreateMap<ProductoDTO, Producto>()
                .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => Convert.ToBoolean(o.EsActivo)))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, cultureInfo)));

            CreateMap<Venta, VentaDTO>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToString(o.Total, cultureInfo)))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(o => o.FechaRegistro.Value.ToString("dd/MM/yyyy")));
            CreateMap<VentaDTO, Venta>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToDecimal(o.Total, cultureInfo)));

            CreateMap<DetalleVenta, DetalleVentaDTO>()
               .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(o => o.IdProductoNavigation.Nombre))
               .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToString(o.Precio, cultureInfo)))
               .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToString(o.Total, cultureInfo)));
            CreateMap<DetalleVentaDTO, DetalleVenta>()
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToDecimal(o.Total, cultureInfo)))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, cultureInfo)));

            CreateMap<DetalleVenta, ReporteDTO>()
               .ForMember(dest => dest.FechaRegistro,
                          opt => opt.MapFrom(o => o.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")))
               .ForMember(dest => dest.NumeroDocumento, 
                          opt => opt.MapFrom(o => o.IdVentaNavigation.NumeroDocumento))
               .ForMember(dest => dest.TipoPago, 
                          opt => opt.MapFrom(o => o.IdVentaNavigation.TipoPago))
               .ForMember(dest => dest.TotalVenta,
                          opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation.Total, cultureInfo)))
               .ForMember(dest => dest.Producto,
                          opt => opt.MapFrom(o => o.IdProductoNavigation.Nombre))
               .ForMember(dest => dest.Precio,
                          opt => opt.MapFrom(o => Convert.ToString(o.Precio, cultureInfo)))
               .ForMember(dest => dest.Total,
                          opt => opt.MapFrom(o => Convert.ToString(o.Total, cultureInfo)));

        }
    }
}
