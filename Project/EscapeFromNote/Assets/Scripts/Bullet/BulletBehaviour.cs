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
    }

    //Methods
    private void ApplyReflection(Collision2D coll)
    {
        GetNextDirFromReflection(coll);
    }
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
    private float GetNextDirFromReflection(Collision2D coll)
    {
        if(coll.collider.CompareTag("Wall"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            int contactsLength = coll.GetContacts(contacts);
            float _angle = Mathf.Asin(Vector3.Dot(contacts[0].normal, moveDir));
            return _angle;
        }
        else
        {
            return 0.0f;
        }
    }
}
