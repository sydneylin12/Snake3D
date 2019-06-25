using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioClip fruitSound, deadSound;
    public int temp;

    // Start is called before the first frame update
    void Awake()
    {
        MakeInstance();
    }

    // Update is called once per frame
    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void playFruitSound()
    {
        AudioSource.PlayClipAtPoint(fruitSound, transform.position);
    }

    public void playDeadSound()
    {
        AudioSource.PlayClipAtPoint(deadSound, transform.position);
    }
}
