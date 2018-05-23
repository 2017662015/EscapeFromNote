using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEraserTest : MonoBehaviour {
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
