using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInf : Character
{
    //Prefabs
    private GameObject prefab_eraser;
    private GameObject prefab_white;

    //Instances
    private Transform weaponAxis;
    private TweenColor tweenColor;
    private Coroutine checkEraserCount;
    private PlayerMove playerMove;
    private List<GameObject> erasers;
    private List<Transform> eraserPosAxises;
    private List<Transform> eraserPoses;

    //Variables
    private bool isWhiteEquipped;
    private bool isPencilCaseEquipped;
    [SerializeField][Range(0, 4)]private int currentEraserCount = 0;
    private int previousEraserCount = 0;
    [SerializeField][Range(3, 4)]private int eraserSpaceCount = 0;
    [SerializeField]private float whiteSpawnElapsedTime = 0.0f;

    //Constants
    private const int PLAYER_ERASER_COUNT_INIT = 3;
    private const int PLAYER_ERASER_COUNT_MAX = 4;
    private const int PLAYER_HP_INIT = 2;
    private const float PLAYER_ERASER_SPAWN_TIME = 10.0f;
    private const float PLAYER_WEAPON_ROTATION_SPEED = 500.0f;
    private const float PLAYER_WHITE_SPAWN_TIME = 30.0f;

    //Getters
    private bool GetIsWhiteEquipped() { return this.isWhiteEquipped; }

    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    private void FixedUpdate()
    {
        SpawnWhite();
        RotateWeaponAxis();
    }
    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
        CheckHitByEnemy(coll);
        CheckHitByBullet(coll);
    }
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        base.OnTriggerEnter2D(coll);
        CheckAndGetPencilCase(coll);
        CheckAndGetEraser(coll);
    }

    //Initialze Method of Class
    protected override void Init()
    {
        base.Init();
        prefab_eraser = Resources.Load("Prefabs/Eraser") as GameObject;
        playerMove = gameObject.GetComponent<PlayerMove>();
        tweenColor = gameObject.GetComponent<TweenColor>();
        checkState = StartCoroutine(CheckState());
    }

    //State Machine Callback Methods
    protected override void OnInit()
    {
        hp = PLAYER_HP_INIT;
        EraserInit();
        checkEraserCount = StartCoroutine(CheckEraserCount());
        currentState = BehaviourState.IDLE;
    }
    protected override void OnIdle()
    {
        
    }
    protected override void OnMove()
    {
        
    }
    protected override void OnAttack()
    {
        throw new NotImplementedException();
    }
    protected override void OnSkill()
    {
        isWhiteEquipped = false;
    }
    protected override void OnDamaged()
    {
        hp--;
        tweenColor.enabled = true;
        tweenColor.duration = 2.0f;
    }
    protected override void OnDie()
    {
        //TODO: 죽는동안 일어나는 행동들 더 적을것
        currentState = BehaviourState.FINALIZE;
    }
    protected override void OnGoToNextStage()
    {
        throw new NotImplementedException();
    }
    protected override void OnFinalize()
    {
        StopCoroutine(checkEraserCount);
        StopCoroutine(checkState);
        checkEraserCount = null;
        checkState = null;
        isPencilCaseEquipped = false;
        whiteSpawnElapsedTime = 0.0f;
        GameManagement.GetInstance().SetCurrentState(GameManagement.GameState.GAMEOVER);
        gameObject.SetActive(false);
    }

    //Methods
    private void EraserInit()
    {
        erasers = new List<GameObject>();
        eraserPosAxises = new List<Transform>();
        eraserPoses = new List<Transform>();
        weaponAxis = transform.GetChild(0);
        int i = 0;
        do
        {
            eraserPosAxises.Add(weaponAxis.GetChild(i));
            eraserPoses.Add(eraserPosAxises[i].GetChild(0));
            erasers.Add(AddEraser(eraserPoses[i]));
            if (i != 0)
            {
                eraserPosAxises[i].gameObject.SetActive(false);
            }
            i++;
        } while (erasers.Count != PLAYER_ERASER_COUNT_MAX);
        currentEraserCount = 1;
        previousEraserCount = 1;
        eraserSpaceCount = PLAYER_ERASER_COUNT_INIT;
        ReformatEraserPos();
    }
    private void CheckAndGetEraser(Collider2D coll)
    {
        if(coll.CompareTag("Item_Eraser") && currentEraserCount < eraserSpaceCount)
        {
            IncreaseCurrentEraserCount();
        }
    }
    private void CheckAndGetPencilCase(Collider2D coll)
    {
        if (coll.CompareTag("Item_PencilCase"))
        {
            IncreaseEraserSpaceCount();
            isPencilCaseEquipped = true;
            ItemManagement.GetInstance().SetIsPlayerPencilCaseEquipped(true);
        }
    }
    private void CheckHitByEnemy(Collision2D coll)
    {
        if (coll.collider.CompareTag("Enemy"))
        {
            Debug.Log(hp);
            if (hp > 0)
            {
                currentState = BehaviourState.DAMAGED;
            }
            else
            {
                currentState = BehaviourState.DIE;
            }
        }
    }
    private void CheckHitByBullet(Collision2D coll)
    {
        if (coll.collider.CompareTag("Bullet"))
        {
            Debug.Log(hp);
            if (hp > 0)
            {
                currentState = BehaviourState.DAMAGED;
            }
            else
            {
                currentState = BehaviourState.DIE;
            }
        }
    }
    private void IncreaseCurrentEraserCount() { currentEraserCount++; }
    private void DecreaseCurrentEraserCount() { currentEraserCount--; }
    private void IncreaseEraserSpaceCount() { eraserSpaceCount++; }
    private void DecreaseEraserSpaceCount() { eraserSpaceCount--; }
    private void RotateWeaponAxis()
    {
        if (currentState != BehaviourState.DIE || currentState != BehaviourState.INIT)
        {
            weaponAxis.Rotate(new Vector3(0, 0, 1 * PLAYER_WEAPON_ROTATION_SPEED * Time.fixedDeltaTime));
        }
    }
    private void ReformatEraserPos()
    {
        for (int i = 0; i < currentEraserCount; i++)
        {
            eraserPosAxises[i].localRotation = Quaternion.Euler(new Vector3(0, 0, (360 / currentEraserCount) * i));
        }
    }
    private void SetActiveEraser(int index, bool condition)
    {
        eraserPosAxises[index].gameObject.SetActive(condition);
    }
    private void SpawnWhite()
    {
        if(currentState != BehaviourState.INIT || currentState != BehaviourState.DIE)
        {
            if (!isWhiteEquipped && PlayerManagement.GetInstance().GetCurrentState() == GameManagement.GameState.PLAY)
            {
                whiteSpawnElapsedTime += Time.fixedDeltaTime;
                if(whiteSpawnElapsedTime > PLAYER_WHITE_SPAWN_TIME)
                {
                    isWhiteEquipped = true;
                    UIManagement.GetInstance().GetGameSceneBG_white().SetActive(true);
                    whiteSpawnElapsedTime = 0;
                }
            }
        }
    }
    private GameObject AddEraser(Transform position)
    {
        return Instantiate<GameObject>(prefab_eraser, position);
    }
    public void EndDamaged()
    {
        tweenColor.ResetToBeginning();
        tweenColor.enabled = false;
        currentState = BehaviourState.IDLE;
    }

    //Coroutines
    private IEnumerator CheckEraserCount()
    {
        do
        {
            if(previousEraserCount != currentEraserCount)
            {
                if(previousEraserCount > currentEraserCount)
                {
                    SetActiveEraser(previousEraserCount - 1, false);
                }
                else
                {
                    SetActiveEraser(currentEraserCount - 1, true);
                }
                previousEraserCount = currentEraserCount;
                ReformatEraserPos();
            }
            yield return new WaitForEndOfFrame();
        } while (currentState != BehaviourState.DIE);
    }
    protected override IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
            {
                previousState = currentState;
                playerMove.SetCurrentState(currentState);
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
