using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpInput : MonoBehaviour
{
    //PLAYER 1 = ARROW KEYS
    public GameObject player;
    private SnakePvpController controller;
    private int horizontal = 0;
    private int vertical = 0;
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    void Awake()
    {
        controller = GetComponent<SnakePvpController>();
    }

    // Update is called once per frame
    void Update()
    {
        //reset values here
        horizontal = 0;
        vertical = 0;
        GetKeyboardInput();
        SetMovement();
    }

    void GetKeyboardInput()
    {
        horizontal = GetAxisRaw(Axis.Horizontal);
        vertical = GetAxisRaw(Axis.Vertical);
        if (horizontal != 0)
        {
            //prevents double movement
            vertical = 0;
        }
    }

    void SetMovement()
    {
        if (vertical != 0)
        {
            controller.SetInputDirection((vertical == 1) ? PlayerDirection.UP : PlayerDirection.DOWN);
        }
        else if (horizontal != 0)
        {
            controller.SetInputDirection((horizontal == 1) ? PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

    int GetAxisRaw(Axis axis)
    {
        //returns 1 or -1 based on input and direction/axis AND PLAYERTAG
        if(player.tag == "Player1")
        {
            if (axis == Axis.Horizontal)
            {
                bool left = Input.GetKeyDown(KeyCode.A);
                bool right = Input.GetKeyDown(KeyCode.D);
                if (left)
                {
                    return -1;
                }
                if (right)
                {
                    return 1;
                }
            }
            else if (axis == Axis.Vertical)
            {
                bool up = Input.GetKeyDown(KeyCode.W);
                bool down = Input.GetKeyDown(KeyCode.S);
                if (up)
                {
                    return 1;
                }
                if (down)
                {
                    return -1;
                }
            }
            return 0;
        }
        else //player 2 tag
        {
            if (axis == Axis.Horizontal)
            {
                bool left = Input.GetKeyDown(KeyCode.LeftArrow);
                bool right = Input.GetKeyDown(KeyCode.RightArrow);
                if (left)
                {
                    return -1;
                }
                if (right)
                {
                    return 1;
                }
            }
            else if (axis == Axis.Vertical)
            {
                bool up = Input.GetKeyDown(KeyCode.UpArrow);
                bool down = Input.GetKeyDown(KeyCode.DownArrow);
                if (up)
                {
                    return 1;
                }
                if (down)
                {
                    return -1;
                }
            }
            return 0;
        }
    }
}
