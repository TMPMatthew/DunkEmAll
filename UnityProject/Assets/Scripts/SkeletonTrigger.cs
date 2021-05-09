using UnityEngine;
using RootMotion.Dynamics;

namespace RootMotion.Demos
{
    public class SkeletonTrigger : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 9)
            {
                GameManager.instance.SkeletonMem(collision);
            }
        }
    }
}

