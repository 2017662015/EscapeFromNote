using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {
    //Instances
    private CircleCollider2D circleColl;
    private Rigidbody2D rb2D;
    private EnemyBehaviour enemyBehaviour;

    //Variables
    [SerializeField][Range(0.0f, 100.0f)]private float moveSpeed = 1.0f;
    private float currentTime = 0.0f;
    private Vector2 moveDir;
    private Vector2 nextMoveDir;

    //Constants
    private const float BULLET_COLL_REACTIVE_DELAY_TIME = 0.2f;
    private const float BULLET_LIFE_TIME = 10.0f;

    //Setter Methods
    public void SetMoveDir(Vector2 dir) { this.moveDir = dir; }
    public void SetEnemyBehaviour(EnemyBehaviour behaviour) { this.enemyBehaviour = behaviour; }

    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    private void Update()
    {
        LifeCycle();
    }
    private void FixedUpdate()
    {
        MoveBullet();
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        CalculateNextDir(coll);
        if(coll.collider.CompareTag("Player") || coll.collider.CompareTag("Enemy"))
        {
            circleColl.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Wall"))
        {
            circleColl.isTrigger = false;
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Player") || coll.CompareTag("Enemy"))
        {
            circleColl.isTrigger = false;
        }
    }
    private void OnDisable()
    {
        currentTime = 0.0f;
        rb2D.velocity = Vector2.zero;
    }

    //Initialize Method of this class
    private void Init()
    {
        circleColl = gameObject.GetComponent<CircleCollider2D>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        moveDir = transform.up;
    }
    private void LifeCycle()
    {
        if(currentTime < BULLET_LIFE_TIME)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            enemyBehaviour.DecreaseAliveBulletCount();
            gameObject.SetActive(false);
        }
    }
    private void CalculateNextDir(Collision2D coll)
    {
        if (coll.collider.CompareTag("Wall"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            int contactCount = Physics2D.GetContacts(coll.collider, contacts);
            //Debug.Log("contactCount : " + contactCount);
            if(contactCount > 0)
            {
                Vector2 _normalVector = contacts[0].normal;
                //Debug.Log("Normal : " + _normalVector);
                float _angle = Mathf.Asin(Vector2.Dot(moveDir, _normalVector)) * Mathf.Rad2Deg;
                //Debug.Log(_angle);
                nextMoveDir = moveDir - 2 * Vector2.Dot(moveDir, _normalVector) * _normalVector;
                moveDir = nextMoveDir;
            }
        }
    }
    private void MoveBullet()
    {
        rb2D.position += moveDir * Time.fixedDeltaTime * moveSpeed;
    }
}
