using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameNetworkListener : MonoBehaviour
{
    private NetworkManagerP2P network;
    public List<Card> receivedCards = new List<Card>();
    public PlayerDataMult player;
    public TextMeshProUGUI enemyHealthText;
    public RoundManager rm;
    public bool enemyReady = false;
    public int enemyHealth = 20;
    private int lastHealth = -1;
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

    private void Update() {
        enemyHealthText.text = enemyHealth.ToString() + "/20";

        if (player.health != lastHealth)
        {
            lastHealth = player.health;
            SendHealthUpdate(lastHealth);
        }
    }


    private void HandleMessage(MessageData msg)
    {
        switch (msg.type)
        {
            case "ROOM_DATA":
                ChangeCardData(msg);
                break;
            case "HEALTH_UPDATE":
                ChangeEnemyHealth(msg);
                break;
            case "PLAYER_READY":
                enemyReady = true;
                break;
            default:
                Debug.LogWarning("Unknown message type received: " + msg.type);
                break;
        }
    }
    private void SendHealthUpdate(int health)
    {
        MessageData msg = new MessageData
        {
            type = "HEALTH_UPDATE",
            payload = health.ToString()
        };
        network.SendMessage(msg);
    }
    private void ChangeEnemyHealth(MessageData msg)
    {
        if (int.TryParse(msg.payload, out int h))
            enemyHealth = h;
            if (enemyHealth <= 0)
            {
                rm.ready = false;
                enemyReady = false;
                player.WinGame();
            }
        else
            Debug.LogWarning("[NET] HEALTH_UPDATE payload inválido: " + msg.payload);
    }


    private void ChangeCardData(MessageData msg)
    {
        if (string.IsNullOrEmpty(msg.payload))
        {
            Debug.LogWarning("[NET] ROOM_DATA payload vacío.");
            return;
        }

        string[] parts = msg.payload.Split('|');

        receivedCards.Clear();

        foreach (string json in parts)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning("[NET] ROOM_DATA parte vacía, saltando.");
                continue;
            }

            try
            {
                Card c = JsonUtility.FromJson<Card>(json);
                if (c != null)
                    receivedCards.Add(c);
                else
                    Debug.LogWarning("[NET] JsonUtility devolvió null en ROOM_DATA.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[NET] Error parseando ROOM_DATA: {ex.Message}\nJSON: {json}");
            }
        }

        Debug.Log($"[NET] ROOM_DATA recibido con {receivedCards.Count} cartas.");
    }



    public void SendRoomData(List<Card> sentCards)
    {
        string payload = "";
        for (int i = 0; i < sentCards.Count; i++)
        {
            payload += JsonUtility.ToJson(sentCards[i]);
            if (i < sentCards.Count - 1)
                payload += "|";
        }

        MessageData msg = new MessageData
        {
            type = "ROOM_DATA",
            payload = payload
        };
        network.SendMessage(msg);
    }

    public void SendPlayerReady()
    {
        MessageData msg = new MessageData
        {
            type = "PLAYER_READY",
            payload = "READY"
        };
        network.SendMessage(msg);
    }

}
