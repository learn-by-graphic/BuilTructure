using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushMove : MonoBehaviour
{
    string blockname;
    bool start;

    public RectTransform Target;       //도착지

    public float m_Speed = 300.0f;
    public float m_HeightArc = 120.0f;
    private bool m_IsStart;
    Vector3 startpos;
    Vector3 startVec = new Vector3(-380, -70, 0);
    Vector3 targetVec = new Vector3(370, 250, 0);

    //public float firingAngle = 20.0f;
    //public float gravity = 98f;

    //public RectTransform ObjectRect;
    //private RectTransform myTransform;



    public float travelTime = 10.0F;

    private float startTime;

    private void Awake()
    {
        //myTransform = GetComponent<RectTransform>();
    }


    void Start()
    {
        startpos = transform.GetComponent<RectTransform>().anchoredPosition3D;
        Target = GameObject.Find("DstOfMove").transform.GetComponent<RectTransform>();
        //ObjectRect = GetComponent<RectTransform>();
        //StartCoroutine(SimulateProjectile());


        /*
        target = GameObject.Find("DstOfMove").GetComponent<Transform>();
        targetVec = target.position;
        startVec = transform.position;
        //sunset2 = transform.position + new Vector3(550, 290, 0);
        //sunset = GameObject.Find("DstOfMove").GetComponent<Transform>();
        //Debug.Log(sunset.position.ToString());
        startTime = Time.time;
        */
    }


    /*
    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown       
        yield return new WaitForSeconds(1.5f);
        // Move projectile to the position of throwing object + add some offset if needed.       
        ObjectRect.anchoredPosition3D = myTransform.anchoredPosition3D + new Vector3(0, 0, 0);
        // Calculate distance to target       
        float target_Distance = Vector3.Distance(ObjectRect.anchoredPosition3D, Target.anchoredPosition3D);
        // Calculate the velocity needed to throw the object to the target at specified angle.       
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        // Extract the X  Y componenent of the velocity       
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        // Calculate flight time.       
        float flightDuration = target_Distance / Vx;
        // Rotate projectile to face the target.       
        ObjectRect.rotation = Quaternion.LookRotation(Target.anchoredPosition3D - ObjectRect.anchoredPosition3D);
        float elapse_time = 0; 
        while (elapse_time < flightDuration)
        {
            ObjectRect.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }    
    }
    */

    void Update()
    {
        if (start)
        {
            float x0 = startpos.x;
            float x1 = Target.anchoredPosition3D.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition3D.x, x1, m_Speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startpos.y, Target.anchoredPosition3D.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.GetComponent<RectTransform>().anchoredPosition3D.z);

            //transform.rotation = LookAt2D(nextPosition - transform.position);
            transform.GetComponent<RectTransform>().anchoredPosition3D = nextPosition;

            if (nextPosition == Target.anchoredPosition3D)
                Arrived();
            
        }

    }

    public void letsMove(bool m_IsStart, string blockName)
    {
        if (m_IsStart)
        {
            Debug.Log("Move Start");
            start = true;
            blockname = blockName;
        }
    }
    
    void Arrived()
    {
        Debug.Log("도착");
        //
        System.Threading.Thread.Sleep(1000);
        Destroy(gameObject);
        start = false;
        GameObject.Find("Button_push").GetComponent<PushToStack>().AtStack(blockname);
    }
    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
    
}