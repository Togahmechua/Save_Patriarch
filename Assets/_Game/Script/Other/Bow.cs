using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Settings")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float launchForce = 10f;

    [Header("Trajectory")]
    public GameObject pointPrefab;
    public int numberOfPoints = 30;
    public float timeStep = 0.05f;

    private GameObject[] points;

    void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(pointPrefab, shootPoint.position, Quaternion.identity);
            points[i].SetActive(false);
            points[i].transform.parent = this.transform;
        }
    }

    public void ShowTrajectory(Vector3 direction, float launchForce)
    {
        // Xoay quanh trục Z theo hướng direction (trong mặt phẳng XY)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 velocity = direction.normalized * launchForce / arrowPrefab.GetComponent<Rigidbody>().mass;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i * timeStep;
            Vector3 pos = shootPoint.position + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i].transform.position = pos;
            points[i].SetActive(true);
        }
    }


    public void Shoot(Vector3 direction, float launchForce, GameObject owner)
    {
        if (!Level.currentLevel.canShoot) return;
        AudioManager.Ins.PlaySFX(AudioManager.Ins.shoot);

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

        Arrow arr = arrow.GetComponent<Arrow>();
        if (arr != null)
            arr.owner = owner;

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(direction.normalized * launchForce, ForceMode.Impulse);

        // Trừ đạn NGAY KHI BẮN
        Level.currentLevel.DecreaseArrow();
        UIManager.Ins.mainCanvas.StartCountdown();
    }



    public void HideTrajectory()
    {
        foreach (var point in points)
        {
            point.SetActive(false);
        }
    }
}
