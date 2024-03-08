using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> ListaAsync();
    }
}
