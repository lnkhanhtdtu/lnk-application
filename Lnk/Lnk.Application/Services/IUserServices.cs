using Lnk.Application.DTOs;

namespace Lnk.Application.Services
{
    public interface IUserServices
    {
        Task<ResponseModel> CheckLogin(string username, string password, bool rememberMe);

        Task<ResponseDatatable<UserModel>> GetUserPagination(RequestDataTable request);
    }
}
