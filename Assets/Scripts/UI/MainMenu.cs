using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public Slider SensitivitySlider;
    public Slider QualitySlider;
    public Slider VolumeSlider;
    public Slider LoadingSlider;
    public static float ChosenSunrise = 6f;
    public static float ChosenSunset = 20f; 
    public static float ChosenHealthDecreaseSpeed = 0.05f;
    public static float ChosensSprintDecreaseSpeed = 1f;
    public static float ChosensSprintRegenSpeed = 1f;
    public static int ChosenenemyAttackAmount = 10;
    public static float ChosenSensitivity = 50;
    public static int ChosenQuality = 4;
    public TextMeshProUGUI modeInfo; 

    void Start()
    {
        AudioListener.volume = VolumeSlider.value;
    }
    public void StartGame()
    {   
        StartCoroutine(Loading());
        
        //SceneManager.LoadScene("GAME");
    }
    IEnumerator Loading(){
        AsyncOperation load = SceneManager.LoadSceneAsync("GAME");
        while(!load.isDone){
            float progressing = Mathf.Clamp01(load.progress / 0.9f);
            LoadingSlider.value = progressing;
            yield return null ;
        }
    }
    public void easy(){
        ChosenSunrise = 4f;
        ChosenSunset = 22f; 
        ChosenHealthDecreaseSpeed = 0.05f;
        ChosenenemyAttackAmount = 5;
        ChosensSprintDecreaseSpeed = 0.5f;
        ChosensSprintRegenSpeed = 1.3f;
                // Update modeInfo text
        modeInfo.text = "Difficulty: Easy\n" +
                        "Sunrise: 4 AM\n" +
                        "Sunset: 10 PM\n" +
                        "Enemy Attack Amount: 5\n" +
                        "Health Decrease Speed: 0.05\n" +
                        "Sprint Decrease Speed: 0.5\n" +
                        "Sprint Regen Speed: 1.3";
    }
    
    public void medium(){
        ChosenSunrise = 6f; 
        ChosenSunset = 20f;        
        ChosenHealthDecreaseSpeed = 0.1f; 
        ChosenenemyAttackAmount = 10;
        ChosensSprintDecreaseSpeed = 1f;
        ChosensSprintRegenSpeed = 1f;
        // Update modeInfo text
        modeInfo.text = "Difficulty: Medium\n" +
                        "Sunrise: 6 AM\n" +
                        "Sunset: 8 PM\n" +
                        "Enemy Attack Amount: 10\n" +
                        "Health Decrease Speed: 0.1\n" +
                        "Sprint Decrease Speed: 1.0\n" +
                        "Sprint Regen Speed: 1.0";
    }
    public void hard(){
        ChosenSunrise = 7f;
        ChosenSunset = 19f;
        ChosenHealthDecreaseSpeed = 0.5f;
        ChosenenemyAttackAmount = 15;
        ChosensSprintDecreaseSpeed = 1.5f;
        ChosensSprintRegenSpeed = 0.7f;
                modeInfo.text = "Difficulty: Hard\n" +
                        "Sunrise: 7 AM\n" +
                        "Sunset: 7 PM\n" +
                        "Enemy Attack Amount: 15\n" +
                        "Health Decrease Speed: 0.5\n" +
                        "Sprint Decrease Speed: 1.5\n" +
                        "Sprint Regen Speed: 0.7";
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
