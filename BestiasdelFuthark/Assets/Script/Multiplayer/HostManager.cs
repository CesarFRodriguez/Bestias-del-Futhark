using UnityEngine;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine.UI;

public class HostManager : MonoBehaviour
{
    public TMP_Text ipText, portText;
    public TMP_Text statusText;
    public Button startHostButton;
    public TMP_Text startHostButtonText;
    public NetworkManagerP2P p2p;
    public GameObject HostPanel;
    public string sceneToLoad;

    public string defaultPort = "1225";

    void Awake()
    {
        p2p = Object.FindAnyObjectByType<NetworkManagerP2P>();
    }

    void Start()
    {
        startHostButton.onClick.AddListener(NextScene);
        startHostButton.interactable = false;
        startHostButtonText.text = "Waiting for players...";
        if (p2p == null)
        {
            Debug.LogError("[HOST] NetworkManagerP2P not found in scene.");
            return;
        }

        if (p2p.isHost)
        {
            InicializarHost();
        }
    }

    void Update()
    {
        if (p2p != null)
            HostPanel.SetActive(p2p.isHost);
        if (p2p.connection.IsCreated)
        {
            statusText.text = "A player has connected!";
            startHostButton.interactable = true;
            startHostButtonText.text = "Start Game";
        }else
        {
            statusText.text = "...";
        }
    }

    void InicializarHost()
    {
        string localIP = GetLocalIPAddress();

        if (ipText != null)
            ipText.text = localIP;

        if (portText != null)
            portText.text = defaultPort;

        // Crear servidor si no se ha hecho aún
        if (!p2p.driver.IsCreated)
        {
            Debug.Log("[HOST] Starting Host...");
            p2p.StartHost(ushort.Parse(defaultPort)); // 👈 función nueva que ahora añadimos
        }
    }
    string GetLocalIPAddress()
    {
        try
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                .First(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
        }
        catch
        {
            Debug.LogWarning("Could not detect local IP address.");
            return "Not detected";
        }
    }
    void NextScene()
    {
        p2p.SendMessage(new MessageData {
            type = "CHANGE_SCENE",
            payload = sceneToLoad
        });
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
