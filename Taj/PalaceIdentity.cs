using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj
{
    /// <summary>
    /// A client class that contains palace-specific configurations and assets,
    /// but is independent of connection or state.
    /// </summary>
    public class PalaceIdentity : BaseNotificationModel
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
