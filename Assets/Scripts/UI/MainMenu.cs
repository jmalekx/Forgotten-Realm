using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Slider SensitivitySlider; 
    public static float ChosenHealthDecreaseSpeed = 1f;
    public static float ChosensSprintDecreaseSpeed = 1f;
    public static float ChosensSprintRegenSpeed = 1f;
    public static int ChosenenemyAttackAmount = 10;
    public static float ChosenSensitivity = 50;

    public void StartGame()
    {
        SceneManager.LoadScene("GAME");
    }
    public void easy(){
        ChosenHealthDecreaseSpeed = 0.5f;
        ChosenenemyAttackAmount = 5;
        //enemyAmount
        //brighter night
        ChosensSprintDecreaseSpeed = 0.5f;
        ChosensSprintRegenSpeed = 1.3f;
    }
    public void medium(){
        ChosenHealthDecreaseSpeed = 1f; 
        ChosenenemyAttackAmount = 10;
        //enemyAmount
        //normal night
        ChosensSprintDecreaseSpeed = 1f;
        ChosensSprintRegenSpeed = 1f;

    }
    public void hard(){
        ChosenHealthDecreaseSpeed = 1.5f;
        ChosenenemyAttackAmount = 15;
        //enemyAmount
        //darker night
        ChosensSprintDecreaseSpeed = 1.5f;
        ChosensSprintRegenSpeed = 0.7f;
    }

    public void Quit(){
        Application.Quit();
    }
    public void setVolume(){

    }
    public void setSensitivity(){
        ChosenSensitivity = SensitivitySlider.value;
    }
    public void setNightBrightness(){

    }

}
