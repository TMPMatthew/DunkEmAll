using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RootMotion.Demos {
    public class EnemyMain : MonoBehaviour
    {
        public int heath = 3;

        private void Start()
        {
            CameraFollow.instance.rivalsPos.Add(this.transform);
            GetComponent<UserControlAI>().moveTarget = GameManager.instance.player;
        }
    }
}