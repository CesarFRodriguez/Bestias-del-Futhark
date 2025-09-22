using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class PlayerData : MonoBehaviour
{
    public int health = 20;
    public int weapon = 0;
    public List<int> weaponWear = new List<int>();
    public int money = 0; 
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI weaponWearText;
    public Toggle useWeaponToggle;
    private bool useWeapon = false;
    public Button sellWeaponButton;
    void Start()
    {
        sellWeaponButton.onClick.AddListener(SellWeapon);
    }
    void Update()
    {
        sellWeaponButton.interactable = weapon > 0;
    }
    public void ApplyCardEffect(Card card)
    {
        if (card.suit == "H")
        {
            if (health < 20)
            {
                health = Mathf.Min(20, health + card.number);
            }
        }
        else if (card.suit == "S" || card.suit == "C")
        {
            if (health > 0)
            {
                if (useWeaponToggle.isOn)
                {
                    if (weapon > 0)
                    {
                        useWeapon = true;
                    }
                    else
                    {
                        useWeapon = false;
                    }
                }
                else
                {
                    useWeapon = false;
                }
                if (weaponWear.Count > 0 && weaponWear[weaponWear.Count - 1] < card.number) useWeapon = false;
                int damage = 0;
                if (useWeapon)
                {
                    weaponWear.Add(card.number);
                    weaponWearText.text = weaponWear[weaponWear.Count - 1].ToString();
                    damage = Mathf.Max(0, card.number - weapon);
                }
                else
                {
                    damage = card.number;
                }
                health = Mathf.Max(0, health - damage);
                if (health <= 0)
                {
                    if (Object.FindFirstObjectByType<MechanicsManager>() != null)
                        Object.FindFirstObjectByType<MechanicsManager>().GameOver("Player has been defeated.");
                }
            }
        }
        else if (card.suit == "D")
        {
            if (weapon > 0)
            {
                SellWeapon();
            } 
            weapon = card.number;
        }
        healthText.text = health.ToString() + "/20";
        weaponText.text = weapon.ToString();
    }
    public void SellWeapon()
    {
        money += weapon;
        moneyText.text = money.ToString();
        weapon = 0;
        weaponText.text = weapon.ToString();
        weaponWear.Clear();
        weaponWearText.text = "0";
    }
}
