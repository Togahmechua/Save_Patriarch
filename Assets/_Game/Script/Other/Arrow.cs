using System;
using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit;
    public GameObject owner;

    private Coroutine despawnCoroutine;

    public static event Action<Arrow> OnArrowDespawned;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        despawnCoroutine = StartCoroutine(ForceDespawnAfterTime(3f));
    }

    private void Update()
    {
        if (!hasHit)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Ground"))
        {
            hasHit = true;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            StartCoroutine(IEDespawn(1f));
            return;
        }

        if (other.CompareTag("Body"))
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.pain);

            hasHit = true;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            StopDespawnTimer();

            Debug.Log("Minus time");

            // ✅ Trừ từ thời gian hiện tại chứ không phải timer ban đầu
            int curTime = UIManager.Ins.mainCanvas.CurrentTime;
            UIManager.Ins.mainCanvas.DecreaseTime(5);

            StartCoroutine(IEDespawn(1f));
            return;
        }


        Rope rope = Cache.GetRope(other);
        if (rope != null)
        {
            hasHit = true;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            StopDespawnTimer();
            rope.BeDestroyed();
            DespawnNow();
        }
    }

    private IEnumerator IEDespawn(float time)
    {
        yield return new WaitForSeconds(time);
        DespawnNow();
    }

    private IEnumerator ForceDespawnAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!hasHit)
        {
            hasHit = true;
            DespawnNow();
        }
    }

    private void StopDespawnTimer()
    {
        if (despawnCoroutine != null)
        {
            StopCoroutine(despawnCoroutine);
            despawnCoroutine = null;
        }
    }

    private void DespawnNow()
    {
        OnArrowDespawned?.Invoke(this);
        Destroy(gameObject);
    }
}
