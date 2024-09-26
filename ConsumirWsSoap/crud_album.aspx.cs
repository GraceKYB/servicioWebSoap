using ConsumirWsSoap.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ConsumirWsSoap
{
    public partial class crud_album : System.Web.UI.Page
    {
        WsSoapSoapClient wsClient = new WsSoapSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAlbumes();
                LoadArtistas();
                LoadGeneros();
            }
        }

        private void LoadAlbumes()
        {
            DataSet ds = wsClient.ws_listar_albumes();
            if (ds != null && ds.Tables.Count > 0)
            {
                GridViewAlbumes.DataSource = ds.Tables[0];
                GridViewAlbumes.DataBind();
            }
        }

        private void LoadArtistas()
        {
            DataSet ds = wsClient.ws_listar_artistas();
            if (ds != null && ds.Tables.Count > 0)
            {
                ddlArtista.DataSource = ds.Tables[0];
                ddlArtista.DataTextField = "NombreArtista";
                ddlArtista.DataValueField = "IdArtista";
                ddlArtista.DataBind();
                ddlArtista.Items.Insert(0, new ListItem("Seleccione Artista", "0"));
            }
        }

        private void LoadGeneros()
        {
            DataSet ds = wsClient.ws_listar_generos();
            if (ds != null && ds.Tables.Count > 0)
            {
                ddlGenero.DataSource = ds.Tables[0];
                ddlGenero.DataTextField = "NombreGenero";
                ddlGenero.DataValueField = "IdGenero";
                ddlGenero.DataBind();
                ddlGenero.Items.Insert(0, new ListItem("Seleccione Género", "0"));
            }
        }

        private void EditAlbum(int idAlbum)
        {
            DataSet ds = wsClient.ws_listar_albumes();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow row = dt.Select("IdAlbum = " + idAlbum).FirstOrDefault();
                if (row != null)
                {
                    HiddenFieldIdAlbum.Value = row["IdAlbum"].ToString();
                    txtTitulo.Text = row["TituloAlbum"].ToString();
                    ddlArtista.SelectedValue = row["IdArtista"].ToString();
                    ddlGenero.SelectedValue = row["IdGenero"].ToString();
                    LiteralTitle.Text = "Editar Álbum";
                }
            }
        }

        protected void GridViewAlbumes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit" || e.CommandName == "Delete")
            {
                int idAlbum;
                if (int.TryParse(e.CommandArgument.ToString(), out idAlbum))
                {
                    if (e.CommandName == "Edit")
                    {
                        EditAlbum(idAlbum);
                    }
                    else if (e.CommandName == "Delete")
                    {
                        DeleteAlbum(idAlbum);
                    }
                }
            }
        }

        private void DeleteAlbum(int idAlbum)
        {
            string result = wsClient.ws_eliminar_album(idAlbum);
            Response.Write("<script>alert('" + result + "');</script>");
            LoadAlbumes();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int idAlbum;
            bool isEdit = int.TryParse(HiddenFieldIdAlbum.Value, out idAlbum);

            string result;
            if (isEdit)
            {
                result = wsClient.ws_editar_album(idAlbum, txtTitulo.Text, int.Parse(ddlArtista.SelectedValue), int.Parse(ddlGenero.SelectedValue));
            }
            else
            {
                result = wsClient.ws_insertar_album(txtTitulo.Text, int.Parse(ddlArtista.SelectedValue), int.Parse(ddlGenero.SelectedValue));
            }

            Response.Write("<script>alert('" + result + "');</script>");
            ClearForm();
            LoadAlbumes();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            HiddenFieldIdAlbum.Value = string.Empty;
            txtTitulo.Text = string.Empty;
            ddlArtista.SelectedIndex = 0;
            ddlGenero.SelectedIndex = 0;
            LiteralTitle.Text = "Agregar Álbum";
        }

        protected void GridViewAlbumes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int idAlbum = Convert.ToInt32(GridViewAlbumes.DataKeys[e.NewEditIndex].Value);
            EditAlbum(idAlbum);

        }

        //protected void GridViewAlbumes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
            // Obtén el ID del álbum desde los DataKeys
           // int idAlbum = Convert.ToInt32(GridViewAlbumes.DataKeys[e.RowIndex].Value);

            // Obtén los valores de las celdas editadas
            //string titulo = (string)e.NewValues["TituloAlbum"];
            //int idArtista = Convert.ToInt32(e.NewValues["IdArtista"]);
           // int idGenero = Convert.ToInt32(e.NewValues["IdGenero"]);

            // Llama al método de actualización del servicio web
            //string result = wsClient.ws_editar_album(idAlbum, titulo, idArtista, idGenero);

            // Muestra el resultado de la actualización
            //Response.Write("<script>alert('" + result + "');</script>");

            // Restablece el índice de edición y recarga la lista de álbumes
           // GridViewAlbumes.EditIndex = -1;
            //LoadAlbumes();
        //}

        protected void GridViewAlbumes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAlbumes.EditIndex = -1;
            LoadAlbumes();
        }
        protected void GridViewAlbumes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Obtén el ID del álbum desde el GridView
            int idAlbum = Convert.ToInt32(GridViewAlbumes.DataKeys[e.RowIndex].Value);

            // Llama al método de eliminación del servicio web
            string result = wsClient.ws_eliminar_album(idAlbum);

            // Muestra el resultado de la eliminación
            Response.Write("<script>alert('" + result + "');</script>");

            // Recarga la lista de álbumes
            LoadAlbumes();
        }
    }

}