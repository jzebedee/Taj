using System;
using System.Collections.Generic;
using System.Linq;
using Taj.Messages;

namespace Taj
{
    public class Palace
    {
        public Version Version { get; set; }
        public Uri HTTPServer { get; set; }
        public ServerPermissions Permissions { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }

        private List<PalaceUser> _users = new List<PalaceUser>();
        public IEnumerable<PalaceUser> Users { get { return _users; } }

        PalaceUser GetUserByID(int UserID)
        {
            return (from u in Users where u.ID == UserID select u).SingleOrDefault();
        }
        public PalaceUser GetUserByID(int UserID, bool create = false)
        {
            PalaceUser u;
            if ((u = GetUserByID(UserID)) == null && create)
            {
                u = new PalaceUser() { ID = UserID };
                _users.Add(u);
            }

            return u;
        }
    }
}