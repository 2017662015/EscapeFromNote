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
    protected StageManagement stageManagement;

    //Variables
    protected int currentStage = 0;
    protected int bulletSpawnPosesCount;
    protected int bulletCountOfStage;
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
        StartCoroutine(CheckStage());  
    }
    protected override void Init()
    {
        base.Init();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        stageManagement = StageManagement.GetInstance();
        bullets = new List<GameObject>();
        disabledBullets = new List<GameObject>();
    }

    //State Machine Callback Methods;
    protected override void OnInit()
    {
        bulletSpawnPosesCount = GetBulletSpawnPoses();
        bulletCountOfStage = GetBulletCountOfStage();
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
        Debug.Log("OnGoTONextStage Called");
        bulletCountOfStage = GetBulletCountOfStage();
        MakeBulletSpaces(bulletCountOfStage);
        currentState = BehaviourState.IDLE;
    }

    //Methods
    private void LookPlayer()
    {
        Vector2 _offset = target.position - transform.position;
        float _rotZ = -(Mathf.Atan2(_offset.x, _offset.y) * Mathf.Rad2Deg);
        rb2D.rotation = _rotZ;
    }
    private void MakeBulletSpaces(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject _bullet = Instantiate(prefab_bullet);
            _bullet.name = _bullet.name.Replace("(Clone)", "");
            _bullet.SetActive(false);
            bullets.Add(_bullet);
            disabledBullets.Add(_bullet);
        }
    }
    private void HitByEraser(Collision2D coll)
    {
        if (coll.collider.CompareTag("Eraser"))
        {
            currentState = BehaviourState.DIE;
        }
    }
    private int GetBulletSpawnPoses()
    {
        bulletSpawnPoses = new List<Transform>();
        int _count = transform.GetChild(0).childCount;
        if (_count > 0)
        {
            for (int i = 0; i < _count; i++)
            {
                bulletSpawnPoses.Add(transform.GetChild(0).GetChild(i)); 
            }
        }
        return _count;
    }
    protected int GetBulletCountOfStage()
    {
        Debug.Log((int)(StageManagement.STAGE_INTERVAL_TIME / bulletShotDelay) * bulletSpawnPosesCount);
        return (int)(StageManagement.STAGE_INTERVAL_TIME / bulletShotDelay) * bulletSpawnPosesCount;
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
    private IEnumerator CheckStage()
    {
        do
        {
            if (currentStage != stageManagement.GetStage())
            {
                currentStage = stageManagement.GetStage();
                currentState = BehaviourState.GO_TO_NEXT_STAGE;
            }
            yield return null;
        }while(currentState != BehaviourState.DIE);
    }
}
