using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    //tags class for collision detection
    public static string WALL = "Wall";
    public static string FRUIT = "Fruit";
    public static string BOMB = "Bomb";
    public static string TAIL = "Tail";
}

public class Metrics
{
    public static float NODE = 1.1f;
}

public enum PlayerDirection
{
    LEFT = 0,
    UP = 1,
    RIGHT = 2,
    DOWN = 3,
    COUNT = 4
}

public class SnakeOptions
{
    public static float speed = 0.1f; //update refresh rate
    public static float spawnRate = 1f;

    public static void Reset()
    {
        speed = 0.1f;
        spawnRate = 1f;
    }
}
