using UnityEngine;

public class CardBehaviourMult : MonoBehaviour
{
    public bool isSelected = false;
    public bool isUsed = false;
    public bool isFree = true;
    public float hoverDistance = 2f;

    private Color OriginalColor;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    void OnMouseDown()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (isFree || isSelected)
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                if (rend != null)
                    rend.material.color *= new Color(1.75f, 1.75f, 1f);
            }
            else
            {
                if (rend != null)
                    rend.material.color *= new Color(4f / 7f, 4f / 7f, 1f);
            }
        }
    }

    private void Update()
    {
        DisableManager();
        SelectedManager();
    }

    private void DisableManager()
    {
        if (!isUsed)
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            if(rend.material.color == Color.gray)
            {
                rend.material.color = OriginalColor;
            }
            else
            {
                OriginalColor = rend.material.color;
            }
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            if(isSelected)
            {
                rend.material.color *= new Color(4f / 7f, 4f / 7f, 1f);
            }
            isSelected = false;
            transform.position = startPos + new Vector3(0, 0, 1f);
            rend.material.color = Color.gray;
            GetComponent<Collider>().enabled = false;
        }
    }

    private void SelectedManager()
    {
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
