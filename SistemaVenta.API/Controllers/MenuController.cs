using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.API.Utilidad;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route(nameof(Lista))]
        public async Task<IActionResult> Lista(int idUsuario)
        {
            var result = new Response<List<MenuDTO>>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.ListaAsync(idUsuario);
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
