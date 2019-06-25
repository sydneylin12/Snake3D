using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    private SnakeController controller;
    private int horizontal = 0;
    private int vertical = 0;
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<SnakeController>();
    }

    // Update is called once per frame
    void Update()
    {
        //reset values here
        horizontal = 0;
        vertical = 0;
    }

    public void SetMovementUp()
    {
        //prevents same axis opposite movement
        if(controller.direction == PlayerDirection.DOWN)
        {
            return;
        }
        controller.direction = PlayerDirection.UP;
    }
    public void SetMovementDown()
    {
        //prevents same axis opposite movement
        if (controller.direction == PlayerDirection.UP)
        {
            return;
        }
        controller.direction = PlayerDirection.DOWN;
    }
    public void SetMovementLeft()
    {
        //prevents same axis opposite movement
        if (controller.direction == PlayerDirection.RIGHT)
        {
            return;
        }
        controller.direction = PlayerDirection.LEFT;
    }
    public void SetMovementRight()
    {
        //prevents same axis opposite movement
        if (controller.direction == PlayerDirection.LEFT)
        {
            return;
        }
        controller.direction = PlayerDirection.RIGHT;
    }
}
