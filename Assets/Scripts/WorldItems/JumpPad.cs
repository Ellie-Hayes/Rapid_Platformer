using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private Vector2 jumpForce;
    Animator anim; 
    private void Start()
    {
       anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.up * jumpForce;
            anim.SetTrigger("Spring");
           
        }
    }

}
