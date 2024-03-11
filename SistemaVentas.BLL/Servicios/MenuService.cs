using AutoMapper;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<MenuRol> _menuRolRepository;
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<MenuRol> menuRolRepository, IGenericRepository<Menu> menuRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> ListaAsync(int idUsuario)
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar(x => x.IdUsuario == idUsuario);
                var queryMenu = await _menuRepository.Consultar();
                var queryMenuRol = await _menuRolRepository.Consultar();

                var result = (from u in queryUsuario
                             join mr in queryMenuRol
                             on u.IdRol equals mr.IdRol
                             join m in queryMenu
                             on mr.IdMenu equals m.IdMenu
                             select m).AsQueryable();
                return _mapper.Map<List<MenuDTO>>(result.ToList());

            }
            catch { throw; }
        }
    }
}
