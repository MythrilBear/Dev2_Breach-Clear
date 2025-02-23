using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionObjectiveManager : MonoBehaviour
{
    public static MissionObjectiveManager instance;
    public GameObject missionObjectiveText;
    public GameObject missionObjective;

    private List<string> objectives = new List<string>();

    void Awake()
    {
        instance = this;
    }

    public void AddObjective(string objective)
    {
        objectives.Add(objective);
        UpdateMissionObjectivesUI();
    }

    public void CompleteObjective(string objective)
    {

        objectives.Remove(objective);
        UpdateMissionObjectivesUI();

        string currentScene = SceneManager.GetActiveScene().name;

        if (objectives.Count == 0 && currentScene == "Level1")
        {
            missionObjectiveText.GetComponent<TMP_Text>().text = "Mission Objectives:\n";
            missionObjectiveText.GetComponent<TMP_Text>().text += "All objectives complete!\n";
            missionObjective.SetActive(true);
            SceneManager.LoadScene("LoadOutScene 2");
        }
        else if (objectives.Count == 0 && currentScene == "Level3")
        {
            missionObjectiveText.GetComponent<TMP_Text>().text = "Mission Objectives:\n";
            missionObjectiveText.GetComponent<TMP_Text>().text += "All objectives complete!\n";
            missionObjective.SetActive(true);
            SceneManager.LoadScene("LoadOutScene 1");
        }
        else if (objectives.Count == 0 && currentScene == "Level2")
        {
            missionObjectiveText.GetComponent<TMP_Text>().text = "Mission Objectives:\n";
            missionObjectiveText.GetComponent<TMP_Text>().text += "All objectives complete!\n";
            missionObjective.SetActive(true);
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void UpdateMissionObjectivesUI()
    {
        missionObjectiveText.GetComponent<TMP_Text>().text = "Mission Objectives:\n";
        foreach (string objective in objectives)
        {
            missionObjectiveText.GetComponent<TMP_Text>().text += "- " + objective + "\n";
        }
    }
}
