using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public Bow bow;
    public float maxDragDistance = 3f;
    public float maxLaunchForce = 15f;

    private Vector3 startDragPos;


    #region Drag Control
    public void OnPointerDown(PointerEventData eventData)
    {
        LevelManager.Ins.level.isTouch = true;
        startDragPos = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 direction = startDragPos - currentPos;

        float dragDistance = Mathf.Clamp(direction.magnitude, 0, maxDragDistance);
        float forcePercent = dragDistance / maxDragDistance;
        float launchForce = maxLaunchForce * forcePercent;

        bow.ShowTrajectory(direction, launchForce);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 endDragPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 direction = startDragPos - endDragPos;

        float dragDistance = Mathf.Clamp(direction.magnitude, 0, maxDragDistance);
        float forcePercent = dragDistance / maxDragDistance;
        float launchForce = maxLaunchForce * forcePercent;

        bow.Shoot(direction, launchForce, this.gameObject);

        bow.HideTrajectory();
    }

    #endregion
}
