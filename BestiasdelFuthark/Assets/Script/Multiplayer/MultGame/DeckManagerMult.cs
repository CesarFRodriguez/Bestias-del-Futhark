using UnityEngine;
using System.Collections.Generic;

public class DeckManagerMult : MonoBehaviour
{
    [Header("Archivo CSV con las cartas")]
    public TextAsset csvFile;   // ← ahora arrastras aquí tu CSV en el Inspector
    public List<Card> deck = new List<Card>();
    public List<Card> remain = new List<Card>();
    private int totalCards;
    
    void Start()
    {
        if (csvFile != null)
        {
            LoadCSV(csvFile);
        }
        else
        {
            Debug.LogError("No se asignó un archivo CSV en el inspector.");
        }
        remain = new List<Card>(deck);
        remain.RemoveAt(remain.Count - 1);
        remain.Shuffle();
    }
    public int getTotalCards()
    {
        return totalCards = deck.Count;
    }
    void LoadCSV(TextAsset file)
    {
        string[] rows = file.text.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue;

            string[] cols = rows[i].Split(',');

            Card card = new Card();
            card.cardID = cols[0].Trim();   // ← ya no se hace Parse a int
            card.suit = cols[1];
            card.number = int.Parse(cols[2]);
            card.texturePath = cols[3].Trim();
            deck.Add(card);
        }
    }

    public void RemoveFromRemain(int quantity)
    {
        for (int i = 0; i < quantity && remain.Count > 0; i++)
        {
            remain.RemoveAt(0);
        }
    }

    private void Update() {
        if (remain.Count == 0)
        {
            remain = new List<Card>(deck);
            remain.Shuffle();
        }
    }

    public Card defaultCard()
    {
        return deck[totalCards - 1];
    }
}