using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEraserConfig : MonoBehaviour
{

    //Unity Callback Methods
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            gameObject.SetActive(false);

        }
    }
}
