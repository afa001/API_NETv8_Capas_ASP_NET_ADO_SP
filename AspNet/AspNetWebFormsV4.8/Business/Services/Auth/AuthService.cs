using System.Configuration;
using System.Threading.Tasks;
using AspNetWebFormsV4._8.Business.Services.Helpers;
using AspNetWebFormsV4._8.Models;

public class AuthService
{
    private readonly string _apiBaseUrl;

    public AuthService()
    {
        _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] + "Login/Login";
    }

    public async Task<string> LoginAsync(LoginModel loginModel)
    {
        var response = await HttpClientHelper.LoginAsync<dynamic>(_apiBaseUrl, loginModel);
        var token = response.token;
        return token;
    }
}