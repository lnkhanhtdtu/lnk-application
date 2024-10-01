using Lnk.Application.DTOs;

namespace Lnk.Application.Abstracts
{
    public interface IUserService
    {
        Task<ResponseDatatable<UserModel>> GetUserPagination(RequestDataTable request);
        Task<ResponseModel> Save(AccountDTO model);
        Task<AccountDTO> GetUserById(string id);
    }
}
