using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagement : Manager<ItemManagement> {

    private GameObject prefab_item_eraser;

    private GameObject item_eraser;
    private Transform uiRoot;

    private bool isPlayerPencilCaseEquipped;
    private int spawnedEraserCount = 0;
    private int checkAttemptCount = 5;
    private float uiRootScale;
    private float currentTime;
    private Vector2 eraserSize = new Vector2(20, 90);
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;

    private const int ERASER_MAX_SPAWNABLE_COUNT = 4;
    private const float ERASER_SPAWN_DELAY = 20.0f;

    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }
    public void SetIsPlayerPencilCaseEquipped(bool condition) { this.isPlayerPencilCaseEquipped = condition; }
   

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }
    private void Update()
    {
        WaitForEraserSpawn();
    }

    private void Init()
    {
        prefab_item_eraser = Resources.Load("Prefabs/Item_Eraser") as GameObject;
        currentState = GameManagement.GameState.INIT;
        previousState = GameManagement.GameState.NULL;
        uiRoot = GameObject.Find("UI Root").transform;
        uiRootScale = uiRoot.lossyScale.x;
        StartCoroutine(CheckState());
    }

    private void EraserInit()
    {
        item_eraser = Instantiate<GameObject>(prefab_item_eraser, uiRoot);
        item_eraser.SetActive(false);
    }
    private void EraserSpawn()
    {
        float _randX, _randY;
        int _count = 0;
        bool isFound = false;
        if (currentState == GameManagement.GameState.PLAY)
        {
            do
            {
                _randX = Random.Range(0 + (eraserSize.x / 2), GameManagement.DEVICE_SCREEN_WIDTH - (eraserSize.x / 2));
                _randY = Random.Range(0 + (eraserSize.y / 2), GameManagement.DEVICE_SCREEN_HEIGHT - (eraserSize.x / 2));
                Vector2 _spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(_randX, _randY));
                RaycastHit2D hit2D = Physics2D.BoxCast(_spawnPos, eraserSize * uiRootScale, 0.0f, Vector2.up);
                Debug.DrawRay(_spawnPos, Vector3.forward, Color.red, 1f);
                if (hit2D)
                {
                    if (hit2D.collider.CompareTag("Enemy") || hit2D.collider.CompareTag("Bullet") || hit2D.collider.CompareTag("Player") || hit2D.collider.CompareTag("Eraser"))
                    {
                        _count++;
                    }
                    else
                    {
                        item_eraser.transform.position = _spawnPos;
                        item_eraser.SetActive(true);
                        isFound = true;
                        spawnedEraserCount++;
                    }
                }
                else
                {
                    item_eraser.transform.position = _spawnPos;
                    item_eraser.SetActive(true);
                    isFound = true;
                    spawnedEraserCount++;
                }
            } while (!isFound && _count < checkAttemptCount);
        }
    }
    private void WaitForEraserSpawn()
    {
        if (currentState == GameManagement.GameState.PLAY)
        {
            if (!item_eraser.activeSelf)
            {
                if(spawnedEraserCount < ERASER_MAX_SPAWNABLE_COUNT)
                {
                    if(currentTime < ERASER_SPAWN_DELAY)
                    {
                        currentTime += Time.deltaTime;
                    }   
                    else
                    {
                        EraserSpawn();
                        currentTime = 0;
                    }
                }
                else if(spawnedEraserCount < ERASER_MAX_SPAWNABLE_COUNT && isPlayerPencilCaseEquipped)
                {
                    if (currentTime < ERASER_SPAWN_DELAY)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else
                    {
                        EraserSpawn();
                        currentTime = 0;
                    }
                }
            }
        }
    }
    private void RemoveEraser()
    {
        spawnedEraserCount = 0;
        currentTime = 0;
        Destroy(item_eraser);
        item_eraser = null;
        isPlayerPencilCaseEquipped = false;
    }

    //Coroutines
    private IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
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
                        EraserInit();
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
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        RemoveEraser();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }
}
