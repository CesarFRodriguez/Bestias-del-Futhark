using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButton : MonoBehaviour
{
    public string menu;
    public void GoMenu()
    {
        SceneManager.LoadScene(menu);
    }
}
