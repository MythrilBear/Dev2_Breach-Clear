using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CurrentEquipment PlayerEquip;
    [SerializeField] public GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject timeOver;
    [SerializeField] GameObject Selection1, Selection2, Selection3;
    [SerializeField] TMP_Text goalCountText;
    [SerializeField] TMP_Text ammoCountText;
    [SerializeField] TMP_Text reserveAmmoCountText;

    [Range(0, 2)] [SerializeField] public int equipmentLoadout;

    public Image PlayerHPBar;
    public Image PlayerStamBar;
    public GameObject damagePanel;
    public GameObject buttonInteract;

    public GameObject player;
    public playerController playerScript;

    public bool isPaused;

    int goalCount;
    public float ammoCount;
    public float reserveAmmoCount;
    internal float volume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene().name != "LoadOutScene")
        {
            player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<playerController>();
            PlayerEquip = player.GetComponent<CurrentEquipment>();
        }
    }
    public void SelectLoadout(int Select)
    {
        equipmentLoadout = Select;
        LoadOutUI(Select);
      

    }

    public void LoadOutUI(int SelectLoadout)
    {
        GameObject[] loadouts = { Selection1, Selection2, Selection3 };

        for (int i = 0; i < loadouts.Length; i++)
        {
            CanvasGroup canvasGroup = loadouts[i].GetComponent<CanvasGroup>();
            if (i == SelectLoadout)
            {
                loadouts[i].GetComponent<CanvasGroup>().alpha = 1f;
                ToggleLoadOutLabel(loadouts[i], true);
            }
            else
            {
                loadouts[i].GetComponent<CanvasGroup>().alpha = 0.5f;
                ToggleLoadOutLabel(loadouts[i], false);
            }
        }
    }

    private void ToggleLoadOutLabel(GameObject loadout, bool isActive)
    {
        for (int i = 0; i < loadout.transform.childCount; i++)
        {
            GameObject child = loadout.transform.GetChild(i).gameObject;

            if (child.name == "Text (TMP)")
            {
                continue;
            }

            child.SetActive(isActive);
        }
    }
    public void ConfirmSelection()
    {
        PlayerPrefs.SetInt("SelectedLoadout", equipmentLoadout);
        PlayerPrefs.Save();
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(menuActive == null)
            { 
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if(menuActive == menuPause)
            {
                stateUnpause();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateLoadoutButtons(Input.GetKeyDown(KeyCode.DownArrow));
        }
        if (Input.GetKeyDown(KeyCode.Return) && equipmentLoadout == 3) // Only triggers if Confirm is selected
        {
            ConfirmSelection();
        }
    }
    private void NavigateLoadoutButtons(bool moveDown)
    {
        if (equipmentLoadout < 0 || equipmentLoadout > 3) // Allow selection of Confirm (3)
            return;

        if (moveDown)
        {
            if (equipmentLoadout < 3)  // Move down, including Confirm
            {
                equipmentLoadout++;
            }
            else
            {
                equipmentLoadout = 0;  // Loop back to Loadout 1
            }
        }
        else
        {
            if (equipmentLoadout > 0)  // Move up, including Confirm
            {
                equipmentLoadout--;
            }
            else
            {
                equipmentLoadout = 3;  // Loop back to Confirm
            }
        }

        if (equipmentLoadout == 3)
        {
            GameObject confirmButton = GameObject.Find("Confirm/Exit"); // Find Confirm button
            if (confirmButton != null)
            {
                EventSystem.current.SetSelectedGameObject(confirmButton);
            }
        }
        else
        {
            SelectLoadout(equipmentLoadout);
        }

    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause() 
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        goalCount += amount;
        goalCountText.text = goalCount.ToString("F0");

        // you win
        if (goalCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void updateAmmoCount(float amount)
    {
        ammoCount = amount;
        ammoCountText.text = ammoCount.ToString("F0");
    }

    public void updateReserveAmmoCount(float amount)
    {
        reserveAmmoCount = amount;
        reserveAmmoCountText.text = reserveAmmoCount.ToString("F0");
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void timeIsUp()
    {
        statePause();
        menuActive = timeOver;
        menuActive.SetActive(true);
    }
}
