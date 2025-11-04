using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System.Text;
using System.Collections;
using System.Linq;


public class NetworkManagerP2P : MonoBehaviour
{
    public bool isHost = false;
    public NetworkDriver driver;
    private NetworkConnection connection;
    private bool isInitialized = false;
    private bool handshakeConfirmed = false;

    public delegate void MessageReceived(MessageData data);
    public event MessageReceived OnMessageReceived;

    void Update()
    {
        if (!isInitialized) return;

        driver.ScheduleUpdate().Complete();

        if (isHost)
        {
            NetworkConnection c;
            while ((c = driver.Accept()) != default)
            {
                connection = c;
                Debug.Log("[HOST] Client Accepted.");
                var welcome = new MessageData { type = "CONNECTED_OK", payload = "Welcome" };
                SendMessage(welcome);
                Debug.Log("[HOST] Confirmation sent to Client.");
            }
        }
        if (!connection.IsCreated)
        {
            Debug.Log("[HOST] No client connection yet.");
        }
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            switch (cmd)
            {
                case NetworkEvent.Type.Connect:
                    Debug.Log("[CLIENT] Connected to Host.");
                    // Aquí podrías enviar un mensaje de saludo
                    SendMessage(new MessageData { type = "LISTEN?", content = "Client Ready" });
                    break;

                case NetworkEvent.Type.Data:
                    var buffer = new NativeArray<byte>(stream.Length, Allocator.Temp);
                    stream.ReadBytes(buffer);
                    string json = Encoding.UTF8.GetString(buffer.ToArray());
                    buffer.Dispose();

                    var msg = JsonUtility.FromJson<MessageData>(json);
                    Debug.Log($"[NET] Received message: {msg.type}");

                    // manejo interno del handshake
                    if (!isHost && msg.type == "CONNECTED_OK")
                    {
                        handshakeConfirmed = true;
                        Debug.Log("[CLIENT] Handshake confirmed by Host.");
                    }

                    // Si eres host y recibes el saludo del cliente, puedes responder también
                    if (isHost && msg.type == "LISTEN?")
                    {
                        Debug.Log("[HOST] Received LISTEN? from Client — answer CONNECTED_OK");
                        SendMessage(new MessageData { type = "CONNECTED_OK", payload = "Welcome" });
                    }

                    OnMessageReceived?.Invoke(msg);
                    break;

                case NetworkEvent.Type.Disconnect:
                    Debug.LogWarning("Disconnected from Host or host doesn't exist.");
                    connection = default;
                    break;
            }
        }

    }
    public void StartHost(ushort port = 1225)
    {
        if (driver.IsCreated)
            driver.Dispose();

        driver = NetworkDriver.Create();

        // Escuchar en cualquier interfaz IPv4
        var endpoint = NetworkEndpoint.AnyIpv4;
        endpoint.Port = port;

        if (driver.Bind(endpoint) != 0)
        {
            Debug.LogError($"[HOST] No se pudo enlazar al puerto {port}");
            return;
        }

        driver.Listen();
        isHost = true;
        isInitialized = true;

        Debug.Log($"[HOST] Servidor escuchando en {GetLocalIPAddress()}:{port}");
    }


    private string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return "127.0.0.1";
    }


    public void ConnectTo(string ip, int port, System.Action<bool> callback = null)
    {
        Debug.Log($"[CLIENT] Connecting to {ip}:{port}...");

        if (driver.IsCreated && connection.IsCreated)
        {
            Debug.LogWarning("[CLIENT] There's already an active connection.");
            callback?.Invoke(false);
            return;
        }

        try
        {
            driver = NetworkDriver.Create();
            var endpoint = NetworkEndpoint.Parse(ip, (ushort)port);
            connection = driver.Connect(endpoint);
            isInitialized = true;

            Debug.Log($"[CLIENT] Trying to connect to {ip}:{port}...");
            StartCoroutine(WaitForConnection(callback));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[CLIENT] Connection error {ex.Message}");
            callback?.Invoke(false);
        }
    }
    private IEnumerator WaitForConnection(System.Action<bool> callback)
    {
        float timeout = 5f;
        float timer = 0f;

        // Reinicia flag antes de comenzar
        handshakeConfirmed = false;

        while (!handshakeConfirmed && timer < timeout)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        if (handshakeConfirmed)
        {
            Debug.Log("[CLIENT] Connection and handshake successful.");
            callback?.Invoke(true);
        }
        else
        {
            Debug.LogError("[CLIENT] Host didn't answer (timeout).");
            callback?.Invoke(false);
        }
    }

    public void SendMessage(MessageData msg)
    {
        if (!connection.IsCreated) return;

        string json = JsonUtility.ToJson(msg);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        driver.BeginSend(connection, out DataStreamWriter writer);
        writer.WriteBytes(bytes);
        driver.EndSend(writer);
    }
    void OnDestroy()
    {
        if (driver.IsCreated)
            driver.Dispose();
    }
}