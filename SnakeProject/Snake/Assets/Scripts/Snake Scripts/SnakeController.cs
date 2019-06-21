using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    private Rigidbody headBody, mainBody;

    //counter for time
    private float counter;
    //boolean for moving the snake
    private bool move;
    //boolean for creating a node on pickup
    private bool createNodeAtTail;

    //value of 0-4 left, up, right, down, count
    public PlayerDirection direction;
    public float step_length = 1.1f; //size of each block
    public float movement_frequency = SnakeOptions.speed;

    [SerializeField]
    //node to add
    private GameObject tailPrefab;

    //lists of change in position Vector3s and snake nodes
    private List<Vector3> deltaPosition;
    private List<Rigidbody> nodes;


    //Awake is pretty much like start
    void Awake()
    {
        //initialize score and the snake
        mainBody = GetComponent<Rigidbody>();
        Time.timeScale = 1f; //unpauses the game
        InitSnakeNodes();
        InitPlayer();
        UpdateSpeed(); //if options was changed

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
        if (move)
        {
            move = false;
            Move();
        }
    }

    void InitSnakeNodes()
    {
        //adds 3 nodes to the snake: head, body, tail
        nodes = new List<Rigidbody>();
        nodes.Add(transform.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(transform.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(transform.GetChild(2).GetComponent<Rigidbody>());
        //the head rigid body is at index 0 because it is the first child
        headBody = nodes[0];
    }

    void SetDirectionRandom()
    {
        //random direction to start game
        //int dir = Random.Range(0, (int)PlayerDirection.COUNT);
        int dir = 2; //RIGHT
        direction = (PlayerDirection)dir;
    }

    void InitPlayer()
    {
        SetDirectionRandom();
        switch (direction)
        {
            //changing x and y axis
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

    void Move()
    {
        //calculating position of head node
        Vector3 dPosition = deltaPosition[(int)direction];
        Vector3 parentPos = headBody.position;
        Vector3 prevPos;
        mainBody.position = mainBody.position + dPosition;
        headBody.position = headBody.position + dPosition;

        //moving all nodes after head
        for(int i = 1; i < nodes.Count; i++)
        {
            prevPos = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPos;
        }

        //check if we need to create a new node bc pickup
        if (createNodeAtTail)
        {
            createNodeAtTail = false;
            //instantiate a new tail node at the last node's position
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency()
    {
        //for fluid movement??
        counter += Time.deltaTime;
        if(counter >= movement_frequency)
        {
            counter = 0;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        //dir = parameter
        //direction = class variable
        if((dir == PlayerDirection.UP && direction == PlayerDirection.DOWN) ||
           (dir == PlayerDirection.DOWN && direction == PlayerDirection.UP) ||
           (dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT) ||
           (dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT))
        {
            //cannot move in opposite directions with snake
            return;
        }

        //else
        direction = dir;
        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }
    
    void OnTriggerEnter(Collider target)
    {
        if(target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL)
        {
            //SNAKE DEATH
            print("Snake died."); //debugging
            AudioController.instance.playDeadSound();
            Time.timeScale = 0f;
            PvpGameplayController.instance.GameOver();
        }

        if(target.tag == Tags.FRUIT)
        {
            //deactivate pickup and make node at end of snake
            PvpGameplayController.instance.SetScore();
            AudioController.instance.playFruitSound();
            target.gameObject.SetActive(false);
            createNodeAtTail = true;
        }
    }

    public void UpdateSpeed()
    {
        //used by options to update snake's speed
        movement_frequency = SnakeOptions.speed;
    }
}
