using UnityEngine;
using TMPro;

public class DiceRoll : MonoBehaviour
{
    public PlayerData playerData; // Referencia al objeto Player Data
    public int diceCost;     // Costo total del lanzamiento del dado
    public TMP_Text resultText;   // Texto donde se mostrará el resultado

    public void RollDice()
    {
        string message = "";

        // Verificar si hay suficiente dinero para lanzar el dado
        if (!playerData.SpendMoney(diceCost))
        {
            message = "No tienes suficiente dinero para lanzar el dado.";
            if (resultText != null)
                resultText.text = message;
            else
                Debug.Log(message);
            return;
        }

        // Generar un número aleatorio entre 0 y 2 (3 opciones)
        int result = Random.Range(0, 3);

        switch (result)
        {
            case 0: // Fehu
                playerData.ActivateFehu(5, resultText);
                message = "Fehu: Mayor ganancia por armas vendidas durante 5 turnos (+2 por venta)";
                break;

            case 1: // Ehwaz
                playerData.potionBonus = true;
                message = "Ehwaz: Mayor probabilidad de pociones y salud";
                break;

            case 2: // Odin
                int chance = Random.Range(0, 2); // 0 o 1
                if (chance == 0)
                {
                    playerData.health += 10;
                    message = "Odin: ¡Ganaste 10 de vida!";
                }
                else
                {
                    playerData.health -= 5;
                    message = "Odin: ¡Perdiste 5 de vida!";
                }

                // Actualizar texto de salud
                playerData.healthText.text = playerData.health.ToString() + "/20";
                break;
        }

        // Mostrar resultado en pantalla
        if (resultText != null)
            resultText.text = message;
        else
            Debug.Log(message);
    }
}
