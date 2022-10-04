using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortBlockMove : MonoBehaviour
{
    //Block Moving Tool
    int moveType;
    bool deadlock = false;

    Vector3 startpos;
    Vector3 targetpos;

    public float m_Speed = 500.0f;  //450
    public float m_HeightArc = -150.0f;

    // Start is called before the first frame update
    void Start()
    {
        //block Moving
        deadlock = true;
        switch (moveType)
        {
            case 1:
                {
                    m_HeightArc = -150.0f;
                    break;
                }
            case 2:
                {
                    m_HeightArc = 150.0f;
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deadlock)
        {
            Debug.Log("update");
            float x0 = startpos.x;      //출발 x
            float x1 = targetpos.x;     //도착 x
            float distance = x1 - x0;   //x좌표 거리 차이
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startpos.y, targetpos.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

            //transform.rotation = LookAt2D(nextPosition - transform.position);     //회전
            transform.position = nextPosition;

            if (nextPosition == targetpos)
            {
                Debug.Log("arrive");
                Arrived();
                return;
            }
        }
    }

    public void letsMove(int index, Vector3 start, Vector3 dst)
    {
        moveType = index;
        startpos = start;
        targetpos = dst;
        deadlock = true;
    }

    void Arrived()
    {
        deadlock = false;
    }
}

