using Lnk.Application.DTOs;

namespace Lnk.Application.Abstracts
{
    public interface IAuthenticationService
    {
        Task<ResponseModel> CheckLogin(string username, string password, bool rememberMe);
    }
}