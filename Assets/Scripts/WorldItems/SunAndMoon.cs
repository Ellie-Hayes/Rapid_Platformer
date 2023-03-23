using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SunAndMoon : MonoBehaviour
{
    bool playerInRange;
    PlayerCont playerCont;
    [SerializeField] float WaitTime;
    bool CanSwitch = true; 

    private void Start()
    {
         playerCont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCont>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && CanSwitch)
        {
            playerCont.ChangeWorld();
            CanSwitch = false;
            StartCoroutine("wait");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            playerCont.Checkpoint = transform; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(WaitTime);
        CanSwitch = true; 
    }
}
