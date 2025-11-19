namespace ApiEmpleadosPorDeptartamentos.Models.DTOs
{
    public class ActualizarEmpleadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public decimal Salario { get; set; }
        public int DepartamentoId { get; set; }
        public int EstadoEmpleadoId { get; set; }
    }
}
