using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    public Button single, mult, tuto, config, exit;
    public Button returnButton, hostGame, joinGame;
    public bool isHost = false;
    public GameObject isHostToggle;
    public NetworkManagerP2P p2p;
    public string[] scenes;
    void Start()
    {
        isHostToggle.SetActive(false);
        single.onClick.AddListener(Single);
        mult.onClick.AddListener(Multi);
        hostGame.onClick.AddListener(HostGame);
        joinGame.onClick.AddListener(JoinGame);
        returnButton.onClick.AddListener(Return);
        tuto.onClick.AddListener(Tuto);
        config.onClick.AddListener(Config);
        exit.onClick.AddListener(Exit);
    }
    void Single(){
        SceneManager.LoadScene(scenes[0]);
    }
    void Multi()
    {
        isHostToggle.SetActive(true);
    }   
    void HostGame()
    {
        p2p.isHost = true;
        SceneManager.LoadScene(scenes[1]);
    }
    void JoinGame()
    {
        p2p.isHost = false;
        SceneManager.LoadScene(scenes[1]);
    }
    void Return()
    {
        isHostToggle.SetActive(false);
    }
    void Tuto()
    {
        SceneManager.LoadScene(scenes[2]);
    }
    void Config()
    {
        SceneManager.LoadScene(scenes[3]);
    }
    void Exit()
    {
        Application.Quit();
    }
}
