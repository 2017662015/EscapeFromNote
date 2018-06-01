using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Character
{
    //Prefab Instances
    protected GameObject prefab_bullet;
    protected GameObject prefab_item_pencilcase;

    //Instances
    protected Transform target;
    protected Transform uiRoot;
    protected Rigidbody2D rb2D;
    protected UISprite uiSprite;
    protected BoxCollider2D boxColl;
    protected List<GameObject> bullets;
    protected List<GameObject> disabledBullets;
    protected List<Transform> bulletSpawnPoses;
    protected StageManagement stageManagement;

    //Variables
    protected bool isPencilCaseContained;
    protected int currentStage = 0;
    protected int bulletSpawnPosesCount;
    protected int bulletCountOfStage;
    protected int aliveBulletCount = 0;
    private float waitTime = 0.0f;
    private float spawnDelay = 0.0f;
    protected float bulletShotDelay;

    //Constants
    protected float ENEMY_SPAWN_DELAY_LENGTH = 0.5f;

    public void SetIsPencilCaseContained(bool condition) { this.isPencilCaseContained = condition; }

    //Unity Callback Methods
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        base.OnTriggerEnter2D(coll);
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
    protected virtual void Update()
    {
        WaitForShot();
        //TODO: Need to be finalize on EnemyManagement
        if(stageManagement.GetCurrentState() == GameManagement.GameState.GAMEOVER)
        {
            OnFinalize();
        }
    }
    protected virtual void OnDisable()
    {
        Finalize();
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
        uiRoot = GameObject.Find("UI Root").transform;
        target = GameObject.FindWithTag("Player").transform;
        stageManagement = StageManagement.GetInstance();
        uiSprite = gameObject.GetComponent<UISprite>();
        boxColl = gameObject.GetComponent<BoxCollider2D>();
        prefab_item_pencilcase = Resources.Load("Prefabs/Item_PencilCase") as GameObject;
    }

    //Finalize Method of this class
    protected override void Finalize()
    {
        base.Finalize();
        boxColl.enabled = true;
        uiSprite.enabled = true;
        currentStage = 0;
        aliveBulletCount = 0;
        bulletSpawnPosesCount = 0;
        bulletCountOfStage = 0;
        waitTime = 0;
        spawnDelay = 0;
        bulletShotDelay = 0;
        bulletSpawnPoses = null;
        DestroyAllBullets();
    }

    //State Machine Callback Methods;
    protected override void OnInit()
    {
        bulletSpawnPosesCount = GetBulletSpawnPoses();
        bulletCountOfStage = GetBulletCountOfStage();
    }
    protected override void OnIdle()
    {
        WaitForShot();
    }
    protected override void OnMove()
    {
    }
    protected override void OnAttack()
    {
        Shot();
        currentState = BehaviourState.IDLE;
    }
    protected override void OnSkill()
    {
        ClearAllBullet();
    }
    protected override void OnDamaged()
    {
    }
    protected override void OnDie()
    {
        if (isPencilCaseContained)
        {
            Instantiate(prefab_item_pencilcase, Camera.main.WorldToScreenPoint(gameObject.transform.position), Quaternion.identity);
        }
        uiSprite.enabled = false;
        boxColl.enabled = false;
        StartCoroutine(WaitForFinalize());
    }
    protected override void OnFinalize()
    {
        gameObject.SetActive(false);
    }
    protected override void OnGoToNextStage()
    {
        currentState = BehaviourState.IDLE;
        if (bullets == null || bullets.Count == 0)
        {
            MakeBulletSpaces(bulletCountOfStage);
        }
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
        bullets = new List<GameObject>();
        disabledBullets = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject _bullet = Instantiate(prefab_bullet, uiRoot);
            _bullet.name = _bullet.name.Replace("(Clone)", "");
            _bullet.SetActive(false);
            bullets.Add(_bullet);
            disabledBullets.Add(_bullet);
        }
    }
    private void HitByEraser(Collider2D coll)
    {
        if (coll.CompareTag("Eraser"))
        {
            currentState = BehaviourState.DIE;
        }
    }
    private void WaitForShot()
    {
        if (stageManagement.GetCurrentState() == GameManagement.GameState.PLAY)
        {
            if (currentState == BehaviourState.IDLE && currentStage > 0)
            {
                if (waitTime < bulletShotDelay)
                {
                    waitTime += Time.deltaTime;
                }
                else
                {
                    waitTime = 0;
                    currentState = BehaviourState.ATTACK;
                }
            }
        }
    }
    public void ClearAllBullet()
    {
        int i = 0;
        do
        {
            if (bullets[i].activeSelf)
            {
                bullets[i].SetActive(false);
                disabledBullets.Add(bullets[i]);
                aliveBulletCount--;
            }
            i++;
        } while (disabledBullets.Count != bullets.Count);
        currentState = BehaviourState.IDLE;
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
    protected void DestroyAllBullets()
    {
        if (bullets != null)
        {
            do
            {
                Destroy(bullets[0]);
                bullets.RemoveAt(0);
            } while (bullets.Count != 0);
            bullets = null;
            disabledBullets = null;
        }
    }
    protected int GetBulletCountOfStage()
    {
        return (int)(StageManagement.STAGE_INTERVAL_TIME / bulletShotDelay) * bulletSpawnPosesCount;
    }
    protected virtual void Shot()
    {
        if(disabledBullets.Count != 0)
        {
            for(int i = 0; i < bulletSpawnPoses.Count; i++)
            {
                disabledBullets[0].SetActive(true);
                disabledBullets[0].transform.position = bulletSpawnPoses[i].position;
                disabledBullets[0].GetComponent<BulletBehaviour>().SetEnemyBehaviour(this);
                disabledBullets[0].GetComponent<BulletBehaviour>().SetMoveDir(bulletSpawnPoses[i].up);
                disabledBullets.RemoveAt(0);
                aliveBulletCount++;
            }
        }
        else
        {
            Reload();
        }
    }
    protected virtual void Reload()
    {
        int i = 0;
        do
        {
            if(!bullets[i].activeSelf)
            {
                disabledBullets.Add(bullets[i]);
            }
            i++;
        } while (disabledBullets.Count != bullets.Count - aliveBulletCount);
    }
    public void DecreaseAliveBulletCount() { aliveBulletCount--; }

    //Coroutines
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
        } while (currentState != BehaviourState.DIE);
    }
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
    private IEnumerator WaitForFinalize()
    {
        do
        {
            yield return null;
        } while (aliveBulletCount != 0);
        currentState = BehaviourState.FINALIZE;
    }
}
 
