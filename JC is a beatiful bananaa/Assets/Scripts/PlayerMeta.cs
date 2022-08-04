using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeta : MonoBehaviour
{
    public static PlayerDef[] defs;
    public PlayerDef[] pd;

    private void Awake()
    {
        if (defs == null) defs = pd;
        else Debug.LogWarning("you have multiple PlayerMeta scripts in scene");
        
    }

}

[System.Serializable]
public class PlayerDef
{
    public KeyCode up, down, left, right;
    public Color c;

}



