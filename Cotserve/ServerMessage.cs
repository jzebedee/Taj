using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palace.Messages;

namespace Cotserve
{
    public abstract class ServerMessage : IOutgoingMessage
    {
        public abstract void Write();
    }
}