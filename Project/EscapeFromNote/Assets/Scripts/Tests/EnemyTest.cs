using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour {

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "Eraser")
        {
            gameObject.SetActive(false);
        }
    }
}
