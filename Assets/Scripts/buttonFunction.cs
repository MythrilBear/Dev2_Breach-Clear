using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunction : MonoBehaviour
{
    [SerializeField] public GameObject startGame;
    [SerializeField] public GameObject menuOptions;
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
}