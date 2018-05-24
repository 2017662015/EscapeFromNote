using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserInf : MonoBehaviour { 
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "Enemy")
        {
            gameObject.GetComponentInParent<PlayerInf>().DecreaseEraserCount();
        }
    }
}
    