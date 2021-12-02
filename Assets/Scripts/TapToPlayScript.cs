using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlayScript : MonoBehaviour
{

    void Update()
    {
        if (Input.touchCount > 0)
        {
            GameManager.Instance.GameStarted = true;
            gameObject.SetActive(false);
        }
    }
}
