using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public enum LeverType { DoorUnlock, timerLever };
    public LeverType type;

    [SerializeField] Sprite leverOn;
    [SerializeField] Sprite leverOff;
    [SerializeField] Door affectedDoor;

    SpriteRenderer rend;
    bool playerInRange;
    bool leverTurnedOn = true;

    [Header("Timer")]
    [SerializeField] float indicatorTimer = 1.0f;
    [SerializeField] float maxIndicatorTimer = 1.0f;
    [SerializeField] private Image radialIndicatorUI = null;
    [SerializeField] GameObject timerCanvas;

    bool startTimer;
    bool canInteract = true;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && canInteract) {
            if (leverTurnedOn) { rend.sprite = leverOff; }
            else { rend.sprite = leverOn;}

            leverTurnedOn = !leverTurnedOn;
            LeverBehaviour();
        }

        UpdateTimer();
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

    void LeverBehaviour()
    {
        if(type == LeverType.DoorUnlock)
        {
            if (leverTurnedOn) { affectedDoor.moveUp = true; }
            else { affectedDoor.moveUp = false; }
        }
        else if (type == LeverType.timerLever)
        {
            startTimer = true;
            affectedDoor.moveUp = true;
            timerCanvas.SetActive(true);
        }

    }

    void UpdateTimer()
    {
        if (startTimer)
        {
            canInteract = false;
            indicatorTimer -= Time.deltaTime;
            radialIndicatorUI.enabled = true;
            radialIndicatorUI.fillAmount = indicatorTimer / maxIndicatorTimer;

            if (indicatorTimer <= 0)
            {
                indicatorTimer = maxIndicatorTimer;
                radialIndicatorUI.fillAmount = maxIndicatorTimer;
                radialIndicatorUI.enabled = false;
                startTimer = false;

                rend.sprite = leverOff;
                leverTurnedOn = false;
                affectedDoor.moveUp = false;
                timerCanvas.SetActive(false);
                canInteract = true;
            }
        }
        
    }
}
