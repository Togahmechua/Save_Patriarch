using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Rigidbody rb;
    private CapsuleCollider col;

    private void Start()
    {
        rb = player.transform.GetComponent<Rigidbody>();
        col = player.transform.GetComponentInChildren<CapsuleCollider>();
    }

    public void BeDestroyed()
    {
        rb.useGravity = true;
        col.enabled = false;
        Destroy(this.gameObject);
    }
}
