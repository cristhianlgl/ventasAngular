using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<VentaDTO> RegistrarAsync(VentaDTO datos);
        Task<List<VentaDTO>> HistorialAsync(string buscadoPor, string numeroVenta, string fechaInicio, string fechaFin);
        Task<List<ReporteDTO>> ReporteAsync(string fechaInicio, string fechaFin);

    }
}
