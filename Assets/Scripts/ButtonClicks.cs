using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClicks : MonoBehaviour
{
    public void Play(string str)
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play(str);
        }
    }
}
