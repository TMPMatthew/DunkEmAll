using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RootMotion.Demos
{
    public class Activation : MonoBehaviour
    {
        public List<Player> players = new List<Player>();


        private void Start()
        {
            CameraFollow.instance.levelCheckPoints.Add(this.transform);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("player is here");

                for(int i = 0; i < players.Count; i++)
                {
                    CameraFollow.instance.rivalsPos.Add(players[i].transform);
                    players[i].enable = true;
                }

                for(int i = 0; i< CameraFollow.instance.levelCheckPoints.Count; i++)
                {
                    if (CameraFollow.instance.levelCheckPoints[i] == this.transform)
                        CameraFollow.instance.levelCheckPoints.RemoveAt(i);
                }

                Destroy(gameObject);

            }
        }

        [ContextMenu("Add enemies")]
        public void addEnemies()
        {
            players.Clear();
            Component[] trans = GetComponentsInChildren(typeof(Player));

            //For each of the components in the array, treat the component as a Rigidbody and set its isKinematic property
            foreach (Component c in trans)
            {
                players.Add((c as Player));
            }
        }

    }
}