using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ParticleSystem hitParticle;
    public ParticleSystem headParticle;

    public List<GameObject> enemies = new List<GameObject>();
    //gavno-------------
    private MuscleRemoveMode removeMuscleMode;
    private LayerMask layers;
    private float unpin = 10f;
    private float force = 10f;
    public ParticleSystem particles;
    public Material deathMat;
    private BallShooter vcumHands;
    public Transform player;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        vcumHands = GameObject.Find("hands").GetComponent<BallShooter>();
        //о за прикол?
       // Time.timeScale = 0.0001f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SkeletonMem(Collision collision)
    {

        var broadcaster = collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();

        // If is a muscle...
        if (broadcaster != null)
        {
            broadcaster.Hit(unpin, (collision.transform.position - transform.position) * force, collision.transform.position);

            // Remove the muscle and its children
            broadcaster.puppetMaster.RemoveMuscleRecursive(broadcaster.puppetMaster.muscles[broadcaster.muscleIndex].joint, true, true, removeMuscleMode);
        }
        else
        {
            // Not a muscle (any more)
            //var joint = hit.collider.attachedRigidbody.GetComponent<ConfigurableJoint>();
            //if (joint != null) Destroy(joint);

            // Add force
            collision.collider.attachedRigidbody.AddForceAtPosition((collision.transform.position - transform.position) * force, collision.transform.position);
        }

        // Particle FX
        particles.transform.position = collision.transform.position;
        particles.transform.rotation = Quaternion.LookRotation(-(collision.transform.position - transform.position));
        particles.Emit(5);


    }

    public void FillUlt()
    {
        vcumHands.explodingBallUltimateFill += 0.1f;
    }


    public void onActivateEnemies (Collider collider)
    {
        //collision.

    }

    public void StartTheGame()
    {
        Time.timeScale = 1f;
    }

    //---------------slow mo sheat (matvey, ne bey)-----------------//

    public void SlowMo()
    {
        StopCoroutine(startSlowMo());
        Time.timeScale = 1f;
        StartCoroutine(startSlowMo());

    }
    IEnumerator startSlowMo()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }

    public void StopSlowMo()
    {
        StopCoroutine(startSlowMo());
        Time.timeScale = 1f;
    }
}



