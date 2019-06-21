using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;
    public GameObject fruit, bomb;
    public Text scoreText, startEndText;
    public Button pauseButton;
    private bool isOver, isPaused;
    private int score;
    private float minX = -24f, maxX = 24f, minY = -14f, maxY = 14f;
    private float zPos = -0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        MakeInstance();
        isOver = false;
        isPaused = false;
    }

    void Start()
    {
        scoreText.text = "Score: 0";
        startEndText.text = "";
        Invoke("StartSpawning", 0.5f);
    }

    void Update()
    {
        if(isOver == true && Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("Snake");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseButton.Select();
            PauseUnpauseGame();
        }
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPickups());
    }

    public void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }
    
    public void GameOver()
    {
        isOver = true;
        startEndText.text = "Game Over!\nPress [space] to Restart!";
    }

    IEnumerator SpawnPickups()
    {
        //making fruit or bomb pickups
        yield return new WaitForSeconds(Random.Range(SnakeOptions.spawnRate, SnakeOptions.spawnRate + 0.5f));
        if(Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), zPos), Quaternion.identity);
        }
        else
        {
            Instantiate(bomb, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), zPos), Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    public void SetScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

    public void PauseUnpauseGame()
    {
        //game must not be over to use pause button
        print("CLICKED");
        if (!isPaused && !isOver) //if you click pause
        {
            Time.timeScale = 0f; //handles endgame unpause
            startEndText.text = "PAUSED";
            isPaused = true;
        }
        else if(isPaused && !isOver)
        {
            //handles unselecting the button to unpause
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            startEndText.text = "";
            isPaused = false;
            Time.timeScale = 1;
        }
    }

}
