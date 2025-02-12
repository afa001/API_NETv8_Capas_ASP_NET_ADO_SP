﻿using AspNetWebFormsV4._8.Business.Clientes;
using AspNetWebFormsV4._8.Business.Services.Clientes;
using AspNetWebFormsV4._8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetWebFormsV4._8.Web.Clientes
{
    public partial class Clientes : System.Web.UI.Page
    {
        private readonly AuthService _authService = new AuthService();
        private readonly TipoClienteService _tipoClienteService = new TipoClienteService();
        private readonly ClientesService _clientesService = new ClientesService();

        private string _token
        {
            get { return Session["Token"] as string; }
            set { Session["Token"] = value; }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoginModel loginModel = new LoginModel
                {
                    UserName = "username",
                    Password = "password"
                };
                _token = await _authService.LoginAsync(loginModel);
                await GetTiposClienteAsync(ddlTipoCliente, _token);
                await LoadClientesAsync(_token);
            }
        }

        private async Task LoadClientesAsync(string _token)
        {
            var clientes = await _clientesService.GetAllClientesAsync(_token);

            foreach (var cliente in clientes)
            {
                cliente.TipoCliente = await _tipoClienteService.GetTipoClienteByIdAsync(cliente.IdTipoCliente, _token);
            }

            gvClientes.DataSource = clientes;
            gvClientes.DataBind();
        }

        private async Task GetTiposClienteAsync(DropDownList dropdown,string token)
        {
            var tiposCliente = await _tipoClienteService.GetAllTipoClientesAsync(token);

            dropdown.DataSource = tiposCliente;
            dropdown.DataTextField = "TipoCliente";
            dropdown.DataValueField = "Id";

            dropdown.DataBind();

            dropdown.Items.Insert(0, new ListItem("--Seleccione un tipo de cliente--", "0"));
        }

        protected async void btnAddCliente_Click(object sender, EventArgs e)
        {
            var cliente = new TblClientes
            {
                RazonSocial = txtRazonSocial.Text,
                RFC = txtRFC.Text,
                FechaCreacion = DateTime.TryParse(txtFechaCreacion.Text, out DateTime fecha) ? fecha : DateTime.Now,
                IdTipoCliente = int.Parse(ddlTipoCliente.SelectedValue)
            };

            await _clientesService.AddClienteAsync(cliente, _token);
            LimpiarCampos();
            await LoadClientesAsync(_token);
        }

        protected async void gvClientes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvClientes.DataKeys[e.RowIndex].Value);
            await _clientesService.DeleteClienteAsync(id, _token);
            await LoadClientesAsync(_token);
        }

        protected async void gvClientes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvClientes.EditIndex = e.NewEditIndex;
            await LoadClientesAsync(_token);
        }

        protected async void gvClientes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idRow = Convert.ToInt32(gvClientes.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvClientes.Rows[e.RowIndex];

            TblClientes cliente = new TblClientes();
            
            cliente.Id = int.Parse(((TextBox)row.Cells[0].Controls[0]).Text.Trim());
            cliente.RazonSocial = ((TextBox)row.Cells[1].Controls[0]).Text; 
            cliente.RFC = ((TextBox)row.Cells[2].Controls[0]).Text;

            string fechaCreacionStr = ((TextBox)row.FindControl("txtFechaCreacion")).Text;
            cliente.FechaCreacion = DateTime.Parse(fechaCreacionStr);

            var ddlTipoCliente = (DropDownList)row.FindControl("ddlTipoCliente");
            if (ddlTipoCliente == null) return;
            cliente.IdTipoCliente = int.Parse(ddlTipoCliente.SelectedValue);

            await _clientesService.UpdateClienteAsync(idRow, cliente, _token);

            gvClientes.EditIndex = -1;
            await LoadClientesAsync(_token);
        }

        protected async void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Verifica si la fila está en modo de edición
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {

                var ddlTipoCliente = (DropDownList)e.Row.FindControl("ddlTipoCliente");
                if (ddlTipoCliente != null)
                {
                    await GetTiposClienteAsync(ddlTipoCliente, _token);

                    // Seleccionar el valor actual del cliente
                    var tipoClienteId = DataBinder.Eval(e.Row.DataItem, "IdTipoCliente");
                    if (tipoClienteId != null)
                    {
                        ddlTipoCliente.SelectedValue = tipoClienteId.ToString();
                    }
                }

                var txtFechaCreacion = (TextBox)e.Row.FindControl("txtFechaCreacion");
                if (txtFechaCreacion != null)
                {
                    // Obtiene el valor actual de FechaCreacion
                    var fechaCreacion = DataBinder.Eval(e.Row.DataItem, "FechaCreacion");
                    if (fechaCreacion != null)
                    {
                        // Formatea la fecha como yyyy-MM-dd para que sea válida en el campo tipo Date
                        txtFechaCreacion.Text = Convert.ToDateTime(fechaCreacion).ToString("yyyy-MM-dd");
                    }
                }
            }
        }

        protected async void gvClientes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvClientes.EditIndex = -1;
            await LoadClientesAsync(_token);
        }

        protected void LimpiarCampos()
        {
            txtRazonSocial.Text = string.Empty;
            txtRFC.Text = string.Empty;
            txtFechaCreacion.Text = string.Empty;
            ddlTipoCliente.SelectedIndex = 0;
        }

    }
}