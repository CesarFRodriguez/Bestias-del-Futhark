using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickSound : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.clickSound);
        }
    }
}
