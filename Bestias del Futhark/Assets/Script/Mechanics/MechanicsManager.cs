using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MechanicsManager : MonoBehaviour
{
    public GameObject panelGameplay;
    public GameObject panelGameOver;
    public Button skipButton;
    public Button removeWear;
    public Button restartButton;
    public DeckManager deckManager;
    public CardManager cardManager;
    public PlayerData playerData;
    public TextMeshProUGUI removeWearPriceText;
    public TextMeshProUGUI gameOverText;
    private int removeWearPrice = 5;
    private bool skipUsedThisTurn = false;        // Skip usado en sala completa
    private bool skipBlockedByCardUsage = false;  // Skip bloqueado por uso de carta en sala parcial

    void Start()
    {
        if (skipButton != null)
            skipButton.onClick.AddListener(OnSkipPressed);
        if (removeWear != null)
            removeWear.onClick.AddListener(OnRemoveWearPressed);
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        UpdateSkipButton();
        if (removeWearPriceText != null)
            removeWearPriceText.text = removeWearPrice.ToString();
        removeWear.interactable = false;
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelGameplay != null) panelGameplay.SetActive(true);
    }
    void Update()
    {
        if (playerData == null || removeWear == null) return;
        if (playerData.money >= removeWearPrice)
            removeWear.interactable = true;
        else
            removeWear.interactable = false;
    }
    public void OnRemoveWearPressed()
    {
        if (playerData == null) return;
        if (playerData.money < removeWearPrice)
        {
            removeWear.interactable = false;
            return;
        }
        if (playerData.weaponWear.Count > 0)
        {
            playerData.money -= removeWearPrice;
            playerData.moneyText.text = playerData.money.ToString();
            removeWearPrice += 5;
            removeWearPriceText.text = removeWearPrice.ToString();
            playerData.weaponWear.RemoveAt(playerData.weaponWear.Count - 1);
            if (playerData.weaponWear.Count > 0)
                playerData.weaponWearText.text = playerData.weaponWear[playerData.weaponWear.Count - 1].ToString();
            else
                playerData.weaponWearText.text = "0";
        }
    }
    void UpdateSkipButton()
    {
        if (skipButton == null || deckManager == null || cardManager == null) return;

        int activeCards = 0;
        foreach (var cardObj in cardManager.cards)
            if (cardObj.GetComponent<Collider>().enabled) activeCards++;

        // Siempre habilitar skip si queda solo 1 carta activa
        if (activeCards <= 1)
        {
            skipButton.interactable = true;
            return;
        }

        // Bloqueos normales
        if (skipUsedThisTurn || (skipBlockedByCardUsage && activeCards > 1))
        {
            skipButton.interactable = false;
            return;
        }

        skipButton.interactable = true;
    }
    public void OnSkipPressed()
    {
        if (deckManager == null || cardManager == null) return;

        int activeCards = 0;
        foreach (var cardObj in cardManager.cards)
            if (cardObj.GetComponent<Collider>().enabled) activeCards++;

        // Sala completa → bloquear skip
        if (activeCards == 4)
            skipUsedThisTurn = true;
        if (activeCards == 1)
            skipUsedThisTurn = false;

        // Recorremos la mesa
        for (int i = 0; i < cardManager.cards.Count; i++)
        {
            GameObject cardObj = cardManager.cards[i];
            bool isActive = cardObj.GetComponent<Collider>().enabled;

            if (activeCards == 4)
            {
                // Guardar carta en remain y marcar mesa vacía
                if (deckManager.inTable[i] != null)
                {
                    deckManager.remain.Add(deckManager.inTable[i]);
                    deckManager.inTable[i] = null; // ⚡ marcamos vacío para reemplazarla
                }
            }
            else
            {
                // Sala parcial: solo las cartas inactivas se eliminan
                if (!isActive)
                    deckManager.inTable[i] = null; // se eliminan
            }
        }

        // Tomamos nuevas cartas para rellenar la mesa
        for (int i = 0; i < cardManager.cards.Count; i++)
        {
            GameObject cardObj = cardManager.cards[i];

            // Si hay carta activa en la mesa, la dejamos
            if (deckManager.inTable[i] != null)
                continue;

            if (deckManager.remain.Count == 0) break;

            Card nextCard = deckManager.remain[0];
            deckManager.remain.RemoveAt(0);
            deckManager.inTable[i] = nextCard;

            // Reset visual e interacción
            Renderer rend = cardObj.GetComponentInChildren<Renderer>();
            if (rend != null) rend.material.color = Color.white;

            Collider col = cardObj.GetComponent<Collider>();
            if (col != null) col.enabled = true;
            CardBehaviour cb = cardObj.GetComponent<CardBehaviour>();
            if (cb != null) cb.enabled = true;

            cardManager.ApplyCardTexture(nextCard, cardObj);
            cardManager.ApplyCardBehaviour(nextCard, cardObj);
        }

        if (activeCards < 4)
            skipBlockedByCardUsage = false;

        deckManager.UpdateRemain();
        UpdateSkipButton();
    }

    private void ReplaceCard(int index)
    {
        if (deckManager.remain.Count == 0) return;

        GameObject cardObj = cardManager.cards[index];

        // Guardar la carta actual en el mazo si existe
        Card currentCard = deckManager.inTable[index];
        if (currentCard != null)
            deckManager.remain.Add(currentCard);  // La carta reemplazada vuelve al final

        // Tomar siguiente carta del mazo
        Card nextCard = deckManager.remain[0];
        deckManager.remain.RemoveAt(0);
        deckManager.inTable[index] = nextCard;

        // Reset visual
        Renderer rend = cardObj.GetComponentInChildren<Renderer>();
        if (rend != null) rend.material.color = Color.white;

        // Reset interacción
        Collider col = cardObj.GetComponent<Collider>();
        if (col != null) col.enabled = true;
        CardBehaviour cb = cardObj.GetComponent<CardBehaviour>();
        if (cb != null) cb.enabled = true;

        // Asignar nueva carta
        cardManager.ApplyCardTexture(nextCard, cardObj);
        cardManager.ApplyCardBehaviour(nextCard, cardObj);
    }
    // Llamar desde CardBehaviour al usar una carta
    public void OnCardUsed()
    {
        int activeCards = 0;
        foreach (var cardObj in cardManager.cards)
            if (cardObj.GetComponent<Collider>().enabled) activeCards++;

        // Bloquea skip si se usó carta y quedan >1 cartas
        if (activeCards > 1)
            skipBlockedByCardUsage = true;

        // Si todas las cartas se usaron → avanzar sala y reset flags
        if (activeCards == 0)
        {
            skipBlockedByCardUsage = false;
            skipUsedThisTurn = false;
            AdvanceRoom();
        }

        UpdateSkipButton();
    }
    private void AdvanceRoom()
    {
        if (deckManager == null || cardManager == null) return;
        if (deckManager.remain.Count == 0)
        {
            GameOver("You won! No more cards in the deck.");
        }
        for (int i = 0; i < cardManager.cards.Count; i++)
        {
            GameObject cardObj = cardManager.cards[i];

            if (deckManager.remain.Count == 0) continue;
            Card nextCard = deckManager.remain[0];
            deckManager.remain.RemoveAt(0);
            deckManager.inTable[i] = nextCard;

            // Reset visual e interacción
            Renderer rend = cardObj.GetComponentInChildren<Renderer>();
            if (rend != null) rend.material.color = Color.white;

            Collider col = cardObj.GetComponent<Collider>();
            if (col != null) col.enabled = true;
            CardBehaviour cb = cardObj.GetComponent<CardBehaviour>();
            if (cb != null) cb.enabled = true;

            cardManager.ApplyCardTexture(nextCard, cardObj);
            cardManager.ApplyCardBehaviour(nextCard, cardObj);
        }

        // Resetea flags **después** de que las cartas estén activas
        skipBlockedByCardUsage = false;
        skipUsedThisTurn = false;

        // Ahora sí actualizar botón
        UpdateSkipButton();

        deckManager.UpdateRemain();
    }
    public void GameOver(string reason)
    {
        if (panelGameplay != null) panelGameplay.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(true);
        gameOverText.text = reason;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
