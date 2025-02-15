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
        PlayerPrefs.SetInt("SelectedLoadout", 0); // Reset to Loadout 1
        PlayerPrefs.Save(); // Ensure the reset is stored

        Time.timeScale = 1; // Ensure time is running when restarting
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("LoadoutScene");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.instance.stateUnpause();
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