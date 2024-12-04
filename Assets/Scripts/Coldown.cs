using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coldown : MonoBehaviour {

    public static float add(float time)
    {
        time += Time.deltaTime;
        return time;
    }

    public static float reset()
    {
        return 0;
    }
}