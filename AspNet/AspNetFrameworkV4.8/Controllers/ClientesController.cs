using AspNetFrameworkV4._8.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AspNetFrameworkV4._8.Controllers
{
    public class ClientesController : Controller
    {
        private readonly WebServiceClientGlobal _apiClient = new WebServiceClientGlobal();
        private readonly AuthService _authService = new AuthService();

        private string Token
        {
            get { return Session["Token"] as string; }
            set { Session["Token"] = value; }
        }

        public async Task<ActionResult> Index()
        {
            if (string.IsNullOrEmpty(Token))
            {
                var loginModel = new LoginModel
                {
                    UserName = "username",
                    Password = "password"
                };
                Token = await _authService.LoginAsync(loginModel);
            }

            var clientes = await _apiClient.GetAllClientesAsync(Token);

            foreach (var cliente in clientes)
            {
                if (cliente.TipoCliente == null)
                {
                    cliente.TipoCliente = await _apiClient.GetAllTiposClientesByIdAsync(cliente.IdTipoCliente, Token);
                }
            }

            return View(clientes);
        }

        public async Task<ActionResult> Create()
        {
            if (string.IsNullOrEmpty(Token))
            {
                return RedirectToAction("Index", "Clientes");
            }

            var tiposCliente = await _apiClient.GetAllTiposClientesAsync(Token);
            ViewBag.TiposClientes = new SelectList(tiposCliente, "Id", "TipoCliente");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(TblClientes cliente)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    return RedirectToAction("Index", "Clientes");
                }

                await _apiClient.AddClienteAsync(cliente, Token);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return RedirectToAction("Index", "Clientes");
            }

            var cliente = await _apiClient.GetClienteByIdAsync(id, Token);
            var tiposCliente = await _apiClient.GetAllTiposClientesAsync(Token);
            ViewBag.TiposClientes = new SelectList(tiposCliente, "Id", "TipoCliente");
            return View(cliente);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TblClientes cliente)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    return RedirectToAction("Index", "Clientes");
                }

                await _apiClient.UpdateClienteAsync(cliente.Id, cliente, Token);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return RedirectToAction("Index", "Clientes");
            }

            var cliente = await _apiClient.GetClienteByIdAsync(id, Token);
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return RedirectToAction("Index", "Clientes");
            }

            await _apiClient.DeleteClienteAsync(id, Token);
            return RedirectToAction("Index");
        }
    }
}