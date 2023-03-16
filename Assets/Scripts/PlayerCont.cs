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
    private bool upDashing = false; 
    [SerializeField] private float dashingPower = 100f;
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private float dashingCooldown = 1.5f;
    [SerializeField] private float upDashFloat;

    [SerializeField] Animator anim;

    public bool IsDay = true;
    [SerializeField] GameObject spriteMask; 
    Animator maskAnim;

    public GhostEffect ghost; 


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

        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 Point_1 = new Vector2(transform.position.x, transform.position.y);
        //Vector2 Point_2 = new Vector2(worldPosition.x, worldPosition.y);
        //float angle = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;

        //Debug.Log(angle);
    }

    private void FixedUpdate()
    {
        if (isDashing || upDashing ) { return; }
        
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
        rb.gravityScale = 4;

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
        //Removes gravity for smooth and predictable dash
        canDash = false; 
        float originalGravity = rb.gravityScale; 
        rb.gravityScale = 0f;

        ghost.displayGhost = true;

        //Gets mouse position and calculates which way to dash based on set angles 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Vector2 Point_1 = new Vector2(transform.position.x, transform.position.y);
        Vector2 Point_2 = new Vector2(worldPosition.x, worldPosition.y);
        float angle = Mathf.Atan2(Point_2.y - Point_1.y, Point_2.x - Point_1.x) * Mathf.Rad2Deg;

        if (angle < -130 || angle > 140 ) { rb.velocity = new Vector2(-dashingPower, 0f); isDashing = true; } // Dash left
        else if (angle > 30 && angle < 140) { rb.velocity = new Vector2(0f, dashingPower); upDashing = true; } // Dash up
        else if (angle > -50 && angle < 30) { rb.velocity = new Vector2(dashingPower, 0f); isDashing = true; } // Dash right
        else { yield return null; }

        //Resets player to original state and removes ghost effect
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        ghost.displayGhost = false;

        //snaps back to normal if dashing to side, if dashing up allows a small window of "float" time to move around
        if (!upDashing) { rb.velocity = Vector3.zero; }
        else { rb.velocity = Vector2.up * upDashFloat; }
        isDashing = false; upDashing= false;


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

    public void ChangeWorld()
    {
        IsDay = !IsDay;
    }

}