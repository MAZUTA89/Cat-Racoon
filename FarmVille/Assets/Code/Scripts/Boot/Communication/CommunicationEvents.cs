using System;

namespace Assets.Code.Scripts.Boot.Communication
{
    public class CommunicationEvents
    {
        public static Action OnStartCommunicateEvent;
        public static Action OnWaitForCommunicateEvent;
        public static Action<int> OnSendDataEvent;
        public static Action<int> OnRecvDataEvent;

        public static void InvokeSendDataEvent(int bytes)
        {
            OnSendDataEvent?.Invoke(bytes);
        }
        public static void InvokeRecvDataEvent(int bytes)
        {
            OnRecvDataEvent?.Invoke(bytes);
        }
        public static void InvokeCommunicationEvent()
        {
            OnStartCommunicateEvent?.Invoke();
        }
        public static void InvokeOnWaitForCommunicateEvent()
        {
            OnWaitForCommunicateEvent?.Invoke();
        }
    }
}
