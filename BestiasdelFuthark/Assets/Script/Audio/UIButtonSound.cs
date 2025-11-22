using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public enum ButtonSoundType
    {
        General,      // Botones
        Comodin,      // Comodin
        SellWeapon,   // SellWeapon
        Skip,         // Skip
        RemoveWear,   // RemoveWear
        UseWeapon     // UseWeapon
    }

    public ButtonSoundType soundType;

    public void PlaySound()
    {
        switch (soundType)
        {
            case ButtonSoundType.General:
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Botones");
                break;

            case ButtonSoundType.Comodin:
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Comodin");
                break;

            case ButtonSoundType.SellWeapon:
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/SellWeapon");
                break;

            case ButtonSoundType.Skip:
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Skip");
                break;

            case ButtonSoundType.RemoveWear:
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/RemoveWear");
                break;

            case ButtonSoundType.UseWeapon:
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UseWeapon");
                break;
        }
    }
}
