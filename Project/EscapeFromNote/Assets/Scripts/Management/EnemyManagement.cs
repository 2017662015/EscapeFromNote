using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManagement : Manager<EnemyManagement> {
    //Enums
    private enum EnemyType { NULL, PENCIL = 1, BALLPEN, FOUNTAINPEN }

    //Prefab Isntances
    private GameObject prefab_pencil;
    private GameObject prefab_ballPen;
    private GameObject prefab_fountainPen;

    //Instances
    private Vector2 enemySize = new Vector2(1, 1);
    private Transform uiRoot;
    private StageManagement stageManagement;
    private List<GameObject> enemys;
    private List<GameObject> disabledEnemys;
    private Vector2 spawnPos;

    //Variables
    private bool isPencilCaseSetted;
    private int currentStage;
    private int enemyCountOfStage = 0;
    private int checkAttemptCount = 5;
    [SerializeField][Range(0.0f, 10.0f)]private float spawnDelay = 3.0f;
    private float delayFactor = 0.0f;
    private EnemyType currentEnemyType;
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;
    private Character.BehaviourState currentBehaviourState;
    private Character.BehaviourState previousBehaviourState;

    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }
    public void SetCurrentBehaviourState(Character.BehaviourState state) { this.currentBehaviourState = state; }

    private void Update()
    {
        WaitForEnemySpawn();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(spawnPos, Camera.main.ScreenToWorldPoint(enemySize));
        Debug.Log(Camera.main.ScreenToWorldPoint(enemySize));
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    //Initialize Method of this class
    private void Init()
    {
        prefab_pencil = Resources.Load("Prefabs/Pencil") as GameObject;
        prefab_ballPen = Resources.Load("Prefabs/BallPen") as GameObject;
        prefab_fountainPen = Resources.Load("Prefabs/FountainPen") as GameObject;
        uiRoot = GameObject.Find("UI Root").transform;
        stageManagement = StageManagement.GetInstance();
        currentState = GameManagement.GameState.INIT;
        previousState = GameManagement.GameState.NULL;
        StartCoroutine(CheckState());
        StartCoroutine(CheckBehaviourState());
    }

    //BehaviourState State Machine Callback Methods
    private void OnGoToNextStage()
    {
        enemyCountOfStage += GetEnemyCountOfStage();
        MakeEnemys(enemyCountOfStage, currentStage);
        currentBehaviourState = Character.BehaviourState.IDLE;
    }
    private void OnGameOver()
    {
        EnemyDestroy();
    }

    //Methods
    private void MakeEnemys(int count, int stage)
    {
        //TODO: This method was hardcoded. Need to be refactored.
        if (enemys == null)
        {
            enemys = new List<GameObject>();
            disabledEnemys = new List<GameObject>();
        }
        stage = Mathf.Clamp(stage, 0, 3);
        int i = 0;
        if (stage > 0)
        {
            do
            {
                if (i < stage)
                {
                    i++;
                }
                else
                {
                    i = 1;
                }
                currentEnemyType = (EnemyType)i;
                switch (currentEnemyType)
                {
                    case EnemyType.PENCIL:
                        InstantiateEnemy(prefab_pencil, uiRoot);
                        break;
                    case EnemyType.BALLPEN:
                        InstantiateEnemy(prefab_ballPen, uiRoot);
                        break;
                    case EnemyType.FOUNTAINPEN:
                        InstantiateEnemy(prefab_fountainPen, uiRoot);
                        break;
                }
            } while (enemys.Count != enemyCountOfStage);
        }
    }
    private void EnemySpawn()
    {
        float _randX, _randY;
        int _count = 0;
        bool isFound = false;
        if (currentState == GameManagement.GameState.PLAY)
        {
            do
            {
                _randX = Random.Range(0 + enemySize.x, GameManagement.DEVICE_SCREEN_WIDTH - enemySize.x);
                _randY = Random.Range(0 + enemySize.y, GameManagement.DEVICE_SCREEN_HEIGHT - enemySize.y);
                spawnPos = UICamera.mainCamera.ScreenToWorldPoint(new Vector2(_randX, _randY));
                RaycastHit2D hit2D = Physics2D.BoxCast(spawnPos, UICamera.mainCamera.ScreenToWorldPoint(enemySize), 0.0f, Vector3.forward, Mathf.Infinity);
                Debug.Log(UICamera.mainCamera.ScreenToWorldPoint(enemySize) * 2);
                Debug.DrawRay(spawnPos, Vector3.forward, Color.red, 1f);
                if (hit2D)
                {
                    Debug.Log(hit2D.collider.tag);
                    if (hit2D.collider.CompareTag("Enemy") || hit2D.collider.CompareTag("Bullet") || hit2D.collider.CompareTag("Player") || hit2D.collider.CompareTag("Eraser") || hit2D.collider.CompareTag("Wall"))
                    {
                        _count++;
                    }
                    else
                    {
                        disabledEnemys[0].transform.position = spawnPos;
                        disabledEnemys[0].SetActive(true);
                        disabledEnemys.RemoveAt(0);
                        isFound = true;
                    }
                }
                else
                {
                    disabledEnemys[0].transform.position = spawnPos;
                    disabledEnemys[0].SetActive(true);
                    disabledEnemys.RemoveAt(0);
                    isFound = true;
                }
            } while(!isFound && _count < checkAttemptCount);
        }
    }
    private void WaitForEnemySpawn()
    {
        if(currentState == GameManagement.GameState.PLAY)
        {
            if(delayFactor < spawnDelay)
            {
                delayFactor += Time.deltaTime;
            }
            else
            {
                EnemySpawn();
                delayFactor = 0;
            }
        }
    }
    private void EnemyDestroy()
    {
        do
        {
            Destroy(enemys[0]);
            enemys.RemoveAt(0);
        } while (enemys.Count != 0);
        enemys = null;
        disabledEnemys = null;
        delayFactor = 0;
        currentEnemyType = EnemyType.NULL;
        currentStage = 0;
        enemyCountOfStage = 0;
        isPencilCaseSetted = false;
    }
    private void InstantiateEnemy(GameObject prefab_enemy, Transform parentTr)
    {
        GameObject _enemy;
        _enemy = Instantiate<GameObject>(prefab_enemy, parentTr);
        if (Random.Range(0.0f, 1.0f) > 0.8f && !isPencilCaseSetted)
        {
            isPencilCaseSetted = true;
            _enemy.GetComponent<EnemyBehaviour>().SetIsPencilCaseContained(true);
        }
        _enemy.SetActive(false);
        enemys.Add(_enemy);
        disabledEnemys.Add(_enemy);
    }
    public void CallSkillEnabled() { StartCoroutine(SkillEnabled()); }

    private int GetEnemyCountOfStage()
    {
        return (int)(StageManagement.STAGE_INTERVAL_TIME / spawnDelay);
    }
    //Coroutines
    private IEnumerator CheckState()
    {
        do
        {
            if(previousState != currentState)
            {
                previousState = currentState;
                switch (currentState)
                {
                    case GameManagement.GameState.INIT:
                        break;
                    case GameManagement.GameState.TITLE:
                        break;
                    case GameManagement.GameState.OPTION_TITLE:
                        break;
                    case GameManagement.GameState.INIT_PLAY:
                        StartCoroutine(CheckStage());
                        break;
                    case GameManagement.GameState.PLAY:
                        break;
                    case GameManagement.GameState.PAUSE:
                        break;
                    case GameManagement.GameState.OPTION_PAUSE:
                        break;
                    case GameManagement.GameState.RESUME:
                        break;
                    case GameManagement.GameState.GAMEOVER:
                        OnGameOver();
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        break;
                    case GameManagement.GameState.FINALIZE:
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
            if(currentStage != stageManagement.GetStage())
            {
                currentStage = stageManagement.GetStage();
                currentBehaviourState = Character.BehaviourState.GO_TO_NEXT_STAGE;
            }
            yield return null;
        } while (currentState != GameManagement.GameState.GAMEOVER);
    }
    private IEnumerator CheckBehaviourState()
    {
        do
        {
            if (previousBehaviourState != currentBehaviourState)
            {
                previousBehaviourState = currentBehaviourState;
                switch (currentBehaviourState)
                {
                    case Character.BehaviourState.IDLE:
                        break;
                    case Character.BehaviourState.GO_TO_NEXT_STAGE:
                        OnGoToNextStage();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }
    private IEnumerator SkillEnabled()
    {
        int i = 0;
        do
        {
            if (enemys[i].activeSelf)
            {
                enemys[i].GetComponent<EnemyBehaviour>().CallClearAllBullet();
            }
            i++;
            yield return null;
        } while (i < enemys.Count);
    }
}
