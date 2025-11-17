using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Card Sounds")]
    public AudioClip healthCard;
    public AudioClip commonCard;

    [Header("Mouse")]
    public AudioClip clickSound;

    [Header("Buttons")]
    public AudioClip restoreSound;
    public AudioClip sellSound;
    public AudioClip wildcardSound;
    public AudioClip skipSound;
    public AudioClip toggleWeaponSound;
    public AudioClip GeneralBottom;


    private AudioSource source;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public void PlayRestoreSound() //Resturar
    {
        AudioManager.instance.PlaySound(AudioManager.instance.restoreSound);
    }

    public void PlaySellSound() //Vender
    {
        AudioManager.instance.PlaySound(AudioManager.instance.sellSound);
    }

    public void PlayWildcardSound() //Comodin
    {
        AudioManager.instance.PlaySound(AudioManager.instance.wildcardSound);
    }

    public void PlaySkipSound() //Skip
    {
        AudioManager.instance.PlaySound(AudioManager.instance.skipSound);
    }

    public void PlayToggleWeaponSound()  // Usar/Desusar arma
    {
        AudioManager.instance.PlaySound(AudioManager.instance.toggleWeaponSound);
    }
        public void PlayGeneralBotton() //Boton General
    {
        AudioManager.instance.PlaySound(AudioManager.instance.GeneralBottom);
    }

}

