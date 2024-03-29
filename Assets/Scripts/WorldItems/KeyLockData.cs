using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockData : MonoBehaviour
{
    public string KeyLockColour;
    [SerializeField] bool key; 
    bool keyCollected;

    public GameObject player; // in the inspector drag the gameobject the will be following the player to this field
    public int followDistance;
    private List<Vector3> storedPositions;

    PlayerCont playerController;
    int keyNumber;

    LockUI lockUI;
    void Awake()
    {
        playerController = player.GetComponent<PlayerCont>();
        if (key)
        {
            storedPositions = new List<Vector3>(); //create a blank list

            if (!player)
            {
                Debug.Log("The FollowingMe gameobject was not set");
            }

            if (followDistance == 0)
            {
                Debug.Log("Please set distance higher then 0");
            }
        }
        
    }
    void Start()
    {
        if (!key) {
            lockUI = GetComponentInChildren<LockUI>();
            lockUI.colour = KeyLockColour;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (keyCollected)
        {
            if (storedPositions.Count == 0)
            {
                Debug.Log("blank list");
                storedPositions.Add(player.transform.position); //store the players currect position
                return;
            }
            else if (storedPositions[storedPositions.Count - 1] != player.transform.position)
            {
                //Debug.Log("Add to list");
                storedPositions.Add(player.transform.position); //store the position every frame
            }

            if (storedPositions.Count > followDistance + (keyNumber * 10))
            {
                transform.position = storedPositions[0]; //move
                storedPositions.RemoveAt(0); //delete the position that player just move to
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !key)
        {
            PlayerCont controller = collision.gameObject.GetComponent<PlayerCont>();

            foreach (var obj in controller.ownedItems)
            {
                KeyLockData data = obj.GetComponent<KeyLockData>();
                if (data.KeyLockColour == KeyLockColour)
                {
                    Door door = GetComponent<Door>();
                    LockUI ui = GetComponentInChildren<LockUI>();
                    controller.ownedItems.Remove(obj);
                    Destroy(obj);
                    door.moveUp = true; 
                    ui.open= true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && key && !keyCollected)
        {
            PlayerCont controller = collision.gameObject.GetComponent<PlayerCont>();

            keyCollected = true;
            keyNumber = controller.keysCollected;
            controller.keysCollected++;

            controller.ownedItems.Add(gameObject);
        }
    }

   


    


   
}

