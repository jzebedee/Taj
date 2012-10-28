using Taj.Messages;
using Taj.Messages.Flags;

namespace Taj
{
    public class PalaceUser
    {
        public Palace Host { get; protected set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public UserFlags Flags { get; set; }

        public PalaceUser(Palace host)
        {
            Host = host;
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}