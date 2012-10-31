using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj
{
    public abstract class PalaceObject : BaseNotificationModel
    {
        public abstract double X { get; set; }
        public abstract double Y { get; set; }
    }
}
