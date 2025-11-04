using UnityEngine;
public class CardBehaviour : MonoBehaviour
{
    public Card cardData; // Esta es la info de la carta cargada del CSV
    private PlayerData player;
    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerData>(); // Busca al Player en la escena
    }
    void OnMouseDown()
    {
        if (player != null)
        {
            player.ApplyCardEffect(cardData);

            // Feedback visual
            transform.position += new Vector3(0, 0, 1f);
            Renderer rend = GetComponentInChildren<Renderer>();
            if (rend != null) rend.material.color = Color.gray;

            // Bloquea interacción futura
            GetComponent<Collider>().enabled = false;
            this.enabled = false;

            // Informar al MechanicsManager
            MechanicsManager mm = FindAnyObjectByType<MechanicsManager>();
            if (mm != null) mm.OnCardUsed();
        }
    }
}
