using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallExplode : MonoBehaviour
{
    public float delay = 1f;
    private GameObject lastCollided;

    void Start()
    {
        StartCoroutine(Destruct());
        lastCollided = this.gameObject;
    }

    private IEnumerator Destruct()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.AddForce((-transform.position + collision.transform.position) * 40, ForceMode.Impulse);
            if (lastCollided != collision.transform.root.gameObject)
            {

                lastCollided = collision.transform.root.gameObject;
                RagdollHelper helper = collision.transform.root.GetComponent<RagdollHelper>();

                if (helper.DeathDone) return;

                helper.ragdolled = true;
                if (helper.player.hites >= helper.player.lives) { helper.player.ChangeMat(); GameManager.instance.SlowMo(); };
            }

        }
    }
}