using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [Header("Modelos de cartas en escena")]
    public List<GameObject> cards;
    private DeckManager deckManager;

    void Start()
    {
        deckManager = Object.FindFirstObjectByType<DeckManager>();
    }
    public void CheckNextTurn()
    {
        if (deckManager == null) return;

        if (AllCardsDisabled()) // Solo avanza turno si todas se usaron
        {
            deckManager.inTable.Clear();

            for (int i = 0; i < 4 && deckManager.remain.Count > 0; i++)
            {
                Card nextCard = deckManager.remain[0];
                deckManager.remain.RemoveAt(0);
                deckManager.inTable.Add(nextCard);

                if (i >= cards.Count) continue;

                GameObject cardObj = cards[i];
                if (cardObj == null) continue;

                // Reset visual
                Renderer rend = cardObj.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    rend.material.color = Color.white;
                }

                // Reset interacción
                Collider col = cardObj.GetComponent<Collider>();
                if (col != null) col.enabled = true;
                CardBehaviour cb = cardObj.GetComponent<CardBehaviour>();
                if (cb != null) cb.enabled = true;

                ApplyCardTexture(nextCard, cardObj);
                ApplyCardBehaviour(nextCard, cardObj);
            }

            deckManager.UpdateRemain();

            if (deckManager.remain.Count == 0)
            {
                Debug.Log("No quedan más cartas en el mazo.");
            }
        }
    }

    public void ApplyCardTexture(Card card, GameObject cardObj)
    {
        Renderer rend = cardObj.GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            Texture2D tex = Resources.Load<Texture2D>("Cards/" + card.texturePath);

            // Cambiar color según el palo
            Color suitColor = Color.white;
            switch (card.suit)
            {
                case "H": suitColor = new Color(1f, 0.5f, 0.5f); break;
                case "D": suitColor = new Color(1f, 0.7f, 0.5f); break;
                case "C": suitColor = new Color(0.5f, 0.5f, 1f); break;
                case "S": suitColor = new Color(0.5f, 0.7f, 1f); break;
            }

            // Para URP/Lit shader
            if (rend.material.HasProperty("_BaseColor"))
                rend.material.SetColor("_BaseColor", suitColor);
            else
                rend.material.color = suitColor; // fallback

            if (tex != null)
            {
                if (rend.material.HasProperty("_BaseMap"))
                    rend.material.SetTexture("_BaseMap", tex);
                else
                    rend.material.mainTexture = tex; // fallback
            }
            else
            {
                Debug.LogWarning("No se encontró la textura: " + card.texturePath);
            }
        }
    }

    public void ApplyCardBehaviour(Card card, GameObject cardObj)
    {
        CardBehaviour cb = cardObj.GetComponent<CardBehaviour>();
        if (cb != null)
        {
            cb.cardData = card;
        }
        else
        {
            Debug.LogWarning("No se encontró CardBehaviour en el objeto de la carta.");
        }
    }

    // Método que reparte las primeras 4 cartas
    public void DealFirstCards(List<Card> remain)
    {
        for (int i = 0; i < 4 && i < remain.Count; i++)
        {
            ApplyCardTexture(remain[i], cards[i]);
            ApplyCardBehaviour(remain[i], cards[i]);
        }
    }
    public bool AllCardsDisabled()
    {
        foreach (var cardObj in cards)
        {
            Collider col = cardObj.GetComponent<Collider>();
            if (col != null && col.enabled) 
            {
                return false; // Todavía hay cartas activas
            }
        }
        return true; // Todas están deshabilitadas
    }
}

