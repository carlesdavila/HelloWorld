using CloudDataAnalytics.Domain.Interfaces;
using CloudDataAnalytics.Shared.Entities;


namespace ServMan.Domain.Interfaces
{
    public interface IUserService : IServices<User>
    {
        User GetByLogin(string login);
    }
}
