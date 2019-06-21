using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    //script for OPTIONS tab ONLY
    public Slider spawnRateSlider, speedSlider;
    public Text speedText, spawnRateText;

    void Start()
    {
        //reset the values for the snake every time we click options
        //update text boxes too
        SnakeOptions.Reset();
        speedText.text = "Speed: " + 1/SnakeOptions.speed;
        spawnRateText.text = "Spawn Rate: " + SnakeOptions.spawnRate;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ChangeSpawnRate()
    {
        //spawn rate is in seconds and we need decimals so we divide by 10
        float newSpawn = spawnRateSlider.value/10;
        SnakeOptions.spawnRate = newSpawn;
        spawnRateText.text = "Spawn Rate: " + newSpawn;
    }

    public void ChangeSpeed()
    {
        //as user inreases the slider, speed will increase but the movements per frame will decrease
        //so we use reciprocal
        float newSpeed = 1/speedSlider.value;
        SnakeOptions.speed = newSpeed;
        speedText.text = "Speed: " + 1/newSpeed;
    }
}
