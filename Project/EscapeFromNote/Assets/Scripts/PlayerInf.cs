using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInf : Character
{

    //Prefab Instances
    private GameObject prefab_eraser;

    //Instance
    private Coroutine checkState;
    private Coroutine checkEraserCount;
    private Transform eraserPosAxis;
    private PlayerMove playerMove;
    private List<GameObject> erasers = new List<GameObject>();
    private List<Transform> axises = new List<Transform>();
    private List<Transform> eraserPoses = new List<Transform>();

    //Variables
    private int skillCount = 0;
    private int eraserCount = 0;
    private int availableEraserCount = 0;
    private int previousEraserCount = 0;
    private float currentTime = 0;

    //Constants 
    private const int PLAYER_HP = 7;
    private const int PLAYER_SKILLCNT = 3;
    private const int PLAYER_ERASERINIT = 3;
    private const int PLAYER_ERASERMAX = 4;
    private const float ERASER_SPAWN_REMAIN_TIME = 30.0f;
    private const float MOVE_SPEED = 1.5f;
    private const float ROTATION_SPEED = 100.0F;
    public const float ITEM_REMAIN_LENGTH = 60.0f;

    public void SetCurrentState(BehaviourState state) { currentState = state; }

    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    private void LateUpdate()
    {
        if(currentState != BehaviourState.DIE || currentState != BehaviourState.INIT)
        {
            RotatePosEraserAxis();
        }
    }
    private void FixedUpdate()
    {
        if(eraserCount < availableEraserCount)
        {
            currentTime += Time.fixedDeltaTime;
            if(currentTime > ERASER_SPAWN_REMAIN_TIME)
            {
                IncreaseEraserCount();
                currentTime = 0;
            }
        }
    }
    protected override void OnCollisionEnter(Collision coll)
    {
        base.OnCollisionEnter(coll);
        Debug.Log(coll.collider.tag);
        //TODO: 추후 충돌처리 구현 예정
        if (coll.collider.tag == "Enemy")
        {
            if (hp > 1)
            {
                currentState = BehaviourState.DAMAGED;
            }
            else
            {
                currentState = BehaviourState.DIE;
            }
        }
        if (coll.collider.tag == "Item_PencilCase" && availableEraserCount < PLAYER_ERASERMAX)
        {
            IncreaseAvailableEraserCount();
            IncreaseEraserCount();
        }
    }

    //Initialize Method of this script
    private void Init()
    {
        prefab_eraser = Resources.Load("Prefabs/Eraser") as GameObject;
        playerMove = gameObject.GetComponent<PlayerMove>();
        currentState = BehaviourState.INIT;
        previousState = BehaviourState.DIE;
        checkState = StartCoroutine(CheckState());
    }

    //State Machine Callbacks
    protected override void OnInit()
    {
        //TODO: Player 생성시 동작할 Function을 구현해 주세요.
        hp = PLAYER_HP;
        EraserInit();
        checkEraserCount = StartCoroutine(CheckEraserCount());
        currentState = BehaviourState.IDLE;
    }
    protected override void OnIdle()
    {
        //TODO: Player가 Idle 상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnMove()
    {
        //TODO: Player가 Move 상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnAttack()
    {
        //TODO: Player가 Attack상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnDamaged()
    {
        //TODO: Player가 Damage를 받은 상태일때 동작할 Function을 구현해주세요.
        hp--;
        Debug.Log(hp);
        currentState = BehaviourState.IDLE;
    }
    protected override void OnDie()
    {
        //TODO: Player가 Die상태일때 동작할 Function을 구현해주세요.
        hp--;
        gameObject.SetActive(false);
    }
    protected override void OnNextStage()
    {
        
    }

    //Methods   
    private void EraserInit()
    {
        int i = 0;
        eraserPosAxis = gameObject.transform.GetChild(0);
        do
        {
            axises.Add(eraserPosAxis.GetChild(i));
            eraserPoses.Add(axises[i].GetChild(0));
            erasers.Add(AddEraser(eraserPoses[i]));
            i++;
        } while (erasers.Count != PLAYER_ERASERMAX);
        eraserPoses[PLAYER_ERASERINIT].gameObject.SetActive(false);
        eraserCount = i - 1;
        previousEraserCount = eraserCount;
        availableEraserCount = PLAYER_ERASERINIT;
        ResetEraserFormation();
    }
    private void ResetEraserFormation()
    {
        for(int i = 0; i < eraserCount; i++)
        {
            axises[i].localRotation = Quaternion.Euler(new Vector3(0, 0, (360 / eraserCount) * i));
        }
    }
    private void SetActiveEraser(int index, bool condition)
    {
        eraserPoses[index].gameObject.SetActive(condition);
    }
    private void RotatePosEraserAxis()
    {
        eraserPosAxis.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * ROTATION_SPEED)); 
    }
    private void IncreaseAvailableEraserCount() { availableEraserCount++; }
    private void DecreaseAvailableEraserCount() { availableEraserCount--; }
    private GameObject AddEraser(Transform position)
    {
        return Instantiate<GameObject>(prefab_eraser, position);
    }

    public void IncreaseEraserCount() { eraserCount++; }
    public void DecreaseEraserCount() { eraserCount--; }
    
    
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
                    case BehaviourState.DAMAGED:
                        OnDamaged();
                        break;
                    case BehaviourState.DIE:
                        OnDie();
                        break;
                    case BehaviourState.NEXTSTAGE:
                        OnNextStage();
                        break;
                }
                playerMove.SetCurrentState(currentState);
            }
            yield return new WaitForEndOfFrame();
        } while (true);
    }
    private IEnumerator CheckEraserCount()
    {
        do
        {
            if (previousEraserCount != eraserCount)
            {
                if(previousEraserCount > eraserCount)
                {
                    SetActiveEraser(previousEraserCount - 1, false);
                }
                else
                {
                    SetActiveEraser(eraserCount - 1, true);
                }
                previousEraserCount = eraserCount;
                ResetEraserFormation();
            }
            yield return new WaitForEndOfFrame();
        } while (true);
    }
}
