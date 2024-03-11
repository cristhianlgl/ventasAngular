using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Servicios
{
    public class DashhBoardService : IDashBoardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public DashhBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        private IQueryable<Venta> RetornarVentas(IQueryable<Venta> queryVenta, int restarCantDias)
        {
            DateTime? ultimaFecha = queryVenta.OrderByDescending(x => x.FechaRegistro)
                .Select(x => x.FechaRegistro).First();
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantDias);
            return queryVenta.Where(x => x.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }

        private async Task<int> TotalVentasUltimaSemana() 
        {
            var queryVenta = await _ventaRepository.Consultar();
            if(queryVenta.Count() == 0)
                return 0;
            var ventas = RetornarVentas(queryVenta, -7);
            return ventas.Count();  
        }

        private async Task<string> TotalIngresosUltimaSemana()
        {
            var queryVenta = await _ventaRepository.Consultar();
            if (queryVenta.Count() == 0)
                return "0";
            var ventas = RetornarVentas(queryVenta, -7);
            var result = ventas.Select(x => x.Total).Sum(x => x.Value);
            return Convert.ToString(result, new CultureInfo("es-CO"));
        }

        private async Task<int> TotalProductos() =>
            (await _productoRepository.Consultar()).Count();
            


        public Task<DashBoardDTO> ResumenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
