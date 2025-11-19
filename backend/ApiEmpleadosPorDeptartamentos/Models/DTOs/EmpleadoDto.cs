namespace ApiEmpleadosPorDeptartamentos.Models.DTOs
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal Salario { get; set; }
        public int DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int EstadoEmpleadoId { get; set; }
        public string EstadoEmpleado { get; set; }
    }
}
