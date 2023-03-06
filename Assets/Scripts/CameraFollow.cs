using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject followObj;
    [SerializeField] float offset;
   

    // Update is called once per frame
    void Update()
    {
        if (followObj == null)
        {
            followObj = GameObject.FindGameObjectWithTag("Player");
        }
        transform.position = new Vector3(followObj.transform.position.x, followObj.transform.position.y, offset);
    }
}
