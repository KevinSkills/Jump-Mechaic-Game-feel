using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public List<Transform> players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = new Vector3(0,0,0);
        foreach (Transform t in players) {
            cameraPos += t.position;
        }
        cameraPos /= players.Count;

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);

        
    }
}
