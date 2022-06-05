using System.ComponentModel.DataAnnotations;
namespace DSW1_CL2_VillavicencioCardenasJoseAntonio.Models
{
    public class Horario
    {
        [Display(Name = "CodHorario")] public int codhorario { get; set; }
        [Display(Name = "Nombre Curso")] public string nomcurso { get; set; }
        [Display(Name = "Fecha de Inicio")] public DateTime fecinicio { get; set; }
        [Display(Name = "Fecha de Final")] public DateTime fectermino { get; set; }
        [Display(Name = "Vacantes Disponibles")] public int vacantes { get; set; }
    }
}
