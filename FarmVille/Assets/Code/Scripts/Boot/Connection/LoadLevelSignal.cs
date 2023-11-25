using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.Boot
{
    public class LoadLevelSignal
    {
        public ConnectionType ConnectionType { get; private set; }
        public LoadLevelSignal(ConnectionType connectionType)
        {
            ConnectionType = connectionType;
        }
    }
}
