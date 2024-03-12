using Microsoft.AspNetCore.Mvc;
using SistemaVentas.API.Utilidad;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route(nameof(Lista))]
        public async Task<IActionResult> Lista()
        {
            var result = new Response<List<ProductoDTO>>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.Lista();
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route(nameof(Guardar))]
        public async Task<IActionResult> Guardar([FromBody] ProductoDTO producto)
        {
            var result = new Response<ProductoDTO>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.Crear(producto);
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }

        [HttpPut]
        [Route(nameof(Editar))]
        public async Task<IActionResult> Editar([FromBody] ProductoDTO producto)
        {
            var result = new Response<bool>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.Editar(producto);
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route($"{nameof(Eliminar)}/{{id:int}}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = new Response<bool>();
            try
            {
                result.Estatus = true;
                result.Valor = await _service.Eliminar(id);
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
