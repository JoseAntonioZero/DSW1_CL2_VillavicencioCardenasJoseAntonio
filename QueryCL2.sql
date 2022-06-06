Create database Virtuales2022
go
Use Virtuales2022
go
Set dateFormat dmy
go
create table tb_curso(
	codcurso int primary key,
	nomcurso varchar(255) not null
)
go
insert tb_curso 
Values(1,'java'),(2,'C#'),(3,'SQL'),(4,'Phyton'),(5,'Angular')
go
create table tb_horario(
	codhorario int primary key,
	codcurso int references tb_curso,
	fecinicio datetime,
	fecterminio datetime null,
	vacantes int
)
go
create table tb_registro(
	idvacante int identity(1,1) primary key,
	codhorario int references tb_horario,
	fregistro datetime,
	alumno varchar(255),
	email varchar(255)
)
go

create or alter proc usp_cursos
as
select * from tb_curso
go

exec usp_cursos
go

create or alter proc usp_horariosFecha
@fecinicio datetime
as
select h.codhorario [Codigo de horario], c.nomcurso [Nombre de Curso], 
	h.fecinicio [Fecha de inicio], h.fecterminio [Fecha de fin], h.vacantes [Vacantes disponibles]
from tb_horario h join tb_curso c on h.codcurso = c.codcurso where h.fecinicio = @fecinicio
go

create or alter proc usp_horarios
as
select h.codhorario [Codigo de horario], c.nomcurso [Nombre de Curso], 
	h.fecinicio [Fecha de inicio], h.fecterminio [Fecha de fin], h.vacantes [Vacantes disponibles]
from tb_horario h join tb_curso c on h.codcurso = c.codcurso
go

exec usp_horariosFecha '01-06-2022'
go

create or alter proc usp_inserta_curso
@codhorario int,
@codcurso int,
@fecinicio datetime
as
insert tb_horario values(@codhorario, @codcurso, @fecinicio, DATEADD(DAY,30,@fecinicio),15)
go

exec usp_inserta_curso 100,1,'1-6-2022'
exec usp_inserta_curso 101,2,'2-6-2022'
exec usp_inserta_curso 104,3,'10-06-2022'
exec usp_inserta_curso 105,2,'15-06-2022'
exec usp_inserta_curso 106,1,'18-06-2022'
exec usp_inserta_curso 107,5,'21-06-2022'
go

--Procedure para Registrar y disminuir el numero de vacantes

create or alter proc usp_registra_registro
@codhorario int,
@alumno varchar(255),
@email varchar(255)
as
begin
	insert into tb_registro (codhorario, fregistro, alumno, email) 
	values(@codhorario, GETDATE() , @alumno, @email)
	update tb_horario
	set vacantes = vacantes -1 where codhorario = @codhorario
end
go

select * from tb_registro
go

select * from tb_horario
go
