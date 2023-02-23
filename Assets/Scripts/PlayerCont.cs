using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    [Header("Movement")]
    public CharacterMovement controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;


    [Header("Jumping")]
    Rigidbody2D rb;
    [SerializeField] private LayerMask ground;
    bool jump = false;


    [Header("Keys")]
    public List<GameObject> ownedItems = new List<GameObject>();
    public int keysCollected; 

    
    [Header("Death")]
    public Transform Checkpoint;
    public Transform StartPoint;
    public GameObject soul;

    CameraFollow cameraFollow;
    bool dead; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Checkpoint = StartPoint;

        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        cameraFollow.followObj = gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) { return; }
       
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
            StartCoroutine(SpawnSoul());
        }
       

    }

    IEnumerator SpawnSoul()
    {
        GameObject Soul = (GameObject)Instantiate(soul, this.transform.position, this.transform.rotation);
        DeathProjectile projectile = Soul.GetComponent<DeathProjectile>();

        projectile.player = gameObject; 
        projectile.ChaseEnemy(Checkpoint);

        cameraFollow.followObj = Soul;
        transform.position = new Vector2(-186.8f, 10f);
        rb.gravityScale = 2;

        Vector3 scale = transform.localScale;
        scale.y = 1;
        transform.localScale = scale;
        controller.jumpDirection = 1;

        dead = true; 

        //reset level
        yield break;
    }

    public void GetCheckpoint(Transform target)
    {
        dead = false;
        Checkpoint = target;
        StartPoint = Checkpoint;
    }
}