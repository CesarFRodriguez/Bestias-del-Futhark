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

    // Bonos activados por el dado 
    public bool weaponBonus = false;
    public bool potionBonus = false;

    // --- Fehu Bonus internals ---
    private int fehuTurnsLeft = 0;
    private string previousResultText = "";
    private TMP_Text diceResultText = null;

    // Propiedad pública para consultar si Fehu está activo
    public bool FehuActive => fehuTurnsLeft > 0;

    void Start()
    {
        if (sellWeaponButton != null)
            sellWeaponButton.onClick.AddListener(SellWeapon);

        // Si quieres, inicializa previousResultText con algún texto por defecto
        // if (diceResultText != null) previousResultText = diceResultText.text;
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
        // +2 si Fehu está activo
        int bonus = (weaponBonus && FehuActive) ? 2 : 0;
        money += weapon + bonus;

        moneyText.text = money.ToString();
        weapon = 0;
        weaponText.text = weapon.ToString();
        weaponWear.Clear();
        weaponWearText.text = "0";
    }

    // Gasta dinero a través de PlayerData (actualiza UI)
    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            if (moneyText != null)
                moneyText.text = money.ToString();
            return true; // Compra exitosa
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para esta acción.");
            return false; // No se pudo comprar
        }
    }

    // Activa Fehu por 'turns' turnos y guarda el texto anterior solo si no estaba activo ya
    public void ActivateFehu(int turns, TMP_Text resultText)
    {
        if (resultText == null)
        {
            Debug.LogWarning("ActivateFehu: resultText es null. No se podrá restaurar el texto.");
        }

        // Si Fehu ya está activo, extendemos turno(s) pero NO sobrescribimos previousResultText
        if (!FehuActive)
        {
            previousResultText = (resultText != null) ? resultText.text : "";
            diceResultText = resultText;
        }

        weaponBonus = true;
        fehuTurnsLeft = Mathf.Max(fehuTurnsLeft, 0) + turns; // si queremos acumular, usamos += turns en vez de esto
        if (resultText != null)
            resultText.text = $"Fehu activo: +2 por venta durante {fehuTurnsLeft} turnos.";

        Debug.Log($"Fehu activado. Turnos restantes: {fehuTurnsLeft}");
    }

    // Reducir 1 turno (llamar desde MechanicsManager cuando avance la sala/turno)
    public void AdvanceTurn()
    {
        if (fehuTurnsLeft > 0)
        {
            fehuTurnsLeft--;
            Debug.Log($"AdvanceTurn llamado. Fehu turns left: {fehuTurnsLeft}");

            // Actualizar texto si aún activo
            if (diceResultText != null && fehuTurnsLeft > 0)
                diceResultText.text = $"Fehu activo: +2 por venta durante {fehuTurnsLeft} turnos.";

            if (fehuTurnsLeft == 0)
            {
                weaponBonus = false;

                // Restaurar texto anterior
                if (diceResultText != null)
                {
                    diceResultText.text = previousResultText;
                }

                // limpiar referencias
                diceResultText = null;
                previousResultText = "";
                Debug.Log("Fehu finalizó y texto restaurado.");
            }
        }
    }
}
