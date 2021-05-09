using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform target;
    public bool enable = true;
    public int lives = 3;
    public int hites = 0;
    public float speed = 1;

    SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = transform.GetChild(1).transform.GetComponent<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        target = GameManager.instance.player;
    }

    public void ChangeMat()
    {
        skinnedMeshRenderer.material = GameManager.instance.deathMat;
        GameManager.instance.FillUlt();
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    void OnAnimatorMove()
    {
        if (animator.enabled && enable)
        {
            //move
            transform.position += transform.forward * speed * Time.deltaTime;

            //look
            Vector3 lTargetDir = target.position - transform.position;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 3);
        }
    }

}
