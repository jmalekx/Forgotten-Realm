using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Slider SensitivitySlider; 
    public Slider QualitySlider;
    public Slider VolumeSlider;
    public static float ChosenHealthDecreaseSpeed = 0.05f;
    public static float ChosensSprintDecreaseSpeed = 1f;
    public static float ChosensSprintRegenSpeed = 1f;
    public static int ChosenenemyAttackAmount = 10;
    public static float ChosenSensitivity = 50;
    public static int ChosenQuality = 4;

    void Start()
    {
        AudioListener.volume = VolumeSlider.value;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GAME");
    }
    public void easy(){
        ChosenHealthDecreaseSpeed = 0.05f;
        ChosenenemyAttackAmount = 5;
        //enemyAmount
        //brighter night
        ChosensSprintDecreaseSpeed = 0.5f;
        ChosensSprintRegenSpeed = 1.3f;
    }
    public void medium(){
        ChosenHealthDecreaseSpeed = 0.1f; 
        ChosenenemyAttackAmount = 10;
        //enemyAmount
        //normal night
        ChosensSprintDecreaseSpeed = 1f;
        ChosensSprintRegenSpeed = 1f;

    }
    public void hard(){
        ChosenHealthDecreaseSpeed = 0.5f;
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
        AudioListener.volume = VolumeSlider.value;
    }
    public void setSensitivity(){
        ChosenSensitivity = SensitivitySlider.value;
    }
    public void setNightBrightness(){

    }
    public void setQuality(){
        ChosenQuality = Mathf.RoundToInt(QualitySlider.value);
        QualitySettings.SetQualityLevel(ChosenQuality);
    }

}
