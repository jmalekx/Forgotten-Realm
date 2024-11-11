using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    //work out public static float meaning 
    public Slider SensitivitySlider; 
    public static float ChosenHealthDecreaseSpeed = 1f;
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
    }
    public void medium(){
        ChosenHealthDecreaseSpeed = 1f; 
        ChosenenemyAttackAmount = 10;
        //enemyAmount
        //normal night
    }
    public void hard(){
        ChosenHealthDecreaseSpeed = 1.5f;
        ChosenenemyAttackAmount = 15;
        //enemyAmount
        //darker night
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
