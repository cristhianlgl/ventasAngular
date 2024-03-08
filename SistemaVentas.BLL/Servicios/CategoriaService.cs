using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    public class CategoriaService:ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _repository;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> ListaAsync()
        {
            try
            {
                var listaCategoria = await _repository.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategoria);
            }
            catch
            {
                throw;
            }
        }
    }
}
