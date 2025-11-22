using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public int round = 0;
    public float timePrep = 30.0f;
    public float timeDef = 30.0f;
    public float timeRest = 5.0f;
    public float timeLeft = 0f;

    public Button nextRound;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI roundInfo;

    // Multijugador
    public bool ready = false;
    public bool enemyReady = false;
    public GameNetworkListener netListener;

    private void Start() {
        nextRound.onClick.AddListener(NextRound);
    }

    private void Update() 
    {
        // Botón solo disponible en ronda 0
        if (round == 0) nextRound.interactable = !ready;
        else nextRound.interactable = false;

        if (round == 0)
            ReadyChecker();

        timeLeft += Time.deltaTime;

        if (netListener != null)
            netListener.enemyReady = enemyReady;

        float remainingTime = actualTime(round);
        toTime(timeLeft);

        if (timeLeft > remainingTime){
            round++;
            timeLeft = 0;
        }

        if (round > 3) round = 0;
    }

    private float actualTime(int round){
        float actTime = 0;
        switch (round)
        {
            case 0:
                roundInfo.text = "Select your Enemy's dungeon.";
                actTime = timePrep + 1f;
                break;
            case 2:
                roundInfo.text = "Protect Your Base.";
                actTime = timeDef + 1f;
                break;
            default:
                roundInfo.text = "Prepare Yourself...";
                actTime = timeRest + 1f;
                break;
        }
        return actTime;
    }

    private void toTime(float timeLeft){
        float remainTime = (actualTime(round) - timeLeft);

        int minutes = (int)remainTime / 60;
        int seconds = (int)remainTime % 60;

        timeText.text = $"{minutes:0}:{seconds:00}";
    }

    public void NextRound(){
        ready = true;
    }

    public void PassRound(){
        if(round == 0)
        {
            round = 1;
            timeLeft = 0;
            ready = false;
            enemyReady = false;
        }
    }

    private void ReadyChecker()
    {
        if(ready && enemyReady)
        {
            PassRound();
        }
    }
}
