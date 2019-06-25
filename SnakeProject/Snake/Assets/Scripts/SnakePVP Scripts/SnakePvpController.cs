using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakePvpController : MonoBehaviour
{
    public GameObject player; //GREEN OR PURPLE
    private Rigidbody head, body;

    private float counter;
    private bool movePlayer, createNode;

    public PlayerDirection direction;
    public float step_length = 1.1f;
    private float movement_frequency;

    [SerializeField]
    //node to add
    private GameObject tailPrefab;

    //lists of change in position Vector3s and snake nodes
    private List<Vector3> deltaPosition;
    private List<Rigidbody> nodes;

    void Awake()
    {
        //get two UNIQUE rigidbodys for the two snakes
        body = player.GetComponent<Rigidbody>();
        Time.timeScale = 1f; //unpauses the game
        //SnakeOptions.speed = 0.1f; //default speed
        InitSnakeNodes();
        InitPlayers();
        UpdateSpeed();

        //universal direction list for both snakes
        deltaPosition = new List<Vector3>()
        {
            new Vector3(-step_length, 0f, 0f), //left, x is negative
            new Vector3(0f, step_length, 0f), //dy, up )
            new Vector3(step_length, 0f, 0f), //dx, right x is positive
            new Vector3(0f, -step_length, 0f) //-dy, down
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate()
    {
        if (movePlayer)
        {
            movePlayer = false;
            MovePlayer();
        }
    }

    void InitSnakeNodes()
    {
        //must initialize nodes for both snakes using the GameObjects
        nodes = new List<Rigidbody>();
        nodes.Add(player.transform.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(player.transform.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(player.transform.GetChild(2).GetComponent<Rigidbody>());
        head = nodes[0];
    }

    void InitPlayers()
    {
        //determines direction of player depending on player tag
        direction = this.tag == "Player1" ? PlayerDirection.RIGHT : PlayerDirection.LEFT;
        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2, 0f, 0f);
                break;
            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2, 0f, 0f);
                break;
            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2, 0f);
                break;
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2, 0f);
                break;
        }
    }

    //called every frame
    void MovePlayer()
    {
        //calculating position of head node
        Vector3 dPosition = deltaPosition[(int)direction];
        Vector3 parentPos = head.position;
        Vector3 prevPos;
        body.position = body.position + dPosition;
        head.position = head.position + dPosition;

        //moving all nodes after head
        for (int i = 1; i < nodes.Count; i++)
        {
            prevPos = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPos;
        }

        //check if we need to create a new node bc pickup
        if (createNode)
        {
            createNode = false;
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(player.transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;
        if (counter >= movement_frequency)
        {
            counter = 0;
            movePlayer = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
       if ((dir == PlayerDirection.UP && direction == PlayerDirection.DOWN) ||
       (dir == PlayerDirection.DOWN && direction == PlayerDirection.UP) ||
       (dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT) ||
       (dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT))
       {
           return;
       }
       direction = dir;
       ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        movePlayer = false;
        MovePlayer();
    }

    void OnTriggerEnter(Collider target)
    {
        if(target.tag == Tags.TAIL)
        {
            //snake dies on hitting other player
            print(player.tag + " Died!");
            AudioController.instance.playDeadSound();
            Time.timeScale = 0f;
            //use current tag here, opposite in gameOverPvp function
            PvpGameplayController.instancePvp.GameOverPvp(player.tag);
        }
        else if(target.tag == Tags.BOMB || target.tag == Tags.WALL)
        {
            //compare score to get winner
            print(player.tag + " Died!");
            AudioController.instance.playDeadSound();
            Time.timeScale = 0f;
            PvpGameplayController.instancePvp.GameOverPvp("compareScores");
        }

        if (target.tag == Tags.FRUIT)
        {
            //Need to edit for p1/p2 gameplay controllers
            PvpGameplayController.instancePvp.SetScorePvp(player.tag);
            AudioController.instance.playFruitSound();
            target.gameObject.SetActive(false);
            createNode = true;
        }
    }

    public void UpdateSpeed()
    {
        //used by options to update snake's speed
        movement_frequency = SnakeOptions.speed;
    }
}

