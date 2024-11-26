using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class Objective
    {
        public string description;
        public bool isComplete;
        public TMP_Text objectiveText;
    }

    public Objective[] objectives; //array for objectives
    public PopupManager popupManager; //ref popupmanager

    void Start()
    {
        foreach (Objective obj in objectives)
        {
            obj.isComplete = false;
            obj.objectiveText.fontStyle = FontStyles.Normal;
        }
    }
    public void CompleteObjective(int index)
    {
        if (index >= 0 && index < objectives.Length && !objectives[index].isComplete)
        {
            objectives[index].isComplete = true;
            objectives[index].objectiveText.fontStyle = FontStyles.Strikethrough; //crossed out text

            //display popup
            popupManager.ShowPopup($"Objective Completed: {objectives[index].description}");
        }
    }
    public void TrackObjective(string description)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (objectives[i].description == description && !objectives[i].isComplete)
            {
                CompleteObjective(i);
                break;
            }
        }
    }
}
