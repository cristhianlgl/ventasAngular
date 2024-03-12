using Microsoft.AspNetCore.Mvc;
using SistemaVentas.API.Utilidad;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route(nameof(Lista))]
        public async Task<IActionResult> Lista()
        {
            var result = new Response<List<CategoriaDTO>>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.ListaAsync();
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
