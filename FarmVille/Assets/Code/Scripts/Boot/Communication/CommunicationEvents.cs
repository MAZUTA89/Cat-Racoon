using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.Boot.Communication
{
    public class CommunicationEvents
    {
        public static Action OnStartCommunicateEvent;
        public static Action OnDataSendedEvent;

        public static void InvokeCommunicationEvent()
        {
            OnStartCommunicateEvent?.Invoke();
        }
        public static void InvokeDataSendedIvent()
        {
            OnDataSendedEvent?.Invoke();
        }
    }
}
