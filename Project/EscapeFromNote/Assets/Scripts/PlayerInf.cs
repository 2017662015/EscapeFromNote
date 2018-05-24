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
    private List<GameObject> erasers = new List<GameObject>();
    private List<Transform> eraserPoses = new List<Transform>();

    //Variables
    private int skillCount;
    private int eraserCount = 0;
    private int previousEraserCount = 0;

    //Constants 
    private const int PLAYER_HP = 7;
    private const int PLAYER_SKILLCNT = 3;
    private const int PLAYER_ERASERINIT = 3;
    private const int PLAYER_ERASERMAX = 4;
    private const float MOVE_SPEED = 1.5f;
    private const float ROTATION_SPEED = 100.0F;
    public const float ITEM_REMAIN_LENGTH = 60.0f;

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
    protected override void OnCollisionEnter(Collision coll)
    {
        base.OnCollisionEnter(coll);
        Debug.Log(coll.collider.tag);
        //TODO: 추후 충돌처리 구현 예정
        if (coll.collider.tag == "Enemy")
        {
            if (hp <= 0)
            {
                currentState = BehaviourState.DIE;
            }
            else
            {
                currentState = BehaviourState.DAMAGED;
            }
        }
        if (coll.collider.tag == "Item_Eraser" && eraserCount < PLAYER_ERASERMAX)
        {
            IncreaseEraserCount();
        }
    }

    //Initialize Method of this script
    private void Init()
    {
        prefab_eraser = Resources.Load("Prefabs/Eraser") as GameObject;
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
    }
    protected override void OnDie()
    {
        //TODO: Player가 Die상태일때 동작할 Function을 구현해주세요.   
    }

    //Methods   
    private void EraserInit()
    {
        int i = 0;
        eraserPosAxis = gameObject.transform.GetChild(0);
        do
        {
            eraserPoses.Add(eraserPosAxis.GetChild(i));
            erasers.Add(AddEraser(eraserPoses[i]));
            eraserPoses[i].GetComponent<EraserPosesBehaviour>().enabled = true;
            eraserPoses[i].GetComponent<EraserPosesBehaviour>().SetPlayerPos(transform);
            i++;
        } while (erasers.Count != PLAYER_ERASERMAX);
        eraserPoses[PLAYER_ERASERINIT].gameObject.SetActive(false);
        eraserCount = i - 1;
        previousEraserCount = eraserCount;
    }
    private void ResetEraserFormation()
    {
        Debug.Log("Reformatting Formation");
    }
    private void SetActiveEraser(int index, bool condition)
    {
        eraserPoses[index].gameObject.SetActive(condition);
        if(condition)
        {
            eraserPoses[index].GetComponent<EraserPosesBehaviour>().SetPlayerPos(transform);
        }
    }
    private void RotatePosEraserAxis()
    {
        eraserPosAxis.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * ROTATION_SPEED)); 
    }
    public void IncreaseEraserCount() { eraserCount++; }
    public void DecreaseEraserCount() { eraserCount--; }
    private GameObject AddEraser(Transform position)
    {
        return Instantiate<GameObject>(prefab_eraser, position);
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
                    case BehaviourState.DAMAGED:
                        OnDamaged();
                        break;
                    case BehaviourState.DIE:
                        OnDie();
                        break;
                }
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
