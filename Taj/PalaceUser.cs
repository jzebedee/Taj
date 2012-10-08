using Taj.Messages;
using UserID = System.Int32;

namespace Taj
{
    public class PalaceUser
    {
        public string Name { get; set; }
        public UserID ID { get; set; }
        public UserFlags Flags { get; set; }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}