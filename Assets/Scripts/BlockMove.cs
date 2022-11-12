using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    int arrIndex = 0;  //궤적이동이 끝난 후 몇 번째로 나가는 오브젝트인지.
    public string[] blocknameArr = new string [20];
    //string blockname;
    int isStart;     //어떤 걸 이동할 건지
    bool newmoveflag;

    public RectTransform Target;       //도착지

    public float m_Speed = 1000.0f;  //450
    public float m_HeightArc = -150.0f;
    Vector3 startpos;
    Vector3 targetpos;

    public void letsMove(int moveIndex, Vector3 start, Vector3 arrive, string blockname, int arrindex)
    {
        arrIndex = arrindex;
        blocknameArr[arrIndex] = blockname;    //들어온 
        isStart = moveIndex;
        startpos = VecRound(start);
        targetpos = VecRound(arrive);
    }
    public void letsMove(int moveIndex, Vector3 start, Vector3 arrive, int arrindex)
    {
        arrIndex = arrindex;
        isStart = moveIndex;
        startpos = VecRound(start);
        targetpos = VecRound(arrive);
    }
    public void letsMove(int moveIndex, Vector3 start, Vector3 arrive)
    {
        isStart = moveIndex;
        startpos = VecRound(start);
        targetpos = VecRound(arrive);

    }

    void Arrived(int moveIndex)   //목적지에 도착했을 때
    {
        isStart = 0;
        switch (moveIndex)
        {
            case 1:     //스택통 입구에 도착
                {
                    //System.Threading.Thread.Sleep(100);
                    Destroy(gameObject);
                    GameObject.Find("Button_push").GetComponent<PushToStack>().AtStack(blocknameArr[arrIndex], arrIndex);

                    break;
                }
            case 2:     //스택통에 도착한 경우.
                {
                    //System.Threading.Thread.Sleep(100);
                    Debug.Log("(스택에 Push 완료!) 개수: " + (arrIndex + 1)); 
                    GameObject.Find("Button_push").GetComponent<PushToStack>().UpdatePeek(arrIndex);
                    break;
                }
            case 3:     //POP
                {

                    Debug.Log("(Pop 완료!) 스택에 담긴 개수 : " + arrIndex);
                    newmoveflag = false;
                    Destroy(gameObject);
                    System.Array.Clear(blocknameArr, arrIndex, 1);
                    break;
                }
            case 4:
                {
                    GameObject.Find("FillArea").GetComponent<Queue_commands>().BlockToQueue(gameObject, arrIndex);
                    break;
                }
            case 5:
                {
                    //Push가 다 끝난 경우에만 POP 누를 수 있게
                    if ((arrIndex+1)== GameObject.Find("FillArea").GetComponent<Queue_commands>().count)
                    {
                        Debug.Log("모든 블록 PUSH 성공! 개수: " + (arrIndex + 1));
                        GameObject.Find("FillArea").GetComponent<Queue_commands>().popflag = true;
                    } else
                    {
                        Debug.Log("블록 한 개 PUSH 성공. 아직 PUSH 진행 중. 개수: " + (arrIndex + 1));
                    }
                    break;
                }
            case 6:
                {
                    //GameObject.Find("FillArea").GetComponent<Queue_commands>().QueueOut();
                    GameObject.Find("FillArea").GetComponent<Queue_commands>().OneBlockDone();
                    Destroy(gameObject);
                    break;
                }
        }
        return;
    }
    private void Awake()
    {
        isStart = 0;     //어떤 걸 이동할 건지
        newmoveflag = false;
    }
    void Start()
    {
    }
    void Update()
    {
        if(isStart == 1 || isStart == 4 || isStart == 6)
        {
            if (isStart == 1)
            {
                m_Speed = 1000.0f;  //450
                m_HeightArc = -150.0f;
            }
            else if(isStart == 4)
            {
                m_Speed = 450.0f;  //450
                m_HeightArc = 450.0f;
            } else if (isStart == 6)
            {
                m_Speed = 700.0f;  //450
                m_HeightArc = -300.0f;

                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(88, 88);
                GameObject image = transform.Find("Image").gameObject;
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 60);
            }
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
        } else if(isStart == 2 || isStart == 5)
        {
            m_Speed = 1000.0f;
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
        } else if (isStart == 3)
        {
            if (!newmoveflag)       //현 위치에서 스택 꼭대기로 이동

            {
                m_Speed = 500.0f;
                Vector3 TransVec = transform.GetComponent<RectTransform>().anchoredPosition3D;
                float x0 = startpos.x;
                float x1 = targetpos.x;
                float distance = x1 - x0;
                float nextX = Mathf.MoveTowards(TransVec.x, x1, m_Speed * Time.deltaTime);
                Vector3 nextPosition = VecRound(new Vector3(nextX, TransVec.y, TransVec.z));

                transform.GetComponent<RectTransform>().anchoredPosition3D = nextPosition;

                //꼭대기에 오면 밖으로 이동
                if (VecRound(nextPosition) == VecRound(targetpos))
                {

                    //System.Threading.Thread.Sleep(100);
                    newmoveflag = true; //스택통 바깥 움직임을 제어할 플래그

                    //오브젝트 하이라키 조정 (스택통 안에 들어있던 블록을 바깥으로 꺼내는 의미)
                    transform.SetParent(GameObject.Find("Canvas").transform, false); //스택인디케이터 안에 있던 오브젝트를 캔버스 하위 오브젝트로 변경
                                                                                     //출발점
                    transform.GetComponent<RectTransform>().anchoredPosition3D = GameObject.Find("DstPosOfArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                    //프리팹 크기 조정
                    transform.GetComponent<RectTransform>().sizeDelta = new Vector2(88, 88);
                    GameObject image = transform.transform.Find("Image").gameObject;
                    image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 60);
                }
            }
            else                 //스택 꼭대기 -> 밖으로 이동
            {
                m_Speed = 1000.0f;

                Vector3 newStartPos = GameObject.Find("DstPosOfArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                Vector3 newTargetPos = GameObject.Find("DstPosOfPopmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                float newx0 = newStartPos.x;      //출발 x
                float newx1 = newTargetPos.x;     //도착 x
                float newdistance = newx1 - newx0;   //x좌표 거리 차이
                float newnextX = Mathf.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition3D.x, newx1, m_Speed * Time.deltaTime);
                float newbaseY = Mathf.Lerp(newStartPos.y, newTargetPos.y, (newnextX - newx0) / newdistance);
                float arc = m_HeightArc * (newnextX - newx0) * (newnextX - newx1) / (-0.25f * newdistance * newdistance);
                Vector3 newnextPosition = new Vector3(newnextX, newbaseY + arc, 0);

                //transform.rotation = LookAt2D(nextPosition - transform.position);
                transform.GetComponent<RectTransform>().anchoredPosition3D = newnextPosition;

                if (newnextPosition == newTargetPos)    //최종 목적지 도착
                {
                    Arrived(isStart);
                    return;
                }
            }
        }
        /*
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

                            //System.Threading.Thread.Sleep(100);
                            newmoveflag = true; //스택통 바깥 움직임을 제어할 플래그

                            //오브젝트 하이라키 조정 (스택통 안에 들어있던 블록을 바깥으로 꺼내는 의미)
                            transform.SetParent(GameObject.Find("Canvas").transform, false); //스택인디케이터 안에 있던 오브젝트를 캔버스 하위 오브젝트로 변경
                            //출발점
                            transform.GetComponent<RectTransform>().anchoredPosition3D = GameObject.Find("DstPosOfArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                            //프리팹 크기 조정
                            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(88, 88);
                            GameObject image = transform.transform.Find("Image").gameObject;
                            image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 60);
                        }
                    }
                    else                 //스택 꼭대기 -> 밖으로 이동
                    {
                        Vector3 newStartPos = GameObject.Find("DstPosOfArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                        Vector3 newTargetPos = GameObject.Find("DstPosOfPopmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                        float newx0 = newStartPos.x;      //출발 x
                        float newx1 = newTargetPos.x;     //도착 x
                        float newdistance = newx1 - newx0;   //x좌표 거리 차이
                        float newnextX = Mathf.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition3D.x, newx1, m_Speed * Time.deltaTime);
                        float newbaseY = Mathf.Lerp(newStartPos.y, newTargetPos.y, (newnextX - newx0) / newdistance);
                        float arc = m_HeightArc * (newnextX - newx0) * (newnextX - newx1) / (-0.25f * newdistance * newdistance);
                        Vector3 newnextPosition = new Vector3(newnextX, newbaseY + arc, 0);

                        //transform.rotation = LookAt2D(nextPosition - transform.position);
                        transform.GetComponent<RectTransform>().anchoredPosition3D = newnextPosition;

                        if (newnextPosition == newTargetPos)    //최종 목적지 도착
                        {
                            Arrived(isStart);
                            return;
                        }
                    }
                    break;
                } 
        } */
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