using SistemaVentas.Model;

namespace SistemaVentas.DAL.Repositorio.Contrato
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta model);
    }
}
