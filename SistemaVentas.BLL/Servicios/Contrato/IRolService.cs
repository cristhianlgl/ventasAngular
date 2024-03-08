using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface IRolService
    {
        Task<List<RolDTO>> Lista();
    }
}
