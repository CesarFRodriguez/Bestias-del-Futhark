using UnityEngine;

public class CardBehaviourMult : MonoBehaviour
{
    public bool isSelected = false;
    public bool canBeUsed = true;
    public bool isUsed = false;

    public float hoverDistance = 2f;

    private Vector3 startPos;
    private Color originalColor;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    void OnMouseDown()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        isSelected = !isSelected;
        if (isSelected)
        {
            if (rend != null)
                rend.material.color *= new Color(1.25f, 1.25f, 1f);
        }
        else
        {
            if (rend != null)
                rend.material.color *= new Color(0.8f, 0.8f, 1f);
        }
    }

    private void Update()
    {
        if (isUsed)
        {
            transform.position += new Vector3(0, 0, 1f);
            Renderer rend = GetComponentInChildren<Renderer>();

            if (rend != null)
                rend.material.color = Color.gray;

            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
        }
        if (isSelected)
        {
            transform.localPosition = startPos + new Vector3(0, hoverDistance, -0.5f);
        }
        else
        {
            transform.localPosition = startPos;
        }
    }
}
