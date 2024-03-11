using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using System.Globalization;

namespace SistemaVentas.BLL.Servicios
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<Producto> _productoRepository;

        public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
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

        private async Task<Dictionary<string, int>> VentasUltimaSemana() 
        {
            var query = await _ventaRepository.Consultar();
            if (query.Count() == 0)
                return new Dictionary<string, int>();
            query = RetornarVentas(query, -7);
            return query
                    .GroupBy(x => x.FechaRegistro.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(x => new { fecha = x.Key.ToString("dd/MM/yyyy"), total = x.Count() })
                    .ToDictionary(keySelector: x => x.fecha, elementSelector: x => x.total);
        }

        public async Task<DashBoardDTO> ResumenAsync()
        {
            try
            {
                var listaVentaSemana = new List<VentaSemanaDTO>();
                var dashBoard = new DashBoardDTO();
                var taskIngresos = TotalIngresosUltimaSemana();
                var taskVentas = VentasUltimaSemana();
                var taskProducto = TotalProductos();
                var taskTotalVentas = TotalVentasUltimaSemana();
                dashBoard.TotalIngresos = await taskIngresos;
                dashBoard.TotalVentas = await taskTotalVentas;
                dashBoard.TotalProductos = await taskProducto;
                var ventas = await taskVentas;

                foreach (var item in ventas)
                {
                    listaVentaSemana.Add(new VentaSemanaDTO
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                dashBoard.VentasUltimaSemana = listaVentaSemana;
                return dashBoard;
            }
            catch { throw; }
        }
    }
}
