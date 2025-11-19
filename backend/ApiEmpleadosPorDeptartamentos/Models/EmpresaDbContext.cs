using ApiEmpleadosPorDeptartamentos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpleadosPorDeptartamentos.Models
{
    public class EmpresaDbContext : DbContext, IEmpresaDbContext
    {
        public EmpresaDbContext(DbContextOptions<EmpresaDbContext> options)
            : base(options) { }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<EstadoEmpleado> EstadosEmpleado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔧 Forzar el nombre de las tablas según tu base de datos real
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Departamento>().ToTable("Departamento");
            modelBuilder.Entity<EstadoEmpleado>().ToTable("EstadoEmpleado");

            // 🔢 Formato del campo decimal
            modelBuilder.Entity<Empleado>()
                .Property(e => e.Salario)
                .HasColumnType("decimal(10,2)");
        }
    }
}
