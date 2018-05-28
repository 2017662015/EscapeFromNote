using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Character
{
    //Prefab Instances
    protected GameObject prefab_bullet;

    //Instances
    protected Transform target;
    protected Rigidbody2D rb2D;
    protected List<GameObject> bullets;
    protected List<GameObject> disabledBullets;
    protected List<Transform> bulletSpawnPoses;
    
    //Variables
    protected float bulletShotDelay;

    //Unity Callback Methods
    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
        HitByEraser(coll);
    }
    protected virtual void OnEnable()
    {
        OnEnableProcedure();
    }
    protected virtual void FixedUpdate()
    {
        LookPlayer();
    }

    //Initialize Methods of this class
    private void OnEnableProcedure()
    {
        Init();
        StartCoroutine(CheckState());
    }
    protected override void Init()
    {
        base.Init();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    //State Machine Callback Methods;
    protected override void OnInit()
    {
    }
    protected override void OnIdle()
    {
    }
    protected override void OnMove()
    {
    }
    protected override void OnAttack()
    {
    }
    protected override void OnSkill()
    {
    }
    protected override void OnDamaged()
    {
    }
    protected override void OnDie()
    {
        currentState = BehaviourState.FINALIZE;
    }
    protected override void OnFinalize()
    {
        gameObject.SetActive(false);
    }
    protected override void OnGoToNextStage()
    {
    }

    //Methods
    private void LookPlayer()
    {
        Vector2 _offset = target.position - transform.position;
        float _rotZ = -(Mathf.Atan2(_offset.x, _offset.y) * Mathf.Rad2Deg);
        rb2D.rotation = _rotZ;
    }
    private void HitByEraser(Collision2D coll)
    {
        if (coll.collider.CompareTag("Eraser"))
        {
            currentState = BehaviourState.DIE;
        }
    }

    //Coroutines
    protected override IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
            {
                previousState = currentState;
                switch (currentState)
                {
                    case BehaviourState.INIT:
                        OnInit();
                        break;
                    case BehaviourState.IDLE:
                        OnIdle();
                        break;
                    case BehaviourState.MOVE:
                        OnMove();
                        break;
                    case BehaviourState.ATTACK:
                        OnAttack();
                        break;
                    case BehaviourState.SKILL:
                        OnSkill();
                        break;
                    case BehaviourState.DAMAGED:
                        OnDamaged();
                        break;
                    case BehaviourState.DIE:
                        OnDie();
                        break;
                    case BehaviourState.GO_TO_NEXT_STAGE:
                        OnGoToNextStage();
                        break;
                    case BehaviourState.FINALIZE:
                        OnFinalize();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }
}
