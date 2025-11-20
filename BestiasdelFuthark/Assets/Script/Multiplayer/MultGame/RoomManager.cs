using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public RoundManager rm;
    public RenderManager render;
    public HandManager hand;
    public DeckManagerMult deckManager;
    public GameNetworkListener netListener;

    public List<GameObject> roomCards = new List<GameObject>();
    public List<Card> roomCardData = new List<Card>();
    public PlayerManager player;

    public List<CardBehaviourMult> roomCardBehaviours = new List<CardBehaviourMult>();

    private int round = 0;
    public int selectedIndex = -1;
    private bool defPhaseStarted = false;
    private bool prepPhaseStarted = false;
    private bool breakPhaseStarted = false;

    private bool started = false;

    private void Start() {
        deckManager = Object.FindFirstObjectByType<DeckManagerMult>();
        for (int i = 0; i < roomCards.Count; i++)
        {
            roomCardData.Add(deckManager.defaultCard());
        }
        for (int i = 0; i < roomCards.Count; i++)
        {
            roomCardBehaviours.Add(roomCards[i].GetComponentInChildren<CardBehaviourMult>());
        }
        started = true;
    }
    private void Update() {
        round = rm.round;
        switch(round)
        {
            case 0:
                prepPhase();
                break;
            case 2:
                defensePhase();
                break;
            default:
                breakPhase();
                break;
        }
    }

    private void prepPhase(){
        EmptyRoom();
        UnblockAllCards();
        BlockCardsIfSelected();
        prepPhaseStarted = true;
        breakPhaseStarted = false;
    }
    private void defensePhase(){
        selectedIndex = -1;
        UnblockAllCards();
        getEnemyCards();
        List<bool> selected = boolChecker();
        if(selectedIndex != -1){
            if(roomCardBehaviours[selectedIndex].isSelected)
            {
                roomCardBehaviours[selectedIndex].isUsed = true;
                player.getCard(roomCardData[selectedIndex]);
            }
        }
        defPhaseStarted = true;
        breakPhaseStarted = false;
    }

    private void breakPhase(){
        netListener.SendRoomData(roomCardData);
        defPhaseStarted = false;
        prepPhaseStarted = false;
        selectedIndex = -1;
        Punishment();
        BlockAllCards();
    }
    private void EmptyRoom(){
        if(!prepPhaseStarted && started)
        {
            for(int i = 0; i < roomCardData.Count; i++){
                roomCardData[i] = deckManager.defaultCard();
                roomCardData[i].suit = "B";
                render.RenderCard(roomCardData[i], roomCards[i]);
            }
        }
    }
    private void Punishment(){
        if(!breakPhaseStarted)
        {
            for(int i = 0; i < roomCardData.Count; i++){
                if(roomCardData[i].cardID == "B"){
                    Card tempCard = new Card();
                    tempCard.suit = "S";
                    tempCard.number = 5;
                    player.getCard(tempCard);
                    roomCardData[i].suit = "H";
                    roomCardData[i].number = 5;
                }
            }
            breakPhaseStarted = true;
        }
    }
    public void ChangeCard(int index, Card cardInfo)
    {
        render.RenderCard(cardInfo, roomCards[index]);
        roomCardData[index] = cardInfo;
    }
    private void BlockCardsIfSelected()
    {
        List<bool> selected = boolChecker();
        if (selected.Contains(true))
        {
        for (int i = 0; i < roomCardBehaviours.Count; i++)
            {
                roomCardBehaviours[i].isFree = false;
            }
        }else{
            selectedIndex = -1;
            for (int i = 0; i < roomCardBehaviours.Count; i++)
            {
                roomCardBehaviours[i].isFree = true;
            }
        }
    }
    private List<bool> boolChecker()
    {
        List<bool> selected = new List<bool>();
        for (int i = 0; i < roomCardBehaviours.Count; i++)
        {
            if(roomCardBehaviours[i].isSelected){
                selected.Add(true);
                selectedIndex = i;
            } else {
                selected.Add(false);
            }
        }
        return selected;
    }
    private void BlockAllCards()
    {
        for (int i = 0; i < roomCardBehaviours.Count; i++)
        {
            roomCardBehaviours[i].isUsed = true;
        }
    }
    private void UnblockAllCards()
    {
        for (int i = 0; i < roomCardBehaviours.Count; i++)
        {
            if(!defPhaseStarted) 
            {
                roomCardBehaviours[i].isFree = true;
                roomCardBehaviours[i].isUsed = false;
            }
        }
    }
    private void getEnemyCards()
    {
        if(!defPhaseStarted)
        {
            if(netListener.receivedCards.Count == roomCardData.Count)
            {
                for (int i = 0; i < roomCardData.Count; i++)
                {
                    roomCardData[i] = netListener.receivedCards[i];
                    render.RenderCard(roomCardData[i], roomCards[i]);
                }
                netListener.receivedCards.Clear();
            }
            else
            {
                for (int i = 0; i < roomCardData.Count; i++)
                {
                    roomCardData[i] = deckManager.deck[15];
                    render.RenderCard(deckManager.deck[15], roomCards[i]);
                }
            }
        }
    }
}