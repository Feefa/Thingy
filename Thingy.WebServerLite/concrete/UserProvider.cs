using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    /// <summary>
    /// TODO - Revisit this class from a point of view of thread safety
    ///        I'm not sure that messing about with a user that's a reference to a member of the thread-safe list is a great idea
    ///        We might be to split the User details that are included on the request (IUser) from the ones that are stored
    ///        in the userList.
    /// </summary>
    public class UserProvider : IUserProvider
    {
        private readonly IUserFactory userFactory;
        private readonly IKnownUserFactory knownUserFactory;

        private IList<IKnownUser> userList;
        private Mutex mutex = new Mutex(false, "29ddb810-4571-486c-bbd3-8da9e49e1f3c");

        public UserProvider(IUserFactory userFactory, IKnownUserFactory knownUserFactory)
        {
            this.userFactory = userFactory;
            this.knownUserFactory = knownUserFactory;
            userList = new List<IKnownUser>(knownUserFactory.GetDefaultKnownUsers());
        }

        public IUser GetUser(string ipAddress, string userId, string password)
        {
            IUser user = GetUserFromListByIpAddress(ipAddress);

            if (user == null)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    user = userFactory.CreateGuestUser();
                }
                else
                {
                    user = GetUserFromuserListByUserId(userId);

                    if (user == null)
                    {
                        user = AddUserToUserList(ipAddress, userId, password);
                    }
                    else
                    {
                        if (!SetKnownUserIpAddress(userId, password, ipAddress))
                        {
                            user = userFactory.CreateFailedUser();
                        }
                    }
                }
            }

            return user;
        }

        private bool SetKnownUserIpAddress(string userId, string password, string ipAddress)
        {
            bool updated = false;

            mutex.WaitOne();

            try
            {
                IKnownUser knownUser = userList.FirstOrDefault(u => u.UserId == userId && u.Password == password);

                if (knownUser != null)
                {
                    UnlinkIpAddress(ipAddress);
                    knownUser.IpAddress = ipAddress;
                    updated = true;
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return updated;
        }

        /// <summary>
        /// Unlink an IP address from any users it is associated with
        /// NVB. This method assumes that the caller already holds the userList mutex. Be careful!
        /// </summary>
        /// <param name="ipAddress"></param>
        private void UnlinkIpAddress(string ipAddress)
        {
            foreach (var u in userList.Where(u => u.IpAddress == ipAddress))
            {
                u.IpAddress = string.Empty;
            }
        }

        private IUser AddUserToUserList(string ipAddress, string userId, string password)
        {
            IUser user;

            mutex.WaitOne();

            try
            {
                UnlinkIpAddress(ipAddress);
                IKnownUser knownUser = knownUserFactory.Create(ipAddress, userId, password);
                userList.Add(knownUser);
                user = userFactory.CreateUserFromKnownUser(knownUser);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return user;
        }

        private IUser GetUserFromuserListByUserId(string userId)
        {
            IUser user;

            mutex.WaitOne();

            try
            {
                IKnownUser knownUser = userList.FirstOrDefault(u => u.UserId == userId);
                user = knownUser != null ? userFactory.CreateUserFromKnownUser(knownUser) : null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return user;
        }

        private IUser GetUserFromListByIpAddress(string ipAddress)
        {
            IUser user;
            mutex.WaitOne();

            try
            {
                IKnownUser knownUser = userList.FirstOrDefault(u => u.IpAddress == ipAddress);
                user = knownUser != null ? userFactory.CreateUserFromKnownUser(knownUser) : null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return user;
        }
    }
}
