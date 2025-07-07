using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Rigidbody rb;
    private CapsuleCollider col;

    public bool isCutted;

    private void Start()
    {
        rb = player.transform.GetComponent<Rigidbody>();
        col = player.transform.GetComponentInChildren<CapsuleCollider>();
    }

    public void BeDestroyed()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.cut);

        isCutted = true;
        rb.useGravity = true;
        col.enabled = false;
        this.gameObject.SetActive(false);
    }
}
