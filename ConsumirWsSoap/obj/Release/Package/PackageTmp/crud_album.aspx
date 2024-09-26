<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="crud_album.aspx.cs" Inherits="ConsumirWsSoap.crud_album" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <title>CRUD Álbum</title>
    <style>
        .container { width: 80%; margin: auto; }
        .form-group { margin-bottom: 15px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Administrar Álbumes</h2>
            <asp:GridView ID="GridViewAlbumes" runat="server" AutoGenerateColumns="False"
                DataKeyNames="IdAlbum"
                OnRowEditing="GridViewAlbumes_RowEditing"
                OnRowCancelingEdit="GridViewAlbumes_RowCancelingEdit"
                OnRowDeleting="GridViewAlbumes_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="IdAlbum" HeaderText="IdAlbum" ReadOnly="True" />
                    <asp:BoundField DataField="TituloAlbum" HeaderText="Título" />
                    <asp:BoundField DataField="NombreArtista" HeaderText="Artista" />
                    <asp:BoundField DataField="NombreGenero" HeaderText="Género" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
            <hr />
            <h3><asp:Literal ID="LiteralTitle" runat="server" Text="Agregar Álbum" /></h3>
            <asp:HiddenField ID="HiddenFieldIdAlbum" runat="server" />
            <div class="form-group">
                <label for="txtTitulo">Título:</label>
                <asp:TextBox ID="txtTitulo" runat="server" />
            </div>
            <div class="form-group">
                <label for="ddlArtista">Artista:</label>
                <asp:DropDownList ID="ddlArtista" runat="server" />
            </div>
            <div class="form-group">
                <label for="ddlGenero">Género:</label>
                <asp:DropDownList ID="ddlGenero" runat="server" />
            </div>
            <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
        </div>
    </form>
</body>
</html>