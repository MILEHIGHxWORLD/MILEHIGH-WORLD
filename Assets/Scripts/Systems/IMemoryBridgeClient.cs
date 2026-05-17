namespace Milehigh.World.Systems
{
    // Error 13 Fix: Interface definition for MemoryBridge
    public interface IMemoryBridgeClient
    {
        void SyncRealityAnchor(string anchorId);
        bool IsConnected();
    }
}
