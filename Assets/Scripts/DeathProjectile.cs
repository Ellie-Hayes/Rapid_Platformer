using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathProjectile : MonoBehaviour
{
    public GameObject player;

    Transform Target;
    [SerializeField] float speed;
    [SerializeField] float RotateSpeed;
    [SerializeField] Vector3 Offset;

    Vector3 Direction;
    float distanceThisFrame;
    CameraFollow cameraFollow;

    private void Start()
    {
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    void Update()
    {
        Direction = Target.position - transform.position;
        distanceThisFrame = speed * Time.deltaTime;

        Vector2 Rotatedirection = Target.position - transform.position;
        float angle = Mathf.Atan2(Rotatedirection.y, Rotatedirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    public void ChaseEnemy(Transform target)
    {
        Target = target;
        StartCoroutine(MoveToCheckpoint());
    }

    IEnumerator MoveToCheckpoint()
    {
        while (Vector3.Distance(transform.position, Target.position) > 0.5f)
        {
            transform.Translate(Direction.normalized * distanceThisFrame, Space.World);
            yield return null;
        }

        player.transform.position = Target.position;
        player.GetComponent<PlayerCont>().GetCheckpoint(Target);
        cameraFollow.followObj = player; 
        Destroy(this.gameObject);

        yield return null;
    }
}
