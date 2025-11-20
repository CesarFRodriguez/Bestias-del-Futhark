using UnityEngine;

public class RenderManager : MonoBehaviour
{
    public Card cardInfo;
    public GameObject cardObject;
    public DeckManagerMult deckManager;

    public void RenderCard(Card cardInfo, GameObject cardObject)
    {
        Renderer cardRender = new Renderer();
        Texture2D cardMaterial = setCard(cardInfo);
        Color suitColor = setColor(cardInfo);

        cardRender = cardObject.GetComponentInChildren<Renderer>();
        if (cardRender.material.HasProperty("_BaseColor"))
            cardRender.material.SetColor("_BaseColor", suitColor);
        else
            cardRender.material.color = suitColor; // fallback

        if (cardMaterial != null)
        {
            if (cardRender.material.HasProperty("_BaseMap"))
                cardRender.material.SetTexture("_BaseMap", cardMaterial);
            else
                cardRender.material.mainTexture = cardMaterial; // fallback
        }
    }  

    private Texture2D setCard(Card cardInfo)
    {
        string path = "Cards/" + cardInfo.texturePath;
        Texture2D tex = Resources.Load<Texture2D>(path);
        return tex;
    }

    private Color setColor(Card cardInfo)
    {
        Color suitColor;
        switch (cardInfo.suit)
        {
            case "H": suitColor = new Color(1f, 0.5f, 0.5f); break;
            case "D": suitColor = new Color(1f, 0.7f, 0.5f); break;
            case "C": suitColor = new Color(0.5f, 0.5f, 1f); break;
            case "S": suitColor = new Color(0.5f, 0.7f, 1f); break;
            default: suitColor = Color.gray; break;
        }
        return suitColor;
    }
}