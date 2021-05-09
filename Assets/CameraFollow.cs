using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera vmCum;
    public List<Transform> rivalsPos = new List<Transform>();
    public List<Transform> levelCheckPoints = new List<Transform>();

    public Transform closestRivalpos;
    public float timeCheck = 0.3f;
    float y = 2;
    GameObject invisibleGO;

    public static CameraFollow instance; private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        vmCum = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        vmCum.m_Follow = GameManager.instance.player;
        StartCoroutine(CheckDist());
        invisibleGO = new GameObject();
        vmCum.LookAt = invisibleGO.transform;
        y = GameManager.instance.player.position.y + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //invisibleGO.transform.position = 
        Vector3 pos = closestRivalpos.position;
        pos.y = y;
        invisibleGO.transform.position = pos;
    }

    Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        if (bestTarget == null) bestTarget = GetCheckPoint(levelCheckPoints);

        return bestTarget;
    }

    Transform GetCheckPoint(List<Transform> checkpoints)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in levelCheckPoints)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    IEnumerator CheckDist()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        while (true)
        {
            var closestEnemytemp = GetClosestEnemy(rivalsPos);
            if (closestRivalpos != closestEnemytemp)
            {
                closestRivalpos = closestEnemytemp;
            }

            yield return new WaitForSecondsRealtime(timeCheck);
        }
        
    }
}
