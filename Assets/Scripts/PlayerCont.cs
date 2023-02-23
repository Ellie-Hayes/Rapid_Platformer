using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    public CharacterMovement controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    Rigidbody2D rb;
    bool jump = false;
    public List<GameObject> ownedItems = new List<GameObject>();
    public int keysCollected; 

    [SerializeField] private LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            rb.gravityScale = rb.gravityScale * -1;
            controller.jumpDirection = controller.jumpDirection * -1;

            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            controller.ExtraJump = true; 
        }
        if (collision.gameObject.layer == 10)
        {
            Destroy(gameObject);
        }

    }
}