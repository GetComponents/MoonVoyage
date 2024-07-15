using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhiaPlant : MonoBehaviour
{
    //[SerializeField]
    //int maxSegment;
    [SerializeField]
    ObjectProperty op;
    [SerializeField]
    GameObject segmentPrefab;
    [SerializeField]
    Transform parentTransform;
    [SerializeField]
    Vector2[] PathIndicators;
    [SerializeField]
    int[] hideStops;
    List<GameObject> CurrentSegments = new List<GameObject>();
    [SerializeField]
    float growThreshhold;
    private float growTimer;
    bool regressing, waiting;
    [SerializeField]
    float regressIncrease;
    float regressMul = 1;
    float regressTimer, waitTimer;
    [SerializeField]
    float timeToRegress = 1, timeToWait = 1;
    Vector3 headPrevPos, headNextPos;

    private void Start()
    {
        op.InteractedWithPlayer.AddListener(Hide);
    }

    void Update()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > timeToWait)
            {
                waiting = false;
                waitTimer = 0;
                headPrevPos = Vector3.zero;
                headNextPos = Vector3.zero;
                regressMul = 1;
            }
            else
            {
                return;
            }
        }
        if (!regressing && CurrentSegments.Count < PathIndicators.Length)
        {
            Grow();
        }
        if (regressing)
        {
            Regress();
        }
    }

    public void Hide()
    {
        if (regressing)
        {
            regressMul *= regressIncrease;
            return;
        }
        if (CurrentSegments.Count == 0)
        {
            return;
        }
        regressing = true;
        regressTimer = (1 - (growTimer / growThreshhold)) * timeToRegress;
        Vector3 tmp = headPrevPos;
        headPrevPos = headNextPos;
        headNextPos = tmp;
        //MoveHead(-1);
        growTimer = 0;
    }

    private void Regress()
    {
        regressTimer += Time.deltaTime * regressMul;
        transform.localPosition = Vector2.Lerp(headPrevPos, headNextPos, regressTimer / timeToRegress);
        if (regressTimer > timeToRegress)
        {
            RemoveSegment();
            MoveHead(-1);
        }
    }

    private void RemoveSegment()
    {
        //MoveSegment(CurrentSegments.Count, gameObject, -1);
        GameObject tmp = CurrentSegments[CurrentSegments.Count - 1];
        for (int i = 0; i < CurrentSegments.Count; i++)
        {
            MoveSegment(CurrentSegments.Count - i - 1, CurrentSegments[i], -1);
        }
        CurrentSegments.Remove(tmp);
        Destroy(tmp);
        if (CurrentSegments.Count == 0)
        {
            regressing = false;
            waiting = true;
        }
        regressTimer = 0;
        //if (CurrentSegments.Count == 1 || hideStops. CurrentSegments)
    }

    private void Grow()
    {
        growTimer += Time.deltaTime;
        transform.localPosition = Vector2.Lerp(headPrevPos, headNextPos, growTimer / growThreshhold);
        if (growTimer > growThreshhold)
        {
            growTimer = 0;
            //if (CurrentSegments.Count < PathIndicators.Length - 1)
            AddSegment();
            MoveHead(1);
        }
    }

    private void AddSegment()
    {
        GameObject tmp = Instantiate(segmentPrefab, parentTransform);
        for (int i = 0; i < CurrentSegments.Count; i++)
        {
            MoveSegment(CurrentSegments.Count - i, CurrentSegments[i], 1);
        }
        //transform.position += new Vector3(tmp.transform.lossyScale.x, 0, 0);
        //tmp.transform.localPosition = new Vector3(tmp.transform.lossyScale.x * (CurrentSegments.Count + 1), 0, 0) * PathIndicators[CurrentSegments.Count];
        tmp.transform.localPosition = Vector3.zero;
        CurrentSegments.Add(tmp);
    }

    private void MoveHead(int inverse)
    {
        //MoveSegment(CurrentSegments.Count + 1, gameObject, 1);
        headPrevPos = headNextPos;
        if (CurrentSegments.Count < PathIndicators.Length)
        {
            //    headNextPos = transform.position + new Vector3(PathIndicators[CurrentSegments.Count + 1].x, PathIndicators[CurrentSegments.Count + 1].y, 0);
            headNextPos = transform.localPosition + (inverse * new Vector3(PathIndicators[CurrentSegments.Count].x, PathIndicators[CurrentSegments.Count].y, 0));
        }
        //else
        //{
        //}
    }

    private void MoveSegment(int index, GameObject segment, int inverse)
    {
        if (PathIndicators[index].x != 0)
        {
            //segment.transform.localEulerAngles = new Vector3(0, 0, 90 + (PathIndicators[index].x * 90));
            segment.transform.position += new Vector3(PathIndicators[index].x * segmentPrefab.transform.lossyScale.x * inverse, 0, 0);
        }
        else
        {
            //segment.transform.localEulerAngles = new Vector3(0, 0, 180 + (PathIndicators[index].y * 90));
            segment.transform.position += new Vector3(0, PathIndicators[index].y * segmentPrefab.transform.lossyScale.x * inverse, 0);
        }
    }
}
