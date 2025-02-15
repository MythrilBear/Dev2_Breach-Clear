using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunction : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.stateUnpause();
        SceneManager.LoadScene("Level1");
    }

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
}