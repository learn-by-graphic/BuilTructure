using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    //string blockname;
    short isStart = 0;     //어떤 걸 이동할 건지
    bool newmoveflag = false;

    public RectTransform Target;       //도착지

    public float m_Speed = 1000.0f;  //450
    public float m_HeightArc = 120.0f;
    Vector3 startpos;
    Vector3 targetpos;
    Vector3 startVec = new Vector3(-380, -70, 0);
    Vector3 targetVec = new Vector3(370, 250, 0);

    public void letsMove(short moveIndex, Vector3 start, Vector3 arrive)
    {
        isStart = moveIndex;
        startpos = VecRound(start);
        targetpos = VecRound(arrive);

        /*
        switch (isStart)
        {
            case 1:     //방향키 블록이 스택 통까지 날아가는 움직임
                {
                    startpos = VecRound(start);
                    targetpos = VecRound(arrive);
                    Debug.Log("Move Index :" + moveIndex);
                    break;
                }
            case 2:     //스택통 맨 위에서 아래로 떨어지는 움직임
                {
                    startpos = VecRound(start);
                    targetpos = VecRound(arrive);
                    Debug.Log("Move Index :" + moveIndex + start + arrive);
                    break;
                }
        }
        */
    }
    private void Awake()
    {
    }
    void Start()
    {
    }
    void Update()
    {
        switch (isStart)
        {
            case 1:     //방향키 블록이 스택 통까지 곡선 이동
                {
                    float x0 = startpos.x;      //출발 x
                    float x1 = targetpos.x;     //도착 x
                    float distance = x1 - x0;   //x좌표 거리 차이
                    float nextX = Mathf.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition3D.x, x1, m_Speed * Time.deltaTime);
                    float baseY = Mathf.Lerp(startpos.y, targetpos.y, (nextX - x0) / distance);
                    float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
                    Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.GetComponent<RectTransform>().anchoredPosition3D.z);

                    //transform.rotation = LookAt2D(nextPosition - transform.position);     //회전
                    transform.GetComponent<RectTransform>().anchoredPosition3D = nextPosition;

                    if (nextPosition == targetpos)
                    {

                        Arrived(isStart);
                        return;
                    }
                    break;
                }
            case 2:     //스택 통 맨 위에서 저장될 곳까지 직선 이동
                {
                    Vector3 TransVec = transform.GetComponent<RectTransform>().anchoredPosition3D;
                    float x0 = startpos.x;
                    float x1 = targetpos.x;
                    float nextX = Mathf.MoveTowards(TransVec.x, x1, m_Speed * Time.deltaTime);
                    Vector3 nextPosition = VecRound(new Vector3(nextX, TransVec.y, TransVec.z));

                    transform.GetComponent<RectTransform>().anchoredPosition3D = nextPosition;

                    if (VecRound(nextPosition) == VecRound(targetpos))
                    {
                        Arrived(isStart);
                        //transform.GetComponent<PushMove>().enabled = false;
                    }

                    break;
                }
            case 3:     //스택 젤 위에 꺼가 꺼내짐
                {
                    if (!newmoveflag)       //현 위치에서 스택 꼭대기로 이동

                    {
                        Vector3 TransVec = transform.GetComponent<RectTransform>().anchoredPosition3D;
                        float x0 = startpos.x;
                        float x1 = targetpos.x;
                        float distance = x1 - x0;
                        float nextX = Mathf.MoveTowards(TransVec.x, x1, m_Speed * Time.deltaTime);
                        Vector3 nextPosition = VecRound(new Vector3(nextX, TransVec.y, TransVec.z));

                        transform.GetComponent<RectTransform>().anchoredPosition3D = nextPosition;

                        //꼭대기에 오면 밖으로 이동
                        if (VecRound(nextPosition) == VecRound(targetpos)) {

                            System.Threading.Thread.Sleep(100);
                            newmoveflag = true; //스택통 바깥 움직임을 제어할 플래그
                            transform.parent = GameObject.Find("Canvas").transform; //스택인디케이터 안에 있던 오브젝트를 캔버스 하위 오브젝트로 변경
                            transform.GetComponent<RectTransform>().anchoredPosition3D = GameObject.Find("DstOfMove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                            //크기 확대
                            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
                            transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                        }
                    }
                    else                 //스택 꼭대기 -> 밖으로 이동
                    {
                        Vector3 newStartPos = GameObject.Find("DstOfMove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                        Vector3 newTargetPos = GameObject.Find("DstOfPop").transform.GetComponent<RectTransform>().anchoredPosition3D;
                        float newx0 = newStartPos.x;      //출발 x
                        float newx1 = newTargetPos.x;     //도착 x
                        float newdistance = newx1 - newx0;   //x좌표 거리 차이
                        float newnextX = Mathf.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition3D.x, newx1, m_Speed * Time.deltaTime);
                        float newbaseY = Mathf.Lerp(newStartPos.y, newTargetPos.y, (newnextX - newx0) / newdistance);
                        float arc = m_HeightArc * (newnextX - newx0) * (newnextX - newx1) / (-0.25f * newdistance * newdistance);
                        Vector3 newnextPosition = new Vector3(newnextX, newbaseY + arc, 0);

                        //transform.rotation = LookAt2D(nextPosition - transform.position);
                        transform.GetComponent<RectTransform>().anchoredPosition3D = newnextPosition;

                        if (newnextPosition == newTargetPos)
                        {
                            Debug.Log("최종목적지 도착");
                            Arrived(isStart);
                            return;
                        }
                    }
                    break;
                }
        }
    }
    void Arrived(short moveIndex)   //목적지에 도착했을 때
    {
        isStart = 0;
        switch (moveIndex)
        {
            case 1:     //방향키 블록이 스택 통까지 날아가는 움직임
                {
                    System.Threading.Thread.Sleep(100);
                    Destroy(gameObject);
                    GameObject.Find("Button_push").GetComponent<PushToStack>().AtStack();

                    break;
                }
            case 2:     //스택통 맨 위에서 아래로 떨어지는 움직임
                {
                    System.Threading.Thread.Sleep(100);
                    Debug.Log("스택에 저장 완료!");
                    GameObject.Find("Button_push").GetComponent<PushToStack>().PeekOnClicked();
                    break;
                }
            case 3:     //POP
                {
                    Debug.Log("Pop 완료");
                    newmoveflag = false;
                    Destroy(gameObject);
                    GameObject.Find("Button_push").GetComponent<PushToStack>().PeekOnClicked();
                    break;
                }
        }
        return;
    }
    Vector3 VecRound(Vector3 org)       //벡터 반올림
    {
        Vector3 ret;
        ret.x = Mathf.Round(org.x);
        ret.y = Mathf.Round(org.y);
        ret.z = Mathf.Round(org.z);

        return ret;
    }
}