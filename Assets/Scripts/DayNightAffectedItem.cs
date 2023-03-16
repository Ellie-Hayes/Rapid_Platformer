using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightAffectedItem : MonoBehaviour
{
    bool dayState = true;
    public bool NightObj;
    PlayerCont playerCont;
    public Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        playerCont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCont>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dayState != playerCont.IsDay)
        {
            dayState = playerCont.IsDay;
            switchCollider();
        }
    }

    void switchCollider()
    {
        if (!dayState && NightObj)
        {
            col.enabled = true; 
        }
        else if (dayState && !NightObj)
        {
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }

}
