using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActiavtor : MonoBehaviour
{
    public GameObject[] objectsToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject item in objectsToActivate)
            {
                item.SetActive(true);
            }
        }
    }
}
