namespace SistemaVentas.DTO
{
    public class DetalleVentaDTO
    {
        public int? IdProducto { get; set; }

        public string? ProductoNombre{ get; set; }

        public int? Cantidad { get; set; }

        public string? Precio { get; set; }

        public string? Total { get; set; }
    }
}
