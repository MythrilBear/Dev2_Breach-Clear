using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunction : MonoBehaviour
{
    [SerializeField] public GameObject startGame;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] public GameObject menuCredits;
    [SerializeField] public GameObject menuControls;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider SFXSlider;
    [SerializeField] private BackgroundMusic backgroundMusic;
    [SerializeField] private SoundEffectsManager soundEffectsManager;

    private GameObject previousMenu;

    private void Start()
    {
        LoadOptions();

        // Add listeners to save values when sliders are changed
        musicSlider.onValueChanged.AddListener(delegate { SaveOptions(); });
        musicSlider.onValueChanged.AddListener(delegate { backgroundMusic.SetMusicVolume(musicSlider.value); });
        SFXSlider.onValueChanged.AddListener(delegate { SaveOptions(); });
        SFXSlider.onValueChanged.AddListener(delegate { soundEffectsManager.SetSFXVolume(SFXSlider.value); });
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

    public void StartMenu()
    {
        SceneManager.LoadScene("Start Menu");
        // Make sure the cursor is visible and not locked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1; // Ensure the game is unpaused
        GameManager.instance.menuActive.SetActive(false); // Deactivate the main menu
    }

    public void StartGame()
    {
        GameManager.instance.menuActive = startGame;

        SceneManager.LoadScene("LoadoutScene");
        // Make sure the cursor is visible and not locked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1; // Ensure the game is unpaused
        GameManager.instance.menuActive.SetActive(false); // Deactivate the main menu

        // Ensure the Loadout UI is displayed
        GameManager.instance.LoadOutUI(GameManager.instance.equipmentLoadout);
    }

   
    public void OpenOptionsFromStartMenu()
    {
        OpenOptions(startGame);
    }

    public void OpenOptionsFromPauseMenu()
    {
        OpenOptions(GameManager.instance.menuPause);
        GameObject missionObjective = GameObject.Find("Mission Objective");
        if (missionObjective != null)
        {
            missionObjective.SetActive(false);
        }
    }

    public void OpenOptions(GameObject currentMenu)
    {
        previousMenu = currentMenu;
        startGame.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void CloseOptions()
    {
        GameObject missionObjective = GameObject.Find("Mission Objective");
        previousMenu.SetActive(true);
        menuOptions.SetActive(false);

        if(missionObjective != null)
        {
            missionObjective.SetActive(true);
        }
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
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("SFXVolume"))
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
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

    public void OpenControls()
    {
        menuControls.SetActive(true);
        GameManager.instance.menuActive = menuControls;     
    }

    public void CloseControls()
    {
        previousMenu.SetActive(true);
        menuControls.SetActive(false);
    }
}