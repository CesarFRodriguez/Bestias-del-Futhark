using UnityEngine;
using TMPro; // Importante para usar TextMeshPro

public class DiceRoll : MonoBehaviour
{
    public PlayerData playerData; // Referencia al objeto Player Data
    public int diceCost = 10;     // Costo por lanzar el dado
    public int comodinCost = 5;   // Costo adicional por activar el comodín
    public TMP_Text resultText;   // Texto donde se mostrará el resultado

    public void RollDice()
    {
        if (playerData.money >= diceCost)
        {
            // Cobro del lanzamiento
            playerData.money -= diceCost;

            int result = Random.Range(0, 3);
            string message = "";

            switch (result)
            {
                case 0: // Fehu
                    if (playerData.money >= comodinCost)
                    {
                        playerData.money -= comodinCost;
                        playerData.weaponBonus = true;
                        message = "Fehu: Mayor ganancia por armas vendidas (se descontó " + comodinCost + ")";
                    }
                    else
                    {
                        message = "No tienes suficiente dinero para activar Fehu.";
                    }
                    break;

                case 1: // Ehwaz
                    if (playerData.money >= comodinCost)
                    {
                        playerData.money -= comodinCost;
                        playerData.potionBonus = true;
                        message = "Ehwaz: Mayor probabilidad de pociones y salud (se descontó " + comodinCost + ")";
                    }
                    else
                    {
                        message = "No tienes suficiente dinero para activar Ehwaz.";
                    }
                    break;

                case 2: // Odin
                    if (playerData.money >= comodinCost)
                    {
                        playerData.money -= comodinCost;
                        int chance = Random.Range(0, 2);
                        if (chance == 0)
                        {
                            playerData.health += 10;
                            message = "Odin: ¡Ganaste 10 de vida! (se descontó " + comodinCost + ")";
                        }
                        else
                        {
                            playerData.health -= 5;
                            message = "Odin: ¡Perdiste 5 de vida! (se descontó " + comodinCost + ")";
                        }
                    }
                    else
                    {
                        message = "No tienes suficiente dinero para activar Odin.";
                    }
                    break;
            }

            if (resultText != null)
                resultText.text = message;
            else
                Debug.Log(message);
        }
        else
        {
            string message = "No tienes suficiente dinero para lanzar el dado.";
            if (resultText != null)
                resultText.text = message;
            else
                Debug.Log(message);
        }
    }
}

