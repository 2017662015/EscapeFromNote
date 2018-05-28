using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PensTest : MonoBehaviour {
    public void OnClicked()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
