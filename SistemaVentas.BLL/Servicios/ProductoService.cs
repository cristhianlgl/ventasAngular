using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _repository;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var producto = await _repository.Consultar();
                var lista = producto.Include(x => x.IdCategoriaNavigation);
                return _mapper.Map<List<ProductoDTO>>(lista);
            }
            catch { throw; }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO data)
        {
            try
            {
                var producto = _mapper.Map<Producto>(data);
                var productoCreado = await _repository.Crear(producto)
                                   ?? throw new TaskCanceledException("El producto no existe"); 
                if(productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("El producto no existe");

                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch { throw; }
        }

        public async Task<bool> Editar(ProductoDTO data)
        {
            try
            {
                var producto = _mapper.Map<Producto>(data);
                var productoModelo = await _repository.Obtener(x => x.IdProducto == producto.IdProducto)
                                   ?? throw new TaskCanceledException("El producto no existe"); 
                
                productoModelo.Stock = producto.Stock;
                productoModelo.Precio = producto.Precio;
                productoModelo.EsActivo = producto.EsActivo;
                productoModelo.IdCategoria = producto.IdCategoria;
                productoModelo.Nombre = producto.Nombre;

                return await _repository.Editar(productoModelo)
                           ? true
                           : throw new TaskCanceledException("El Producto no se puedo Editar");   
            }
            catch { throw; }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var producto = await _repository.Obtener(x => x.IdProducto == id)
                             ?? throw new TaskCanceledException("El producto no existe");

                return await _repository.Eliminar(producto)
                       ? true
                       : throw new TaskCanceledException("El Producto no se puedo Eliminar");

            }
            catch { throw; }
        }
    }
}
