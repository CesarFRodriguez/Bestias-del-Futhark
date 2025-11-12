using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientManager : MonoBehaviour
{
    public TMP_InputField ipInput, portInput;
    public TMP_Text statusText;
    public Button connectButton;
    public TMP_Text connectButtonText;
    public GameObject ClientPanel;
    private NetworkManagerP2P network;

    void Awake()
    {
        network = Object.FindAnyObjectByType<NetworkManagerP2P>();
    }
    void Update()
    {
        if (network.isHost)
        {
            ClientPanel.SetActive(false);
        }
        else
        {
            ClientPanel.SetActive(true);
        }
    }
    void Start()
    {
        network = FindFirstObjectByType<NetworkManagerP2P>();

        if (connectButton != null)
            connectButton.onClick.AddListener(OnConnectPressed);

        if (statusText != null)
            statusText.text = "Waiting for connection...";
        if (network != null)
        {
            // Suscripción única al recibir mensajes
            network.OnMessageReceived += HandleNetworkMessage;
        }
    }
        private void OnConnectPressed()
    {
        string ip = ipInput.text.Trim();
        string portStr = portInput.text.Trim();

        if (string.IsNullOrEmpty(ip))
        {
            statusText.text = "Invalid IP address.";
            return;
        }

        if (!int.TryParse(portStr, out int port))
        {
            statusText.text = "Invalid port number.";
            return;
        }

        statusText.text = $"Connecting to {ip}:{port} ...";
        Debug.Log($"[CLIENT] Trying to connect to {ip}:{port}");
        network.ConnectTo(ip, port, OnConnectionResult);
    }
    private void OnConnectionResult(bool success)
    {
        if (success)
        {
            statusText.text = "Connected to host!";
            connectButton.interactable = false;
            connectButtonText.text = "Waiting for host...";
            Debug.Log("[CLIENT] Successfully connected to host.");
            gameObject.SetActive(false); // Oculta el panel
        }
        else
        {
            statusText.text = "Connection failed.";
            Debug.LogError("[CLIENT] Could not connect to host.");
        }
    }
    private void HandleNetworkMessage(MessageData msg)
    {
        Debug.Log($"[CLIENT] Mensaje recibido: {msg.type}");

        switch (msg.type)
        {
            case "CHANGE_SCENE":
               UnityEngine.SceneManagement.SceneManager.LoadScene(msg.payload);
                break;

            default:
                Debug.LogWarning($"[CLIENT] Tipo de mensaje desconocido: {msg.type}");
                break;
        }
    }

}