using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PlayerDataMult : MonoBehaviour
{
    public int health = 20;

    private int weapon = 0;
    private List<int> wear = new List<int>();
    private int money = 0;

    public TextMeshProUGUI healthText, weaponText, moneyText, wearText;
    public GameObject LosePanel;
    public GameObject WinPanel;

    private void Start() {
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
        wear.Add(0);
        wearText.text = wear[wear.Count - 1].ToString();
    }
    public void getDamage(int damage){
        health -= damage;
        if(health <= 0) LosePanel.SetActive(true);
        if(health <= 0) Time.timeScale = 0;
        healthText.text = health.ToString() + "/20";
    }
    public void getHealth(int heal){
        health += heal;
        if(health + heal > 20) health = 20;
        healthText.text = health.ToString() + "/20";
    }
    public void getWear(int damage){
        wear.Add(damage);
        wearText.text = wear[wear.Count - 1].ToString();
    }
    public void removeWear(){
        wear.RemoveAt(wear.Count - 1);
        wearText.text = wear[wear.Count - 1].ToString();
    }
    public int Wear(){
        return wear[wear.Count - 1];
    }
    public int Weapon(){
        return weapon;
    }
    public void spendMoney(int spend){
        money -= spend;
        moneyText.text = money.ToString();
    }
    public void getWeapon(int value){
        weapon = value;
        weaponText.text = weapon.ToString();
    }
    public void sellWeapon(){
        money =+ weapon;
        weapon = 0;
        moneyText.text = money.ToString();
        weaponText.text = weapon.ToString();
    }
    public int getMoney(){
        return money;
    }
    public void WinGame(){
        WinPanel.SetActive(true);
        Time.timeScale = 0;
    }
}