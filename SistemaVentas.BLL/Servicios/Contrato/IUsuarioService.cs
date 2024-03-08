using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<SesionDTO> ValidarCredenciales(string correo, string clave);
        Task<UsuarioDTO> Crear( UsuarioDTO data);
        Task<bool> Editar(UsuarioDTO data);
        Task<bool> Eliminar(int id);
    }
}
