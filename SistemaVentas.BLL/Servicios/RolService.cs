using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _repository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try
            {
               var listaRol = await _repository.Consultar();
               return _mapper.Map<List<RolDTO>>(listaRol);
            }
            catch 
            {
                throw;
            }
        }
    }
}
