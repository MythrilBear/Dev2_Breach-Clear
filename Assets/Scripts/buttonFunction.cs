using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunction : MonoBehaviour
{
    [SerializeField] public GameObject startGame;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] public GameObject menuCredits;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider SFXSlider;


    private void Start()
    {
        LoadOptions();

        // Add listeners to save values when sliders are changed
        musicSlider.onValueChanged.AddListener(delegate { SaveOptions(); });
        SFXSlider.onValueChanged.AddListener(delegate { SaveOptions(); });
    }

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
        SceneManager.LoadScene("LoadoutScene");
        // Make sure the cursor is visible and not locked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1; // Ensure the game is unpaused
        GameManager.instance.stateUnpause();
        GameManager.instance.menuActive.SetActive(false); // Deactivate the main menu

        // Ensure the Loadout UI is displayed
        GameManager.instance.LoadOutUI(GameManager.instance.equipmentLoadout);
    }

    public void OpenOptions()
    {
        startGame.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void CloseOptions()
    {
        startGame.SetActive(true);
        menuOptions.SetActive(false);
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadOptions()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    } 
    public void OpenCredits()
    {
        startGame.SetActive(false);
        menuCredits.SetActive(true);
    }

    public void CloseCredits()
    {
        startGame.SetActive(true);
        menuCredits.SetActive(false);
    }
}