using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DAL.Repositorio
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbContext;

        public VentaRepository(DbventaContext context) : base(context)
        {
            _dbContext = context;
        }
        public Task<Venta> Registrar(Venta model)
        {

        }
    }
}
