using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunction : MonoBehaviour
{
    [SerializeField] public GameObject startGame;
    [SerializeField] public GameObject menuOptions;

    public void resume()
    {
       GameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }

    public void quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }  

    public void StartGame()
    {  
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1; // Ensure the game is unpaused
        GameManager.instance.stateUnpause();
        GameManager.instance.menuActive.SetActive(false); // Deactivate the main menu
    }

    public void OpenOptions()
    {
        startGame.SetActive(false);
        menuOptions.SetActive(true);
    }
}