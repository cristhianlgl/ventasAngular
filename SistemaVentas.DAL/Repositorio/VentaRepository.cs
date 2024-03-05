using Microsoft.EntityFrameworkCore;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public async Task<Venta> Registrar(Venta model)
        {
            using (var transacion = _dbContext.Database.BeginTransaction()) 
            {
                try
                {
                    foreach (var item in model.DetalleVenta)
                    {
                        var itemEncontrado = _dbContext.Productos.FirstOrDefault(x => x.IdProducto == item.IdProducto);
                        if (itemEncontrado is null)
                            continue;

                        itemEncontrado.Stock -= item.Cantidad;
                        _dbContext.Productos.Update(itemEncontrado);
                    }
                    await _dbContext.SaveChangesAsync();

                    var consecutivo = _dbContext.NumeroDocumentos.First();
                    consecutivo.UltimoNumero++;
                    consecutivo.FechaRegistro = DateTime.Now;
                    _dbContext.NumeroDocumentos.Update(consecutivo);
                    await _dbContext.SaveChangesAsync();

                    int cantDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", cantDigitos));
                    string numeroVenta = ceros + consecutivo.UltimoNumero;
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantDigitos);

                    model.NumeroDocumento = numeroVenta;

                    await _dbContext.Venta.AddAsync(model);
                    await _dbContext.SaveChangesAsync();

                    await transacion.CommitAsync();
                    return model;
                }
                catch 
                {
                    transacion.Rollback();
                    throw;
                }
            }
        }
    }
}
