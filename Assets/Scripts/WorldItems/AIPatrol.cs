using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] Transform GroundCheckPos;
    [SerializeField] Transform WallCheckPos; 
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;

    bool mustPatrol;
    Rigidbody2D rb;
    bool mustturn;
    bool mustTurnWall;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mustPatrol = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol == true)
        {
            Patrol();
        }
    }
    private void FixedUpdate()
    {
        if (mustPatrol == true)
        {
            mustturn = !Physics2D.OverlapCircle(GroundCheckPos.position, 0.1f, groundLayer);
            mustTurnWall = Physics2D.OverlapCircle(WallCheckPos.position, 0.1f, groundLayer) || Physics2D.OverlapCircle(WallCheckPos.position, 0.1f, enemyLayer);
        }
    }
    public void Patrol()
    {
        if (mustturn || mustTurnWall)
        {
            flip();
        }
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }
    public void flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
}
