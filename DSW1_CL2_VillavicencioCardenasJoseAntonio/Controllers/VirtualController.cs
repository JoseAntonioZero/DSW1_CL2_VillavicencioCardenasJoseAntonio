using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json; //serializa/deserializa como cadena json
using DSW1_CL2_VillavicencioCardenasJoseAntonio.Models;

namespace DSW1_CL2_VillavicencioCardenasJoseAntonio.Controllers
{
    public class VirtualController : Controller
    {
        string cadena = @"server = (local);database = Virtuales2022;" +
                    "Trusted_Connection = True;" + "MultipleActiveResultSets = True;" +
                    "TrustServerCertificate = False;" + "Encrypt = False";
        IEnumerable<Horario> listadoFecha(DateTime f1)
        {
            List<Horario> temporal = new List<Horario>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec usp_horariosFecha @fecinicio", cn);
                cmd.Parameters.Add("@fecinicio", SqlDbType.Date).Value = f1;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Horario()
                    {
                        codhorario = dr.GetInt32(0),
                        nomcurso = dr.GetString(1),
                        fecinicio = dr.GetDateTime(2),
                        fectermino = dr.GetDateTime(3),
                        vacantes = dr.GetInt32(4),
                    });
                }
            }

            return temporal;
        }
        public IActionResult Consulta(DateTime? fecha1 = null)
        {
            DateTime f1 = (fecha1 == null ? DateTime.Today.AddDays(1) : (DateTime)fecha1);
            ViewBag.fecha1 = f1;
            return View(listadoFecha(f1));
        }

        IEnumerable<Horario> listado()
        {
            List<Horario> temporal = new List<Horario>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec usp_horarios", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Horario()
                    {
                        codhorario = dr.GetInt32(0),
                        nomcurso = dr.GetString(1),
                        fecinicio = dr.GetDateTime(2),
                        fectermino = dr.GetDateTime(3),
                        vacantes = dr.GetInt32(4),
                    });
                }
            }

            return temporal;
        }

        Horario Buscar(int codigo = 0)
        {
            return listado().FirstOrDefault(c => c.codhorario == codigo);
        }

        Horario BuscarA(int codigo = 0)
        {
            return listado().FirstOrDefault(c => c.codhorario == codigo);
        }

        public IActionResult Registrar(int id = 0)
        {
            //buscar el producto por idproducto
            Horario reg = Buscar(id);
            if (reg == null)
            {
                return RedirectToAction("Consulta");
            }
            else
            {
                return View(reg);
            }
        }
        
        [HttpPost]
        public IActionResult Registrar(int codigo,string alumno ,string email)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_registra_registro", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codhorario", codigo);
                    cmd.Parameters.AddWithValue("@alumno", alumno);
                    cmd.Parameters.AddWithValue("@email", email);

                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Registro Exitoso";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }

            ViewBag.mensaje = mensaje;
            return View(Buscar(codigo));
        }

    }
}
