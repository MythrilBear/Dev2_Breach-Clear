using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        SetButtonSelected("Start Game Button");
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SetButtonSelected("Start Game Button"); // Change if needed
        }

        // Handle Enter key to trigger the selected button
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                UnityEngine.UI.Button button = selected.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOptions.activeSelf) // If options menu is open
            {
                CloseOptions();
            }
            else if (menuCredits.activeSelf) // If credits menu is open
            {
                CloseCredits();
            }
        }
       
    }

    private void SetButtonSelected(string buttonName)
    {
        GameObject button = GameObject.Find(buttonName);
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(button);
        }
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
        SetButtonSelected("Start Game Button");
        SceneManager.LoadScene("LoadoutScene");
        // Make sure the cursor is visible and not locked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1; // Ensure the game is unpaused
        GameManager.instance.menuActive.SetActive(false); // Deactivate the main menu

        // Ensure the Loadout UI is displayed
        GameManager.instance.LoadOutUI(GameManager.instance.equipmentLoadout);
    }

    public void OpenOptions()
    {
        startGame.SetActive(false);
        menuOptions.SetActive(true);
        SetButtonSelected("Options Button");
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
        SetButtonSelected("Credits Button");
    }

    public void CloseCredits()
    {
        startGame.SetActive(true);
        menuCredits.SetActive(false);
    }
}