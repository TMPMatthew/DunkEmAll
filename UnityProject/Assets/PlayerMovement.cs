using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public Joystick stick;
    public CharacterController characterController;
    public float speed = 7;
    public bool able = true;
    public CameraFollow cameraFollow;

    [Header("Vertical memes")]
    private float gravity = 9.87f;
    private float verticalSpeed = 0;

    [Header("Dunk")]
    [SerializeField] Transform target;
    [SerializeField] float initialAngle;
    private Rigidbody rigid;

    [Header("Animation")]
    private Animator anim2hands;
    public GameObject obj;
    private void Awake()
    {
        
    }
    void Start()
    {
        GameManager.instance.player = this.gameObject.transform;
        stick = GameObject.Find("Floating Joystick").GetComponent<Joystick>();
        rigid = GetComponent<Rigidbody>();
        anim2hands = GameObject.Find("hands").GetComponent<Animator>();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        obj = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(able) Move();

    }

    public void AnimateSpawnBall()
    {
        anim2hands.SetTrigger("Shoot");
        GameManager.instance.StopSlowMo();
    }

    private void Move()
    {
        var lookPos = cameraFollow.closestRivalpos.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);


        float horizontalMove = stick.Horizontal;
        float verticalMove = stick.Vertical;

        if (characterController.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;
        Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);

        Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
        //rigid.velocity = speed * move ;
    }

    public void Dunk()
    {
        Vector3 p = target.position;

       // float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f  /*gravity*/ * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
       // rigid.velocity = finalVelocity;

        // Alternative way:
         rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);

    }

}
