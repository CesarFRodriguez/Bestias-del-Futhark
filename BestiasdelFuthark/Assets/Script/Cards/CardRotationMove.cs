using UnityEngine;

public class CardRotationMove : MonoBehaviour
{
    public float amplitude = 1f; // grados en X
    public float speed = 3f; // velocidad de oscilación en X
    private Vector3 initialRotation;
    void Start()
    {
        // Guardamos la rotación original de la carta
        initialRotation = transform.eulerAngles;
    }
    void Update()
    {
        // Calculamos los offsets usando seno
        float offsetX = Mathf.Sin(Time.time * speed) * amplitude;
        float offsetY = Mathf.Cos(Time.time * speed) * amplitude;

        // Aplicamos la rotación relativa a la original
        transform.eulerAngles = new Vector3(
            initialRotation.x + offsetX,
            initialRotation.y + offsetY,
            initialRotation.z
        );
    }
}
