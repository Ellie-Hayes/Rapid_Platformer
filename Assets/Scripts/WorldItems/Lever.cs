using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool playerInRange;
    SpriteRenderer rend;
    public Sprite leverOn;
    public Sprite leverOff;

    bool leverTurnedOn = true;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) {
            if (leverTurnedOn) { rend.sprite = leverOff; }
            else { rend.sprite = leverOn;}

            leverTurnedOn = !leverTurnedOn;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            playerInRange = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { 
            playerInRange = false;
        }
    }
}
