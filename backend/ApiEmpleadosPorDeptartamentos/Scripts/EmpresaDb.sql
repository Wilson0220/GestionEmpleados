
create database EmpresaDb
use EmpresaDb

CREATE TABLE EstadoEmpleado (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo int not null,
    Descripcion NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Departamento (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Empleado (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    FechaIngreso DATE NOT NULL DEFAULT GETDATE(),
    Salario DECIMAL(10,2) NOT NULL,
    DepartamentoId INT NOT NULL,
    EstadoEmpleadoId int not null,
    FOREIGN KEY (DepartamentoId) REFERENCES Departamento(Id),
    foreign key (EstadoEmpleadoId) references EstadoEmpleado(Id)
);
GO

insert into EstadoEmpleado (Codigo, Descripcion)
values
(1, 'Activo'),
(2,'Inactivo'),
(3,'Licencia Medica')

INSERT INTO Departamento (Nombre)
VALUES ('Recursos Humanos'),
       ('Finanzas'),
       ('Tecnología'),
       ('Ventas');

INSERT INTO Empleado (Nombre, Apellido, Salario, DepartamentoId, EstadoEmpleadoId)
VALUES 
('Ana', 'Pérez', 45000, 1, 1),
('Luis', 'Martínez', 52000, 2, 1),
('Carla', 'Gómez', 60000, 3, 1),
('Pedro', 'López', 47000, 4, 1),
('Laura', 'Santos', 55000, 3, 1);

select*from dbo.EstadoEmpleado
select*from dbo.Departamento
select*from dbo.Empleado

--create procedure ActulizarSalarioEmpleado
--@Id int,
--@PorcentajeAumento decimal(5,2)
--as
--begin
--set nocount on;

--if not exists(select*from Empleado where Id=@Id)
--	begin
--		print 'Error. No existe un empleado con ese Id.'
--		return
--	end

--update Empleado
--set Salario=(salario + (Salario*@PorcentajeAumento/100.0))
--where Id=@Id

--print 'Salario actualizado exitosamente.'

--end;

--exec ActulizarSalarioEmpleado @Id=1, @PorcentajeAumento=5;
--drop database EmpresaDb