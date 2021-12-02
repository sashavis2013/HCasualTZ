using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickScript : MonoBehaviour, IPointerClickHandler
{
    public MovementController MovementController;
    private float lastTimeClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        MovementController.DoJumpDown();
        float currentTimeClick = eventData.clickTime;

        if (Mathf.Abs(currentTimeClick - lastTimeClick) < 0.5f)
        {
            MovementController.DoJump();
        }

        lastTimeClick = currentTimeClick;
    }
}
