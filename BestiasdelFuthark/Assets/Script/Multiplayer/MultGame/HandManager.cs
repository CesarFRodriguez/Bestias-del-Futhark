using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    private List<CardBehaviourMult> cardBehaviours = new List<CardBehaviourMult>();
    public List<CardBehaviourMult> cardsInRoom = new List<CardBehaviourMult>();
    
    public List<Card> hand = new List<Card>();

    private List<bool> freeCard = new List<bool>();
    private List<bool> selectedCard = new List<bool>();

    private int round;
    private bool prepPhaseStarted = false;

    public int tempIndex;
    
    public int selectedIndex = -1;

    public DeckManagerMult deckManager;
    public RenderManager renderManager;
    public RoundManager roundManager;
    public RoomManager roomManager;
    public PlayerManager player;

    private bool defPhaseStarted = false;

    private void Start() {
        deckManager = Object.FindFirstObjectByType<DeckManagerMult>();
        for (int i = 0; i < cards.Count; i++)
        {
            hand.Add(deckManager.defaultCard());
        }
        for (int i = 0; i < cards.Count; i++)
        {
            freeCard.Add(true);
            selectedCard.Add(false);
            DrawCard(i);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            cardBehaviours.Add(cards[i].GetComponent<CardBehaviourMult>());
        }
        for (int i = 0; i < roomManager.roomCards.Count; i++)
        {
            cardsInRoom.Add(roomManager.roomCards[i].GetComponentInChildren<CardBehaviourMult>());
        }
    }

    private void Update() {
        round = roundManager.round;
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

    private void CardInRoom()
    {
        if(selectedIndex != -1 && roomManager.selectedIndex != -1)
        {
            if(roomManager.roomCardData[roomManager.selectedIndex].cardID != "B")
            {
                hand[tempIndex] = roomManager.roomCardData[roomManager.selectedIndex];
                renderManager.RenderCard(hand[tempIndex], cards[tempIndex]);
                roomManager.ChangeCard(roomManager.selectedIndex, deckManager.defaultCard());
                cardBehaviours[tempIndex].isUsed = false;
            }
            if(roomManager.roomCardData[roomManager.selectedIndex].cardID == "B")
            {
                if(!cardBehaviours[selectedIndex].isUsed)
                {
                tempIndex = selectedIndex;
                roomManager.ChangeCard(roomManager.selectedIndex, hand[selectedIndex]);
                hand[selectedIndex] = deckManager.defaultCard();
                renderManager.RenderCard(hand[selectedIndex], cards[selectedIndex]);
                cardBehaviours[tempIndex].isUsed = true;
                }
            }
        }
    }
    private void prepPhase(){
        LookForFree();
        UnblockAllCards();
        CardInRoom();
        BlockCardsIfSelected();
        prepPhaseStarted = true;
    }
    private void defensePhase(){
        UnblockAllCards();
        List<bool> selected = boolChecker();
        if(selectedIndex != -1){
            if(cardBehaviours[selectedIndex].isSelected)
            {
                cardBehaviours[selectedIndex].isUsed = true;
                player.getCard(hand[selectedIndex]);
                hand[selectedIndex] = deckManager.defaultCard();
                renderManager.RenderCard(hand[selectedIndex], cards[selectedIndex]);
            }
        }
        defPhaseStarted = true;
    }
    private void breakPhase(){
        BlockAllCards();
        selectedIndex = -1;
        prepPhaseStarted = false;
        defPhaseStarted = false;
    }

    private void BlockCardsIfSelected()
    {
        List<bool> selected = boolChecker();
        if (selected.Contains(true))
        {
            for (int i = 0; i < cardBehaviours.Count; i++)
            {
                cardBehaviours[i].isFree = false;
            }
        }
        else{
            selectedIndex = -1;
            for (int i = 0; i < cardBehaviours.Count; i++)
            {
                cardBehaviours[i].isFree = true;
            }
        }
    }
    private void BlockAllCards()
    {
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            cardBehaviours[i].isUsed = true;
            renderManager.RenderCard(deckManager.defaultCard(), cards[i]);
        }
    }
    private void UnblockAllCards()
    {
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            if(hand[i].cardID != "B")
            {
                if(!defPhaseStarted && !prepPhaseStarted ) cardBehaviours[i].isUsed = false;
            }
            renderManager.RenderCard(hand[i], cards[i]);
        }
    }
    private void LookForFree(){
        if(!prepPhaseStarted){
            for(int i = 0; i < hand.Count; i++)
            {
                if(hand[i].cardID == "B")
                {
                    freeCard[i] = true;
                }
                DrawCard(i);
            }
        }
    }
    private void DrawCard(int i)
    {
        if (freeCard[i])
        {
            Card drawnCard = deckManager.remain[0];
            deckManager.RemoveFromRemain(1);
            hand[i] = drawnCard;
            renderManager.RenderCard(drawnCard, cards[i]);
            freeCard[i] = false;
        }
    }

    public void ChangeCard(int index, Card cardInfo)
    {
        renderManager.RenderCard(cardInfo, cards[index]);
    }

    private List<bool> boolChecker()
    {
        List<bool> selected = new List<bool>();
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            if(cardBehaviours[i].isSelected){
                selected.Add(true);
                selectedIndex = i;
            } else {
                selected.Add(false);
            }
        }
        return selected;
    }
}