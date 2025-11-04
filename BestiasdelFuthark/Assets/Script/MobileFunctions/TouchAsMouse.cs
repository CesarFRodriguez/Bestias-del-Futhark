using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputNew : MonoBehaviour
{
    void Update()
    {
        Vector2 pos = Vector2.zero;
        bool pressed = false;

        // 🖱 Mouse en PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            pressed = true;
            pos = Mouse.current.position.ReadValue();
        }

        // 📱 Touch en móvil
        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    pressed = true;
                    pos = touch.position.ReadValue();
                }
            }
        }

        // 🔍 Hacer raycast solo si hubo click/touch
        if (pressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
