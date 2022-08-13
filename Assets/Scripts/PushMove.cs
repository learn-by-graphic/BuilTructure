using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushMove : MonoBehaviour
{

    bool start = false;

    public Transform target;
    public Vector3 targetPos;
    private Vector3 startPos;


    public float m_Speed = 50;
    public float m_HeightArc = 1;
    private bool m_IsStart;

    public float travelTime = 10.0F;

    private float startTime;

    void Start()
    {
        target = GameObject.Find("DstOfMove").GetComponent<Transform>();
        targetPos = target.position;
        startPos = transform.position;
        //sunset2 = transform.position + new Vector3(550, 290, 0);
        //sunset = GameObject.Find("DstOfMove").GetComponent<Transform>();
        //Debug.Log(sunset.position.ToString());
        startTime = Time.time;
    }
    void Update()
    {
        if (start)
        {

            /*
            float x0 = m_StartPosition.x;
            float x1 = m_Target.position.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, m_Target.position.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);
            
            //transform.rotation = LookAt2D(nextPosition - transform.position);
            transform.position = nextPosition;

            if (nextPosition == m_Target.position)
                Arrived();
            */


            /*
            Vector3 center = new Vector3(transform.position.x, target.position.y, 0);
            //Vector3 center = (transform.position + target.position) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 riseRelCenter = transform.position - center;
            Vector3 setRelCenter = target.position - center;
            float fracComplete = (Time.time - startTime) / travelTime;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
            */
        }

    }

    public void letsMove(bool m_IsStart)
    {
        if (m_IsStart)
        {
            
            start = true;
        }
    }
    /*
    void Arrived()
    {
        Debug.Log("도착");
        //Destroy(gameObject);
    }
    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
    */
}