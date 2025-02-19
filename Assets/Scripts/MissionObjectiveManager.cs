using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
