using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//transform = Fillarea
public class Queue_commands : MonoBehaviour
{
    public GameObject prefab;
    public GameObject Parent;
    public GameObject car;
    private Vector2 create_point;
    private RectTransform rect_obj , Parent_rect;

    public int count = 0;
    private int size = 20; // width 에 따라 변경?

    private GameObject[] queue; // 0 :up , 1: right , 2: left , 3: down

    GameObject FillArea;    //스택을 담을 공간
    float FillAreaWidth;    //공간의 너비
    Vector3 entrancePos;    //스택 통의 꼭대기 (입구의 Y좌표)

    public bool popflag = true;      //블록을 넣고 있거나 빼내는 동안 (=큐 통에 움직임이 있는 동안) 빼내기 불가
    bool pushflag = true;     //블록을 Queue에서 빼내는 동안에는 Insert 불가능. Insert가 가능한지 체크

    void Awake()
    {
        //Car
        car = GameObject.Find("Car");
        queue = new GameObject[size];

        //AtStack Method
        FillArea = GameObject.Find("FillArea");  //스택을 담을 공간
        FillAreaWidth = FillArea.GetComponent<RectTransform>().rect.width;           //스택 통의 전체 길이      (896)

    }

    //Queue에 넣을 방향 블록 클릭
    public void createBlock(string blockName, int prefabIndex)
    {
        popflag = false;
        if (pushflag)
        {
            if (count < size)
            {
                count++;
                queue[count - 1] = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockName), new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
                //프리팹 크기 조정
                queue[count - 1].GetComponent<RectTransform>().sizeDelta = new Vector2(88, 88);
                GameObject image = queue[count - 1].transform.Find("Image").gameObject;
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 60);
                //프리팹 좌표 조정
                Vector3 startVec = GameObject.Find("StartInArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                Vector3 arriveVec = GameObject.Find("DstInArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
                queue[count - 1].GetComponent<RectTransform>().anchoredPosition3D = startVec;     //프리펩 생성 위치 (궤적 이동 시작 위치)
                                                                                                  //프리펩에 무브 함수 달아서 궤적 이동 출발
                queue[count - 1].AddComponent<BlockMove>();
                queue[count - 1].GetComponent<BlockMove>().letsMove(4, startVec, arriveVec, blockName, count - 1);

            }
            else        //queue 최대 개수 가득 참
            {
                Debug.Log("QUEUE가 가득 참");
            }
        }
        else
        {
            Debug.Log("Queue에서 블록을 빼내는 동안 Insert 불가능");
        }
        
    }
    public void BlockToQueue(GameObject block, int arrIndex)      //블록이 큐로 들어온 이후
    {
        //블록 하이라키를 FillArea 하위로 변경
        block.transform.SetParent(GameObject.Find("FillArea").transform);
        //프리팹 크기 조정
        block.GetComponent<RectTransform>().sizeDelta = new Vector2(44, 44);
        GameObject image = block.transform.Find("Image").gameObject;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 30);
        //좌표 조정
        //현재 위치 : 스택통 맨위 -> 수정 위치 : 현 위치 - 스택 통 길이 + 프리팹 크기 + 스택 통 두께 + (몇번째인지)
        float IconWidth = block.GetComponent<RectTransform>().sizeDelta.x;    //프리팹 크기    (44)
        block.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0) + new Vector3(-(FillAreaWidth / 2) + IconWidth / 2, 0, 0);
        entrancePos = block.GetComponent<RectTransform>().anchoredPosition3D;
        //도착 위치
        Vector3 ArrivePos = block.GetComponent<RectTransform>().anchoredPosition3D + new Vector3(FillAreaWidth - IconWidth * (arrIndex+1), 0, 0); //(0일 떄 -418 )
        block.AddComponent<BlockMove>();
        block.GetComponent<BlockMove>().letsMove(5, entrancePos, ArrivePos, arrIndex);
    }

    //Queue에서 빼내기 위한 버튼 클릭
    public void BtnOnClicked()      //버튼 클릭시
    {
        //QUEUE 비어 있는 지 먼저 체크
        if (count > 0)
        {
            if (popflag)    //POP 가능 할 때  
            {
                pushflag = false;
                QueueOut();
            }
            else            //Push나 Pop이 안끝나서 아직 블록 움직이는 중 일 때
            {
                Debug.Log("PUSH 또는 POP 진행 중. POP 버튼 불가!!");
            }
        }
        else        
        {
            Debug.Log("현재 QUEUE : EMPTY");
        }
    }
    //Queue에서 맨 앞 블록 제거 하기
    public void QueueOut()
    {
        popflag = false;
        if (count > 0) 
        {
            //카 무브
            switch (queue[0].tag)
            {
                case "UpPrefab":
                    car.GetComponent<Car_Queue>().carMoveUp();
                    break;
                case "RightPrefab":
                    car.GetComponent<Car_Queue>().carMoveRight();
                    break;
                case "LeftPrefab":
                    car.GetComponent<Car_Queue>().carMoveLeft();
                    break;
                case "DownPrefab":
                    car.GetComponent<Car_Queue>().carMoveDown();
                    break;
            }
            if (count == 0) return;     //car를 move했을 때 이동불가 뜨면 큐, count 초기화됨
    
            //이동 불가 아니어서 자동차는 움직인 후
            //큐에서 그 방향 블록 빼기
            count--;
            GameObject outqueue = queue[0];
            System.Array.Clear(queue, 0, 1);
            //큐 정렬 (옆으로 한칸씩 이동)
            for (int i = 0; i < count; i++)
            {
                queue[i] = queue[i + 1];
                queue[i].GetComponent<RectTransform>().anchoredPosition3D += new Vector3(44, 0, 0);
            }

            //젤 앞에 꺼 버리는 궤적 이동
            Vector3 startVec = GameObject.Find("StartOutArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 arriveVec = GameObject.Find("DstOutArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            outqueue.transform.SetParent(GameObject.Find("Canvas").transform);
            outqueue.GetComponent<RectTransform>().anchoredPosition3D = startVec;
            outqueue.GetComponent<BlockMove>().letsMove(6, startVec, arriveVec, count);
        }
        else //다 빼낸 경우
        {
            popflag = true;
            pushflag = true;
            Debug.Log("Queue에 있는 블록 다 빼냄!");
            return;
        }
    }
    //블록 하나 POP 끝나면 다음 블록 꺼내기 POP 호출
    public void OneBlockDone()  
    {
        QueueOut();
        return;
    }

    public void CantMove()
    {
        for (int i = 0; i < count; i++)
        {
            Destroy(queue[i]);      //블록 오브젝트 제거
        }
        System.Array.Clear(queue, 0, count);    //큐 비우기
        count = 0;  //큐에 쌓인 개수 초기화

        popflag = true;
        pushflag = true;
        Debug.Log("Queue 초기화!");
        return;
    }
    public void move_car()
    {
        int i = 0;
        if (i <= count-1)
        {
            switch (queue[i].tag)
            {
                case "UpPrefab":
                    car.GetComponent<Car_Queue>().carMoveUp();
                    break;
                case "RightPrefab":
                    car.GetComponent<Car_Queue>().carMoveRight();
                    break;
                case "DownPrefab":
                    car.GetComponent<Car_Queue>().carMoveLeft();
                    break;
                case "LeftPrefab":
                    car.GetComponent<Car_Queue>().carMoveDown();
                    break;
            }
            Destroy(transform.GetChild(0).gameObject);
            i++;
            if (i == count)
            {
                CancelInvoke("move_car");
                Debug.Log("이동 종료");
                count = 0;
                i = 0;
                return;
            }
            
            Invoke("move_car", 1.34f);
        }
        else
        {
            
        }
    }
    public void UpOnClicked()
    {
        if (count < 20)
        {
            createBlock("upblock", 0);
        }
        else
        {
            Debug.Log("큐 가득 참");
        }
    }
    public void RightOnClicked()
    {
        if (count < 20)
        {
            createBlock("rightblock", 1);
        }
        else
        {
            Debug.Log("큐 가득 참");
        }
    }
    public void LeftOnClicked()
    {
        if (count < 20)
        {
            createBlock("leftblock", 2);
        }
        else
        {
            Debug.Log("큐 가득 참");
        }
    }
    public void DownOnClicked()
    {
        if (count < 20)
        {
            createBlock("downblock", 3);
        }
        else
        {
            Debug.Log("큐 가득 참");
        }
    }
}
