using brainfreeze_new.Server.Models;

namespace brainfreeze_new.Server.Services
{
    public class UserService
    {
        private string _username;

        public void StoreUsername(string username)
        {
            _username = username;
        }

        public object GetUserInfo()
        {

            if (_username != null)
            {
                return new { username = _username };
            }

            return null;
        }
    }

}
