using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockUI : MonoBehaviour
{
    public string colour;
    public bool open;
    bool showUI;
    bool playerInRange;
    bool stopChecking;
    [SerializeField] GameObject UICanvas;
    [SerializeField] GameObject CrossUI;
    PlayerCont controller;


    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCont>();
    }
    private void Update()
    {
        if (showUI && !open) { UICanvas.SetActive(true); }
        else { UICanvas.SetActive(false);}
        
    }
    private void OnMouseOver() { showUI = true; CheckKey(); }
    private void OnMouseExit()
    {
        if (!playerInRange) { showUI = false; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { playerInRange = true; showUI = true; CheckKey(); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { playerInRange = false; showUI = false; }
    }

    void CheckKey()
    {
        if (!stopChecking)
        {
            foreach (var obj in controller.ownedItems)
            {
                KeyLockData data = obj.GetComponent<KeyLockData>();
                if (data.KeyLockColour == colour)
                {
                    stopChecking = true; 
                    CrossUI.SetActive(false);
                }
            }
        }
        
    }
}
