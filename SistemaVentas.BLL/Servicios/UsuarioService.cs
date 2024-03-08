using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorio.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _repository.Consultar();
                var listaUsuarioRol = queryUsuario.Include(rol => rol.IdRolNavigation);
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarioRol);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO data)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(data);
                var usuarioCreado = await _repository.Crear(usuarioModelo);
                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("El Usuario no se ha creado");

                var query = await _repository.Consultar(x => x.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.Include(rol => rol.IdUsuario).First();
                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO data)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(data);
                var usuarioEncontrado = await _repository.Obtener(x => x.IdUsuario == usuarioModelo.IdUsuario);
                if(usuarioEncontrado == null)
                    throw new TaskCanceledException("El Usuario no existe");

                usuarioEncontrado.Correo =  usuarioModelo.Correo;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo?.EsActivo;
                usuarioEncontrado.NombreCompleto = usuarioModelo?.NombreCompleto;
                usuarioEncontrado.IdRol = usuarioModelo?.IdRol;

                bool resultado = await _repository.Editar(usuarioEncontrado);
                if (!resultado)
                    throw new TaskCanceledException("Usuario no se puedo Actualizar");

                return resultado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var usuarioEncontrado = await _repository.Obtener(x => x.IdUsuario == id) 
                                      ?? throw new TaskCanceledException("Usuario no Encontrado");

                return await _repository.Editar(usuarioEncontrado) 
                       ? true                     
                       : throw new TaskCanceledException("Usuario no se puedo Eliminar");
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _repository.Consultar(x => x.Correo == correo && x.Clave == clave);

                if (queryUsuario.FirstOrDefault() == null)
                    throw new TaskCanceledException("Usuario no Exite");

                var usuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(usuario);
            }
            catch
            {
                throw;
            }
        }
    }
}
