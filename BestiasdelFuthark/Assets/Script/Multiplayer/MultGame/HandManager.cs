using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    public List<Card> hand = new List<Card>();
    private List<bool> freeCard = new List<bool>();
    private List<bool> selectedCard = new List<bool>();

    private int totalCards;

    public DeckManagerMult deckManager;
    public RenderManager renderManager;
    public RoundManager roundManager;
    public RoomManager roomManager;

    private void Start() {
        deckManager = Object.FindFirstObjectByType<DeckManagerMult>();
        totalCards = deckManager.getTotalCards();
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
    
}