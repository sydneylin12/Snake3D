using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PvpGameplayController : MonoBehaviour
{
    public static PvpGameplayController instancePvp;
    //helps with reloading scene
    public Canvas c;
    //objects to be spawned
    public GameObject fruit, bomb;
    //score counter and play/pause/endgame text
    public Text scoreText, startEndText;
    //button in corner to pause
    public Button pauseButton;
    //is the game over? is it paused?
    private bool isOver, isPaused;
    //keeps track of 1 player score NOT INHERETED
    private int scoreP1, scoreP2;
    //map coordinates for spawning
    private float minX = -24f, maxX = 24f, minY = -14f, maxY = 14f, zPos = -0.5f;
    // Start is called before the first frame update

    void Awake()
    {
        MakeInstance();
        isOver = false;
        isPaused = false;
    }
    void Start()
    {
        scoreP1 = 0;
        scoreP2 = 0;
        scoreText.text = "Green: 0 | Purple: 0";
        startEndText.text = "";
        Invoke("StartSpawning", 0.5f);
    }

    void Update()
    {
        if (isOver == true && Input.GetKeyDown("space") && c.tag == "PC")
        {
            SceneManager.LoadScene("Snake");
        }
        if (isOver == true && Input.GetKeyDown("space") && c.tag == "PC_PVP")
        {
            SceneManager.LoadScene("SnakePVP");
        }
        if (isOver == true && Input.GetKeyDown("space") && c.tag == "Mobile")
        {
            SceneManager.LoadScene("MobileSnake");
        }
        if (isOver == true && Input.GetKeyDown("space") && c.tag == "Mobile_PVP")
        {
            SceneManager.LoadScene("MobileSnakePVP");
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

    public void MakeInstance()
    {
        if (instancePvp == null)
        {
            instancePvp = this;
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnPickups());
    }

    public void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }

    public void GameOverPvp(string s)
    {
        isOver = true;
        if(s == "Player1")
        {
            startEndText.text = "Player 2 Wins!\nPress [space] to Restart!";
        }
        else if(s == "Player2")
        {
            startEndText.text = "Player 1 Wins!\nPress [space] to Restart!";
        }
        else if(s == "compareScores")
        {
            //win by comparing score
            if(scoreP1 > scoreP2)
            {
                startEndText.text = "Player 1 Wins!\nPress [space] to Restart!";
            }
            else if(scoreP2 > scoreP1)
            {
                startEndText.text = "Player 2 Wins!\nPress [space] to Restart!";
            }
            else
            {
                startEndText.text = "Tie Game!\nPress [space] to Restart!";
            }
        }
    }

    public IEnumerator SpawnPickups()
    {
        //making fruit or bomb pickups
        yield return new WaitForSeconds(Random.Range(SnakeOptions.spawnRate, SnakeOptions.spawnRate + 0.5f));
        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), zPos), Quaternion.identity);
        }
        else
        {
            Instantiate(bomb, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), zPos), Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    public void SetScorePvp(string s)
    {
        if (s == "Player1")
        {
            scoreP1++;
        }
        else
        {
            scoreP2++;
        } 
        scoreText.text = "Green : " + scoreP1 + " | Purple: " + scoreP2;
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
        else if (isPaused && !isOver)
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
