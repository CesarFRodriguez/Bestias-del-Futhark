using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerDataMult player;
    public void getCard(Card card){
        if (card.suit == "S" || card.suit == "C"){
            int damage = 0;
            if(player.isWeapon){
                if(player.Wear() >= card.number || player.Wear() == 0){
                    damage = card.number - player.Weapon();
                    if(damage < 0){
                        damage = 0;
                    }
                    player.getWear(card.number);
                }else{
                    damage = card.number;
                }
            }else{
                damage = card.number;
            }
            player.getDamage(damage);
        }
        if (card.suit == "H" ){
            player.getHealth(card.number);
        }
        if (card.suit == "D"){
            if(player.isWeapon){
                player.sellWeapon();
            }
            player.getWeapon(card.number);
        }
    }
}
