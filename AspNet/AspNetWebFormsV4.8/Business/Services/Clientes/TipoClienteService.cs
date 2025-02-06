using AspNetWebFormsV4._8.Business.Services.Helpers;
using AspNetWebFormsV4._8.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AspNetWebFormsV4._8.Business.Services.Clientes
{
    public class TipoClienteService
    {
        private readonly string _apiBaseUrl;

        public TipoClienteService()
        {
            _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] + "TipoCliente";
        }

        public async Task<List<CatTipoCliente>> GetAllTipoClientesAsync(string token)
        {
            return await HttpClientHelper.GetAsync<List<CatTipoCliente>>(_apiBaseUrl, token);
        }

        public async Task<CatTipoCliente> GetTipoClienteByIdAsync(int id, string token)
        {
            return await HttpClientHelper.GetAsync<CatTipoCliente>($"{_apiBaseUrl}/{id}", token);
        }
    }
}