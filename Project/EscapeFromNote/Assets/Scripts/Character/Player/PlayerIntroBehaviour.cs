using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroBehaviour : MonoBehaviour
{
    private GameObject prefab_eraser;
    private Transform weaponAxis;
    private Coroutine checkEraserCount;
    private List<GameObject> erasers;
    private List<Transform> eraserPosAxises;
    private List<Transform> eraserPoses;

    [SerializeField]
    [Range(0, 4)]
    private int currentEraserCount = 0;
    private int previousEraserCount = 0;
    [SerializeField]
    [Range(3, 4)]
    private int eraserSpaceCount = 0;

    private const int PLAYER_ERASER_COUNT_INIT = 3;
    private const int PLAYER_ERASER_COUNT_MAX = 4;
    private const float PLAYER_WEAPON_ROTATION_SPEED = 500.0f;

    private void OnEnable()
    {
        Init();
    }
    private void Init()
    {
        prefab_eraser = Resources.Load("Prefabs/Eraser_UI") as GameObject;
        EraserInit();
        checkEraserCount = StartCoroutine(CheckEraserCount());
        IncreaseEraserSpaceCount();
    }
    private void FixedUpdate()
    {
        RotateWeaponAxis();
    }
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
            i++;
        } while (erasers.Count != PLAYER_ERASER_COUNT_MAX);
        currentEraserCount = 4;
        previousEraserCount = 4;
        eraserSpaceCount = PLAYER_ERASER_COUNT_MAX;
        ReformatEraserPos();
    }
    private void IncreaseCurrentEraserCount() { currentEraserCount++; }
    private void DecreaseCurrentEraserCount() { currentEraserCount--; }
    private void IncreaseEraserSpaceCount() { eraserSpaceCount++; }
    private void DecreaseEraserSpaceCount() { eraserSpaceCount--; }
    private void SetActiveEraser(int index, bool condition)
    {
        eraserPosAxises[index].gameObject.SetActive(condition);
    }
    private GameObject AddEraser(Transform position)
    {
        return Instantiate<GameObject>(prefab_eraser, position);
    }
    private void RotateWeaponAxis()
    {
        weaponAxis.Rotate(new Vector3(0, 0, 1 * PLAYER_WEAPON_ROTATION_SPEED * Time.fixedDeltaTime));
    }
    private void ReformatEraserPos()
    {
        for (int i = 0; i < currentEraserCount; i++)
        {
            eraserPosAxises[i].localRotation = Quaternion.Euler(new Vector3(0, 0, (360 / currentEraserCount) * i));
        }
    }
    private IEnumerator CheckEraserCount()
    {
        do
        {
            if (previousEraserCount != currentEraserCount)
            {
                if (previousEraserCount > currentEraserCount)
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
        } while (true);
    }
}
