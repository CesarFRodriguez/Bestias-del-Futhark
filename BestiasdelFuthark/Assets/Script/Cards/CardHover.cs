using UnityEngine;

public class CardHover : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isHovered = false;

    [Header("Movimiento al pasar el mouse")]
    public float hoverDistance = 0.5f;  // qué tanto se acerca a la cámara
    public float moveSpeed = 5f;        // velocidad de interpolación

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseEnter()
    {
        isHovered = true;
    }

    void OnMouseExit()
    {
        isHovered = false;
    }

    void Update()
    {
        if (isHovered)
        {
            // Calcula una posición entre la carta y la cámara
            Vector3 target = originalPosition + Camera.main.transform.forward * hoverDistance;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
        }
        else
        {
            // Regresa a la posición original
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
        }
    }
}

