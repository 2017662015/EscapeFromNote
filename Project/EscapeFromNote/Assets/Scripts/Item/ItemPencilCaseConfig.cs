using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPencilCaseConfig : MonoBehaviour
{
    //Variables
    private bool isAssumed;

    //Unity Callback Methods
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player") && !isAssumed)
        {
            isAssumed = true;
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(CheckGameEnds());
    }
    private void OnDisabled()
    {
        isAssumed = false;
    }
    private IEnumerator CheckGameEnds()
    {
        do
        {
            yield return new WaitForEndOfFrame();
        } while (GameManagement.GetInstance().GetCurrentState() != GameManagement.GameState.GAMEOVER);
        Destroy(this.gameObject);
    }
}
