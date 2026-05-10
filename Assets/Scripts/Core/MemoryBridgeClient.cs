namespace Milehigh.Core
{
    // Error 13 Solution: Implement MemoryBridgeClient interface and class
    public interface IMemoryBridgeClient
    {
        bool IsConnected { get; }
        void EstablishConnection();
        void TerminateConnection();
        void SyncMemoryStream(string streamId);
    }

    public class MemoryBridgeClient : IMemoryBridgeClient
    {
        public bool IsConnected { get; private set; }

        public void EstablishConnection()
        {
            IsConnected = true;
            UnityEngine.Debug.Log("Memory Bridge: Connection established.");
        }

        public void TerminateConnection()
        {
            IsConnected = false;
            UnityEngine.Debug.Log("Memory Bridge: Connection terminated.");
        }

        public void SyncMemoryStream(string streamId)
        {
            if (IsConnected)
            {
                UnityEngine.Debug.Log($"Memory Bridge: Syncing stream {streamId}");
            }
        }
    }
}
