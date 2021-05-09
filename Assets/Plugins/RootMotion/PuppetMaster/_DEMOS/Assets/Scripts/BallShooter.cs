using UnityEngine;
using System.Collections;
using Lean.Pool;
using UnityEngine.UI;

// Just shooting objects from the camera towards the mouse position.
public class BallShooter : MonoBehaviour
{

    public KeyCode keyCode = KeyCode.Mouse0;
    public GameObject ball;
    [Space]


    public bool isBallExplosive;

    [HideInInspector]
    public bool explosiveBallSpawned = false;

    [Range(0f, 1f)]
    public float explodingBallUltimateFill;

    public GameObject ExplodingBall;
    public Button explodeBTN;
    public Button switchBTN;
    public Image fillBTN;
    public GameObject ExplodingBallFX;
    public Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    public Vector3 force = new Vector3(0f, 0f, 7f);
    public float mass = 3f;

    /*void Update () {
        if (Input.GetKeyDown(keyCode)) {
            GameObject b = (GameObject)GameObject.Instantiate(ball, transform.position + transform.rotation * spawnOffset, transform.rotation);
            var r = b.GetComponent<Rigidbody>();

            if (r != null) {
                r.mass = mass;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                r.AddForce(Quaternion.LookRotation(ray.direction) * force, ForceMode.VelocityChange);
            }
        }
    }*/

    public void SwitchBool()
    {
        isBallExplosive = !isBallExplosive;
    }

    public void CheckForBallTypeAndSpawn()
    {
        if (isBallExplosive == true && explosiveBallSpawned == false)
        {
            SpawnExplodingBall();
            isBallExplosive = false;
            ExplodingBallFX.SetActive(false);
        }
        else if(explosiveBallSpawned == false)
        {
            SpawnBall();
        }

    }

    private void Update()
    {
        fillBTN.fillAmount = explodingBallUltimateFill;

        if (explodingBallUltimateFill >= 1f)
        {
            switchBTN.interactable = true;
        }
        else
        {
            switchBTN.interactable = false;
        }
    }

    public void SpawnBall()
    {
        GameObject b = LeanPool.Spawn(ball, transform.position + transform.rotation * spawnOffset, transform.rotation);
        //GameObject b = (GameObject)GameObject.Instantiate(ball, transform.position + transform.rotation * spawnOffset, transform.rotation);
        var r = b.GetComponent<Rigidbody>();

        if (r != null)
        {
            r.mass = mass;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            r.AddForce(Quaternion.LookRotation(ray.direction) * force, ForceMode.VelocityChange);
        }
    }

    public void SpawnExplodingBall()
    {
        GameObject b = LeanPool.Spawn(ExplodingBall, transform.position + transform.rotation * spawnOffset, transform.rotation);
        //GameObject b = (GameObject)GameObject.Instantiate(ball, transform.position + transform.rotation * spawnOffset, transform.rotation);
        var r = b.GetComponent<Rigidbody>();
        ExplodingBallFX.SetActive(false);
        explodeBTN.gameObject.SetActive(true);
        explodingBallUltimateFill = 0f;
        explosiveBallSpawned = true;

        if (r != null)
        {
            r.mass = mass;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            r.AddForce(Quaternion.LookRotation(ray.direction) * force, ForceMode.VelocityChange);
        }
    }


}
