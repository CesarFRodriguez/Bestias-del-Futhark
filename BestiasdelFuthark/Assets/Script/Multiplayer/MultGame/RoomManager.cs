using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public RoundManager rm;
    public RenderManager render;
    public HandManager hand;
    public DeckManagerMult deck;
    public List<GameObject> roomCards = new List<GameObject>();
    private int round = 0;

    private void Update() {
        round = rm.round;
        switch(round)
        {
            case 0:
                prepPhase();
                break;
            case 2:
                defensePhase();
                break;
            default:
                breakPhase();
                break;
        }
    }
    private void prepPhase(){
        
    }
    private void defensePhase(){
        
    }
    private void breakPhase(){
        
    }
}