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
        }
        UpdateObjectiveUI();
    }
    public void CompleteObjective(int index)
    {
        //check if has objective scroll before compelting objectives
        if (!Inventory.Instance.ScrollObtained)
        {
            return;
        }

        if (index >= 0 && index < objectives.Length && !objectives[index].isComplete)
        {
            objectives[index].isComplete = true;
            UpdateObjectiveUI();

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
    public void UpdateObjectiveUI() //track and update scroll
    {
        foreach (Objective obj in objectives)
        {
            if (obj.isComplete)
            {
                obj.objectiveText.color = Color.white;
                obj.objectiveText.fontStyle = FontStyles.Strikethrough;
            }
            else
            {
                obj.objectiveText.color = new Color32(0x1D, 0x1C, 0x73, 0xFF);
                obj.objectiveText.fontStyle = FontStyles.Normal;
            }
        }
    }
}
