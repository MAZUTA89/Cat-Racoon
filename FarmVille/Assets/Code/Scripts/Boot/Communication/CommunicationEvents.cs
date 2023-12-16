using System;

namespace Assets.Code.Scripts.Boot.Communication
{
    public class CommunicationEvents
    {
        public static Action OnStartCommunicateEvent;
        public static Action OnDataSendedEvent;
        public static Action oncomplitedAdded;
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
        public static void InvokeDataSendedIvent()
        {
            OnDataSendedEvent?.Invoke();
        }
        public static void InvokeOnComplitedAdded()
        {
            oncomplitedAdded?.Invoke();
        }
    }
}
