using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    [Header("Movement")]
    public CharacterMovement controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    Rigidbody2D rb;
    [SerializeField] private LayerMask ground;
    bool jump = false;


    [Header("Keys")]
    public List<GameObject> ownedItems = new List<GameObject>();
    public int keysCollected; 

    
    [Header("Death")]
    Transform Checkpoint;
    [SerializeField] Transform StartPoint;
    [SerializeField] GameObject soul;
    CameraFollow cameraFollow;
    bool dead;

    [SerializeField] GameObject JumpDownEffect;

    [Header("Dash")]
    private bool canDash = true;

    private bool isDashing = false;
    [SerializeField] private float dashingPower = 100f;
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private float dashingCooldown = 1.5f;

    [SerializeField] Animator anim;

    bool IsDay = true;
    [SerializeField] GameObject spriteMask; 
    Animator maskAnim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Checkpoint = StartPoint;

        maskAnim = spriteMask.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) { return; }
       
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        Inputs();

        anim.SetFloat("speed", Mathf.Abs(horizontalMove));
        anim.SetBool("dashing", isDashing);
        maskAnim.SetBool("IsDay", IsDay);
    }

    private void FixedUpdate()
    {
        if (isDashing) { return; }
        
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) { controller.ExtraJump = true; } // Give extra jump for wall sliding and hitting ground in general
        if (collision.gameObject.layer == 10) { StartCoroutine(SpawnSoul()); } // Kills the player when hit enemy / obstacles
    }

    IEnumerator SpawnSoul()
    {
        spriteMask.transform.parent = null;
        GameObject Soul = (GameObject)Instantiate(soul, this.transform.position, this.transform.rotation);
        DeathProjectile projectile = Soul.GetComponent<DeathProjectile>();

        projectile.player = gameObject; 
        projectile.ChaseEnemy(Checkpoint);

        changeFollowObj(Soul);
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

    public void GetCheckpoint(Transform target) // Called when player is "revived"
    {
        dead = false;
        Checkpoint = target;
        StartPoint = Checkpoint;

        changeFollowObj(this.gameObject);

        spriteMask.transform.parent = this.transform;
        spriteMask.transform.position = transform.position;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (worldPosition.x < transform.position.x) { rb.velocity = new Vector2(-dashingPower, 0f); }
        else { rb.velocity = new Vector2(dashingPower, 0f); }

        //trailRend.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //trailRend.emitting = false;
        rb.gravityScale = originalGravity;
        rb.velocity= Vector3.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void changeFollowObj(GameObject objToFollow)
    {
        CameraFollow[] followingObjects = FindObjectsOfType<CameraFollow>();

        foreach (var obj in followingObjects)
        {
            obj.followObj = objToFollow;
        }
    }

    void Inputs()
    {
       
        if (Input.GetKeyDown(KeyCode.Space)) { jump = true; }  //Jump
        if (Input.GetKeyDown(KeyCode.F) && canDash) { StartCoroutine("Dash"); } //Dash 
        if (Input.GetKeyDown(KeyCode.N)) { IsDay = !IsDay; } //Night

        //Mouse gravity to be changed
        if (Input.GetMouseButtonDown(0))
        {
            rb.gravityScale = rb.gravityScale * -1;
            controller.jumpDirection = controller.jumpDirection * -1;

            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }

        
    }

}