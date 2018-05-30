using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {
    //Instances
    private CircleCollider2D coll;
    private Rigidbody2D rb2D;

    //Variables
    private bool isPlayerHit;
    private bool isWaiting;
    private float moveSpeed;
    private float currentTime;
    private Vector2 moveDir;
    private Vector2 nextMoveDir;

    //Constants
    private const float BULLET_DELAY_TIME = 2.0f;

    //Setter Method
    public void SetMoveDir(Vector2 dir) { this.moveDir = dir; }

    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    private void Update()
    {
        DelayAfterHit();
    }
    private void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            rb2D.AddForce((transform.up + (transform.right * 0.7f)) * Time.fixedDeltaTime * 10000.0f);
        }
        GetNextDirWithRay();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckPlayerHit(collision);
    }

    //Initialize Method of this class
    private void Init()
    {
        coll = gameObject.GetComponent<CircleCollider2D>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        moveDir = transform.up;
    }

    //Methods
    private void DelayAfterHit()
    {
        if(isPlayerHit)
        {
            if (currentTime < BULLET_DELAY_TIME) 
            {
                currentTime += Time.deltaTime;
                isWaiting = true;
                coll.enabled = false;
            }
            else
            {
                isPlayerHit = false;
                isWaiting = false;
                coll.enabled = true;
            }
        }
    }
    private void CheckPlayerHit(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player") && !isWaiting)
        {
            isPlayerHit = true;
        }
    }
    private void GetNextDirWithRay()
    {
        RaycastHit2D hit2D = new RaycastHit2D();
        hit2D = Physics2D.Raycast(transform.position, moveDir, Mathf.Infinity);
        //Debug.Log(Camera.main.WorldToScreenPoint(hit2D.point).ToString());
        //Debug.Log(hit2D.collider.tag);
        Debug.DrawRay(transform.position, moveDir, Color.red, 3f);
        //float _moveDir_x = hit2D.point.x - transform.position.x;
        //float _moveDir_y = hit2D.point.y - transform.position.y;
        //moveDir = new Vector2(_moveDir_x, _moveDir_y);
    }
}
