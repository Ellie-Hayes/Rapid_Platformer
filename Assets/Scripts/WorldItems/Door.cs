using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update

    public bool moveUp;

    [SerializeField] bool sideDoor;
    [SerializeField] float moveAmount;
    [SerializeField] float speed; 
    float xmoveAmount;
    float ymoveAmount;

    bool moveDown = true;

    Vector2 originalPosition;
    Vector2 lerpPosition;

    void Start()
    {
        if (sideDoor) { xmoveAmount = -moveAmount + transform.position.x; ymoveAmount = transform.position.y; }
        else { ymoveAmount = moveAmount + transform.position.y; xmoveAmount = transform.position.x;  }   

        originalPosition= transform.position;
        lerpPosition = new Vector2(xmoveAmount, ymoveAmount);

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (moveUp)
        {
            transform.position = Vector2.Lerp(transform.position, lerpPosition, speed);
        }
        else if(moveDown)
        {
            transform.position = Vector2.Lerp(transform.position, originalPosition, speed);
        }
    }
}
