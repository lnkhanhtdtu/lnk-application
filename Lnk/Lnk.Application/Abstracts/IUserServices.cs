using Lnk.Application.DTOs;

namespace Lnk.Application.Abstracts
{
    public interface IUserServices
    {
        Task<ResponseDatatable<UserModel>> GetUserPagination(RequestDataTable request);
        Task<ResponseModel> CreateUser(AccountDTO model);
        Task<AccountDTO> GetUserById(string id);
    }
}
