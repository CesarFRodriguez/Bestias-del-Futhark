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

<<<<<<< HEAD
=======
    public bool ready = false;
    public bool enemyReady = false;
    public GameNetworkListener netListener;
>>>>>>> 46daeb54dbc27ab5e5254737c173cf68c639b5ee
    private void Start() {
        nextRound.onClick.AddListener(NextRound);
    }
    private void Update() {
<<<<<<< HEAD
        nextRound.interactable = isReady;

=======
        if (round != 0) nextRound.interactable = false; else nextRound.interactable = true;
        if (round == 0) ReadyChecker();
>>>>>>> 46daeb54dbc27ab5e5254737c173cf68c639b5ee
        timeLeft += Time.deltaTime;
        netListener.enemyReady = enemyReady;
        float remainingTime = actualTime(round);
        toTime(timeLeft);
        if(timeLeft > remainingTime){
            round++;
            timeLeft = 0;
        }
        if(round > 3) round = 0;
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
                roundInfo.text = "Protect Your -----.";
                actTime = timeDef+ 1f;
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
        int seconds = (int)remainTime;

        while (seconds >= 60) seconds -= 60;
        
        string secondsStr = seconds.ToString();

        char[] arr = {'0', '0'};
        for(int i = 0; i < secondsStr.Length; i++){
            arr[secondsStr.Length - 1 - i] = secondsStr[i];
        }
        string timeStr = minutes.ToString() + ':' + arr[1] + arr[0];
        timeText.text = timeStr;
    }

    public void NextRound(){
<<<<<<< HEAD
        if(round > 3) round = -1;
        round++;
=======
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
        if(ready) nextRound.interactable = false;
        if(ready && enemyReady)
        {
            PassRound();
        }
>>>>>>> 46daeb54dbc27ab5e5254737c173cf68c639b5ee
    }
}
