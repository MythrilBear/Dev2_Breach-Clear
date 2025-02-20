using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] TMPro.TMP_Text goalCountText;
    [SerializeField] TMPro.TMP_Text ammoCountText;
    [SerializeField] TMPro.TMP_Text reserveAmmoCountText;

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

            MissionObjectiveManager.instance.missionObjective.SetActive(true);
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

    public void AddMissionObjective(string description) //add mission objective
    {
        Debug.Log("GameManager: AddMissionObjective called with: " + description);
        MissionObjectiveManager.instance.AddObjective(description);
    }

    public void CompleteMissionObjective(string description) //remove completed mission objective
    {
        Debug.Log("GameManager: CompleteMissionObjective called with: " + description);
        MissionObjectiveManager.instance.CompleteObjective(description);

        // Temp win scenario

        updateGameGoal(0);
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
