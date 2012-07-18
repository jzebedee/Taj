using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages
{
    public interface IFormattedMessage
    {
        byte[] GetBytes();
    }
}
