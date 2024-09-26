using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace SwSoap
{
    /// <summary>
    /// Descripción breve de WsSoap
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WsSoap : System.Web.Services.WebService
    {
        SqlConnection con = new SqlConnection(@"Data Source = GRACE\SQLEXPRESS; Initial Catalog = SwSoap; Integrated Security = True; Encrypt=False; ");
        public AuthHeader Authentication;

        #region artista,album,genero

        [WebMethod]
        public DataSet ws_listar_artistas()
        {
            return ExecuteQuery("SELECT * FROM artista");
        }

        [WebMethod]
        public DataSet ws_listar_generos()
        {
            return ExecuteQuery("SELECT * FROM genero");
        }

        [WebMethod]
        public DataSet ws_listar_albumes()
        {
            string query = "SELECT album.IdAlbum, album.TituloAlbum, album.IdArtista, album.IdGenero, artista.NombreArtista, genero.NombreGenero " +
                           "FROM album " +
                           "INNER JOIN artista ON album.IdArtista = artista.IdArtista " +
                           "INNER JOIN genero ON album.IdGenero = genero.IdGenero";
            return ExecuteQuery(query);
        }

        [WebMethod]
        public string ws_insertar_artista(string nombreArtista)
        {
            string query = "INSERT INTO artista (NombreArtista) VALUES (@NombreArtista)";
            SqlParameter[] parameters = {
                new SqlParameter("@NombreArtista", nombreArtista)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        public string ws_insertar_genero(string nombreGenero)
        {
            string query = "INSERT INTO genero (NombreGenero) VALUES (@NombreGenero)";
            SqlParameter[] parameters = {
                new SqlParameter("@NombreGenero", nombreGenero)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        public string ws_insertar_album(string tituloAlbum, int idArtista, int idGenero)
        {
            string query = "INSERT INTO album (TituloAlbum, IdArtista, IdGenero) VALUES (@TituloAlbum, @IdArtista, @IdGenero)";
            SqlParameter[] parameters = {
                new SqlParameter("@TituloAlbum", tituloAlbum),
                new SqlParameter("@IdArtista", idArtista),
                new SqlParameter("@IdGenero", idGenero)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        public string ws_editar_artista(int idArtista, string nombreArtista)
        {
            string query = "UPDATE artista SET NombreArtista = @NombreArtista WHERE IdArtista = @IdArtista";
            SqlParameter[] parameters = {
                new SqlParameter("@NombreArtista", nombreArtista),
                new SqlParameter("@IdArtista", idArtista)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        public string ws_editar_genero(int idGenero, string nombreGenero)
        {
            string query = "UPDATE genero SET NombreGenero = @NombreGenero WHERE IdGenero = @IdGenero";
            SqlParameter[] parameters = {
                new SqlParameter("@NombreGenero", nombreGenero),
                new SqlParameter("@IdGenero", idGenero)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        public string ws_editar_album(int idAlbum, string tituloAlbum, int idArtista, int idGenero)
        {
            string query = "UPDATE album SET TituloAlbum = @TituloAlbum, IdArtista = @IdArtista, IdGenero = @IdGenero WHERE IdAlbum = @IdAlbum";
            SqlParameter[] parameters = {
                new SqlParameter("@TituloAlbum", tituloAlbum),
                new SqlParameter("@IdArtista", idArtista),
                new SqlParameter("@IdGenero", idGenero),
                new SqlParameter("@IdAlbum", idAlbum)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        public string ws_eliminar_artista(int idArtista)
        {
            string query = "DELETE FROM artista WHERE IdArtista = @IdArtista";
            SqlParameter[] parameters = {
                new SqlParameter("@IdArtista", idArtista)
            };
            return ExecuteNonQuery(query, parameters);
        }

        [WebMethod]
        [SoapHeader("Authentication", Required = true)]
        public string ws_eliminar_album(int idAlbum)
        {
            // Verifica la autenticación antes de proceder
            if (!IsAuthenticated())
            {
                return "Usuario o contraseña incorrectos.";
            }

            try
            {
                string query = "DELETE FROM album WHERE IdAlbum = @IdAlbum";
                SqlParameter[] parameters = {
            new SqlParameter("@IdAlbum", idAlbum)
        };
                int filasAfectadas= int.Parse(ExecuteNonQuery(query, parameters));

                if (filasAfectadas > 0)
                {
                    return "Álbum eliminado exitosamente.";
                }
                else if (filasAfectadas == 0)
                {
                    return "Error: No se encontró un álbum con el ID especificado.";
                }
                else
                {
                    return "Error: Ocurrió un problema al intentar eliminar el álbum.";
                }
            }
            catch (Exception ex)
            {
                return $"Error al eliminar el álbum: {ex.Message}";
            }
        }
        private bool IsAuthenticated()
        {
            return Authentication != null && Authenticate(Authentication.Username, Authentication.Password);
        }

        private bool Authenticate(string username, string password)
        {
            // Aquí puedes mejorar la validación de credenciales con una base de datos u otro mecanismo
            return username == "admin" && password == "password";
        }
        private string ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    con.Close();
                    return result > 0 ? "Success" : "Failure";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private DataSet ExecuteQuery(string query)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
        }
    }
}
#endregion