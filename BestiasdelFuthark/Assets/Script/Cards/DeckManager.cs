using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [Header("Archivo CSV con las cartas")]
    public TextAsset csvFile;   // ← ahora arrastras aquí tu CSV en el Inspector
    public List<Card> deck = new List<Card>();
    public List<Card> remain = new List<Card>();
    public List<Card> inTable = new List<Card>();
    public TextMeshProUGUI remainText;
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
        remain.Shuffle();
        for (int i = 0; i < 4 && remain.Count > 0; i++)
        {
            inTable.Add(remain[0]);
            remain.RemoveAt(0);
        }
        UpdateRemain();
        remainText.text = remain.Count + "/" + deck.Count;
        CardManager cm = Object.FindFirstObjectByType<CardManager>();
        if (cm != null)
        {
            cm.DealFirstCards(inTable);
        }
        
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
    public void UpdateRemain()
    {
        if (remainText != null)
        {
            remainText.text = remain.Count + "/" + deck.Count;
        }
    }

}
public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}