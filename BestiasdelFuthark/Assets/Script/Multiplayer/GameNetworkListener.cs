using UnityEngine;

public class GameNetworkListener : MonoBehaviour
{
    private NetworkManagerP2P network;

    private void Start()
    {
        network = Object.FindFirstObjectByType<NetworkManagerP2P>();
        if (network != null)
            network.OnMessageReceived += HandleMessage;
    }

    private void OnDestroy()
    {
        if (network != null)
            network.OnMessageReceived -= HandleMessage;
    }

    private void HandleMessage(MessageData msg)
    {
        Debug.Log($"[GAME] Mensaje recibido: {msg.type} -> {msg.payload}");
    }
}
