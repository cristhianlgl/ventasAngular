using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Servicios.Contrato
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> Lista();
        Task<ProductoDTO> Crear(ProductoDTO data);
        Task<bool> Editar(ProductoDTO data);
        Task<bool> Eliminar(int id);
    }
}
