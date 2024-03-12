using Microsoft.AspNetCore.Mvc;
using SistemaVentas.API.Utilidad;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _service;

        public DashBoardController(IDashBoardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route(nameof(Resumen))]
        public async Task<IActionResult> Resumen()
        {
            var result = new Response<DashBoardDTO>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.ResumenAsync();
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
