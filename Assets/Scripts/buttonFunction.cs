using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunction : MonoBehaviour
{
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
}