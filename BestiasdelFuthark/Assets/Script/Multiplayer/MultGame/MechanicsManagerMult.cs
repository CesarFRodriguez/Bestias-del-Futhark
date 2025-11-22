using UnityEngine;
using UnityEngine.UI;

public class MechanicsManagerMult : MonoBehaviour
{
    public PlayerDataMult player;
    public PlayerManager interaction;
    private int price = 5;

    public Button removeWear, sellWeapon;

    private void Start(){

        removeWear.onClick.AddListener(removeWearInteraction);
        sellWeapon.onClick.AddListener(sellWeaponInteraction);
        
    }

    private void Update() {
        if(player.getMoney() < price){
            removeWear.interactable = false;
        }else{
            removeWear.interactable = true;
        }
    }





    public void removeWearInteraction(){
        if(player.Wear() == 0) return;
        player.removeWear();
    }

    public void sellWeaponInteraction(){
        player.sellWeapon();
    }
}