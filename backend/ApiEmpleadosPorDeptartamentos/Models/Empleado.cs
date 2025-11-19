namespace ApiEmpleadosPorDeptartamentos.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public decimal Salario { get; set; }

        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }

        public int EstadoEmpleadoId { get; set; }
        public EstadoEmpleado EstadoEmpleado { get; set; }
    }
}
