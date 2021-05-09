using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lean.Pool
{
    public class BallCollider : MonoBehaviour, IPoolable
    {
        int temp = 0;
        int temp2 = 0;

        GameObject lastCollided;

        private void Start()
        {
            lastCollided = this.gameObject;
        }

        private void OnCollisionEnter(Collision collision)
        {
           
            Debug.Log(temp);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce((-transform.position + collision.transform.position) * 20, ForceMode.Impulse);
                if (lastCollided != collision.transform.root.gameObject) {

                    lastCollided = collision.transform.root.gameObject;
                    RagdollHelper helper = collision.transform.root.GetComponent<RagdollHelper>();

                    if (helper.DeathDone) return;

                    helper.ragdolled = true;
                    helper.player.hites++;
                    if (helper.player.hites >= helper.player.lives) { helper.player.ChangeMat(); GameManager.instance.SlowMo(); };
                }

                if (temp == 0)
                {
                    var particle = Instantiate(GameManager.instance.hitParticle, collision.transform.position, Quaternion.identity);
                }
                temp++;
                return;
            }

            if (collision.gameObject.CompareTag("Head"))
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce((-transform.position + collision.transform.position) * 10, ForceMode.Impulse);

                if (lastCollided != collision.transform.root.gameObject)
                {
                    lastCollided = collision.transform.root.gameObject;
                    RagdollHelper helper = collision.transform.root.GetComponent<RagdollHelper>();

                    if (helper.DeathDone) return;

                    helper.ragdolled = true;
                    helper.player.hites = 3 + helper.player.hites;
                    if (helper.player.hites >= helper.player.lives) { helper.player.ChangeMat(); GameManager.instance.SlowMo(); };
                }

                if (temp == 0)
                {
                    var particle = Instantiate(GameManager.instance.headParticle, collision.transform.position, Quaternion.identity);

                }
                temp++;
                return;
            }

            if (collision.gameObject.layer == 11)
            {
                
                if (temp2 == 0) GameManager.instance.SkeletonMem(collision);
                temp2++;
                return;
            }

        }

        public void OnSpawn()
        {

        }

        public void OnDespawn()
        {
            temp = 0;
            temp2 = 0;
        }


    }
}
