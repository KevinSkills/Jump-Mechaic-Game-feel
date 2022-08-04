using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myMath : MonoBehaviour
{
    public static bool almostNumber(float input, float target) {
        if (Mathf.Abs(input - target) < 0.01) return true;

        return false; 
    }
}
