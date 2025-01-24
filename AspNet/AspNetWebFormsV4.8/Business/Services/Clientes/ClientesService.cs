using AspNetWebFormsV4._8.Business.Services.Helpers;
using AspNetWebFormsV4._8.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AspNetWebFormsV4._8.Business.Clientes
{
    public class ClientesService
    {
        private readonly string _apiBaseUrl;

        public ClientesService()
        {
            _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] + "Clientes";
        }

        public async Task<List<TblClientes>> GetAllClientesAsync()
        {
            return await HttpClientHelper.GetAsync<List<TblClientes>>(_apiBaseUrl);
        }

        public async Task<TblClientes> GetClienteByIdAsync(int id)
        {
            return await HttpClientHelper.GetAsync<TblClientes>($"{_apiBaseUrl}/{id}");
        }

        public async Task AddClienteAsync(TblClientes cliente)
        {
            await HttpClientHelper.PostAsync<TblClientes>(_apiBaseUrl, cliente);
        }

        public async Task UpdateClienteAsync(int id, TblClientes cliente)
        {
            await HttpClientHelper.PutAsync($"{_apiBaseUrl}/{id}", cliente);
        }

        public async Task DeleteClienteAsync(int id)
        {
            await HttpClientHelper.DeleteAsync($"{_apiBaseUrl}/{id}");
        }
    }
}