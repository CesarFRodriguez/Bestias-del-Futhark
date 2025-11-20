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
        SendHealthUpdate(player.health);
        SendRoomData(receivedCards);
        if(rm.round == 0)
        {
            SendPlayerReady();
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
    private void ChangeEnemyHealth(MessageData msg){
        enemyHealth = int.Parse(msg.payload);
    }

    private void ChangeCardData(MessageData msg)
    {
        for (int i = 0; i < receivedCards.Count; i++)
        {
            receivedCards[i] = JsonUtility.FromJson<Card>(msg.payload.Split('|')[i]);
        }
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
