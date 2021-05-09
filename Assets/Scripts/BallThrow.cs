using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallThrow : MonoBehaviour
{
    Vector2 startPos, endPos, direction; // Начальная позиция касания 
    float touchTimeStart, touchTimeFinish, timeInterval; // Для рассчета силы броска

    [SerializeField]
    float throwForceInXandY = 1f; // Для контроля силы броска по осям X и Y

    [SerializeField]
    float throwForceInZ = 50f; // Для контроля силы броска по Z

    Rigidbody rb;


    private IEnumerator coroutine;

    [Space]
    public Transform ballOrigPos;
    public float returnTime;
    private bool ballReturning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {


#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !ballReturning)
        {

            touchTimeStart = Time.time;
            startPos = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0) && !ballReturning)
        {

            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = Input.mousePosition;
            direction = startPos - endPos;


            rb.isKinematic = false;
            rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);


        }

#else

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !ballReturning)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !ballReturning)
        {
            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = Input.GetTouch(0).position;
            direction = startPos - endPos;


            rb.isKinematic = false;
            rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);

        }
#endif

        if (ballReturning == true)
        {
            ReturnBall();
        }


        if (transform.localPosition == Vector3.zero)
        {
            ballReturning = false;
        }

    }

    void ReturnBall()
    {

        transform.DOMove(ballOrigPos.position, returnTime * Time.deltaTime);
        rb.isKinematic = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        coroutine = WaitAndReturnBall(0.1f);
        StartCoroutine(coroutine);

    }

    private IEnumerator WaitAndReturnBall(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ballReturning = true;
    }
}
