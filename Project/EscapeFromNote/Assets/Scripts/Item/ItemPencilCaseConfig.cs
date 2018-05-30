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
            gameObject.SetActive(false);
        }
    }
    private void OnDisabled()
    {
        isAssumed = false;
    }
}
