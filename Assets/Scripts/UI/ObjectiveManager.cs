using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;
    [System.Serializable]
    public class Objective
    {
        public string description;
        public bool isComplete;
        public TMP_Text objectiveText;
    }

    public Objective[] objectives; //array for objectives
    public PopupManager popupManager; //ref popupmanager

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); //ensure only one isnatnce of script
    }

    void Start()
    {
        foreach (Objective obj in objectives)
        {
            obj.isComplete = false;
            obj.objectiveText.color = new Color32(0x1D, 0x1C, 0x73, 0xFF);
            obj.objectiveText.fontStyle = FontStyles.Normal;
        }
    }
    public void CompleteObjective(int index)
    {
        if (index >= 0 && index < objectives.Length && !objectives[index].isComplete)
        {
            objectives[index].isComplete = true;
            objectives[index].objectiveText.color = Color.white;
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
