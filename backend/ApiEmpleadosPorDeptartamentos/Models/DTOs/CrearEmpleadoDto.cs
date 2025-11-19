namespace ApiEmpleadosPorDeptartamentos.Models.DTOs
{
    public class CrearEmpleadoDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public decimal Salario { get; set; }
        public int DepartamentoId { get; set; }
        public int EstadoEmpleadoId { get; set; }
    }
}
