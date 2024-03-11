using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface IMenuService
    {
        Task<List<MenuDTO>> ListaAsync(int idUsuario);
    }
}
