using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using System.Globalization;

namespace SistemaVentas.BLL.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleRepository;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleRepository = detalleRepository;
            _mapper = mapper;
        }
        public async Task<List<VentaDTO>> HistorialAsync(string buscadoPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            try
            {
                var query = await _ventaRepository.Consultar();
                if (buscadoPor == "fecha")
                {
                    DateTime fechaIniDate = ConvertToDate(fechaInicio);
                    DateTime fechaFinDate = ConvertToDate(fechaFin);
                    query = query.Where(x => x.FechaRegistro.Value.Date >= fechaIniDate.Date &&
                                             x.FechaRegistro.Value.Date <= fechaFinDate.Date);
                }
                else
                {
                    query = query.Where(x => x.NumeroDocumento == numeroVenta);
                }
                var lista = await query.Include(x => x.DetalleVenta)
                            .ThenInclude(x => x.IdProductoNavigation)
                            .ToListAsync();
                return _mapper.Map<List<VentaDTO>>(lista);
            }
            catch { throw; }
        }

        public async Task<VentaDTO> RegistrarAsync(VentaDTO datos)
        {
            try
            {
                var ventaModelo = _mapper.Map<Venta>(datos);
                var ventaResult = await _ventaRepository.Registrar(ventaModelo);
                if (ventaResult.IdVenta == 0)
                    throw new TaskCanceledException("No se puedo registrar la venta");
                return _mapper.Map<VentaDTO>(ventaResult);
            }
            catch { throw; }
        }

        public async Task<List<ReporteDTO>> ReporteAsync(string fechaInicio, string fechaFin)
        {
            try
            {
                var query =  await _detalleRepository.Consultar();
                DateTime fechaIniDate = ConvertToDate(fechaInicio);
                DateTime fechaFinDate = ConvertToDate(fechaFin);

                var result = await query
                        .Include(x => x.IdProductoNavigation)
                        .Include(x => x.IdVentaNavigation)
                        .Where(x => x.IdVentaNavigation.FechaRegistro.Value.Date >= fechaIniDate.Date &&
                                x.IdVentaNavigation.FechaRegistro.Value.Date <= fechaFinDate.Date).ToListAsync();

                return _mapper.Map<List<ReporteDTO>>(result);   
            }
            catch { throw; }
        }

        private DateTime ConvertToDate(string fecha) => 
            DateTime.ParseExact(fecha, "dd/MM/yyyy", new CultureInfo("es-CO"));

    }
}
