using Microsoft.AspNetCore.Mvc;
using SistemaVentas.API.Utilidad;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _service;

        public VentaController(IVentaService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route(nameof(Registrar))]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var result = new Response<VentaDTO>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.RegistrarAsync(venta);
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(Historial))]
        public async Task<IActionResult> Historial(string buscadoPor, string? nombre, string? fechaIni, string? fechaFin)
        {
            var result = new Response<List<VentaDTO>>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.HistorialAsync(buscadoPor, nombre, fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(Reporte))]
        public async Task<IActionResult> Reporte(string? fechaIni, string? fechaFin)
        {
            var result = new Response<List<ReporteDTO>>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.ReporteAsync(fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }
    }


}
