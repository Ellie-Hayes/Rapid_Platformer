using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    Animator anim;
    bool falling;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !falling)
        {
            anim.SetTrigger("BlockFall");

            StartCoroutine("Restore");
        }
    }

    IEnumerator Restore()
    {
        yield return new WaitForSeconds(3.0f);
        anim.SetTrigger("BlockRestore");
        falling = false;
    }
}
