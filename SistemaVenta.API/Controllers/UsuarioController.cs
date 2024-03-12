using Microsoft.AspNetCore.Mvc;
using SistemaVentas.API.Utilidad;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route(nameof(Lista))]
        public async Task<IActionResult> Lista()
        {
            var result = new Response<List<UsuarioDTO>>();
            try
            {
                result.Estatus = true;
                result.Valor = await _usuarioService.Lista();
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Mensaje = ex.Message;
            }
            return Ok(result);
        }

        [HttpPost]
        [Route(nameof(IniciarSesion))]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var result = new Response<SesionDTO>();
            try
            {
                result.Estatus = true;
                result.Valor = await _usuarioService.ValidarCredenciales(login.Correo,login.Clave);
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
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {
            var result = new Response<UsuarioDTO>();
            try
            {
                result.Estatus = true;
                result.Valor = await _usuarioService.Crear(usuario);
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
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
        {
            var result = new Response<bool>();
            try
            {
                result.Estatus = true;
                result.Valor = await _usuarioService.Editar(usuario);
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
                result.Valor = await _usuarioService.Eliminar(id);
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
