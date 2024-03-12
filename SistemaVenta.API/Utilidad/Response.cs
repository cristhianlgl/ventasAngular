namespace SistemaVentas.API.Utilidad
{
    public class Response<T>
    {
        public bool Estatus { get; set; }
        public T Valor { get; set; }
        public string Mensaje { get; set; }
    }
}
