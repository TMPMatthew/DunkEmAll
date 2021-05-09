using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosiveBall : MonoBehaviour
{
    public GameObject explosionPrefab;

    public Button explodeBTN;

    private Animator handAnimator;

    private void Start()
    {
        handAnimator = GameObject.Find("hands").GetComponent<Animator>();
        explodeBTN = GameObject.Find("ExplodeButton").GetComponent<Button>();
        explodeBTN.onClick.AddListener(() => ExplodeBall());
        handAnimator.SetBool("ballExplosive", true);
    }

    public void ExplodeBall()
    {
        handAnimator.SetBool("ballExplosive", false);
        handAnimator.SetTrigger("ButtonPress");
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        handAnimator.gameObject.GetComponent<BallShooter>().explosiveBallSpawned = false;
        Destroy(this.gameObject);
    }

}
