using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool displayGhost;

    [SerializeField] SpriteRenderer spriteRend; 

    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                Sprite currentSprite = spriteRend.sprite; 
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                Vector3 scale = transform.localScale;
                currentGhost.transform.localScale = scale;

                ghostDelaySeconds = ghostDelay;
            }
        }
       
    }
}
