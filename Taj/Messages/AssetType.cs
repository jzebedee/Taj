using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages
{
    public enum AssetType : int //sint32
    {
        PROP        = 0x50726f70, //'Prop' - RT_PROP is for assets which are props. 
        USERBASE    = 0x55736572, //'User' - RT_USERBASE is for assets which represent users (the server has vestigial support for a user database stored as a collection of assets). 
        IPUSERBASE  = 0x49557372, //'IUsr' - RT_IPUSERBASE is defined but not used and is an historical artifact.
    }
}
