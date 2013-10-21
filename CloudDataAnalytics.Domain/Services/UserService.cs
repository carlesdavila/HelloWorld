using System.Threading;
using CloudDataAnalytics.Shared.Entities;
using ServMan.Domain.Interfaces;
using System;
using System.Linq;

namespace CloudDataAnalytics.Domain.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {

        }

        public User GetByLogin(string login)
        {
            //todo: code real instead of fake user data
            return new User(){Login = "cdavila"};
        }

    }
}
