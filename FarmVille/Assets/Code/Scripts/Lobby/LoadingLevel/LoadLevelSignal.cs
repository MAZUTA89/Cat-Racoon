namespace Assets.Code.Scripts.Lobby
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
