using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider slider;
    private float progressValue;
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    void Update()
    {
        progressValue = Mathf.Clamp01(GameManager.Instance.DestroyedItems / GameManager.Instance.AllItems);
        slider.value = progressValue;
    }
}
