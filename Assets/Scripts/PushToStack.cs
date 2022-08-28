using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushToStack : MonoBehaviour
{

    GameObject Canvas;
    //프리팹
    public GameObject prefab; //푸쉬 누르기 직전의 방향 프리팹
    public bool moveflag = false;
    
    //스택 안에 관리
    public GameObject[] InStack;
    public string[] blocknameArr;
    public int storedCount;  //스택에 들어있는 개수
    public const int MAXCOUNT = 20;


    GameObject FillArea;    //스택을 담을 공간
    float FillAreaWidth;    //공간의 너비
    Vector3 entrancePos;    //스택 통의 꼭대기 (입구의 Y좌표)

    private void Awake()
    {
        Canvas = GameObject.Find("Canvas");

        storedCount = 0;
        InStack = new GameObject[MAXCOUNT];
        blocknameArr = new string[MAXCOUNT];

        //AtStack Method
        FillArea = GameObject.Find("FillArea");  //스택을 담을 공간
        FillAreaWidth = FillArea.GetComponent<RectTransform>().rect.width;           //스택 통의 전체 길이      (896)

        //픽포인터
        GameObject PeekPointer = FillArea.transform.Find("PeekPointer").gameObject;
        Vector3 PeekPos = new Vector3(-FillArea.GetComponent<RectTransform>().rect.width / 2 + 8, -30, 0);
        PeekPointer.GetComponent<RectTransform>().anchoredPosition3D = PeekPos;
        PeekPointer.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    //각 화살표 방향이 클릭 되었을 때 걔네 정보를 가져올 Onclick 함수
    public void MoveUpOnClicked()
    {
        //시작전에 이미 있는 프리펩 지우기
        Debug.Log("UP 버튼 누름");
        //Car의 flag 가져오기
        if (CanMove())
        {
            MakePrefab("upblock");
        }
        else
        {
            Debug.Log("프리팹 생성 실패");
        }
    }
    public void MoveDownOnClicked()
    {
        //SelectedMove = GameObject.Find("downblock");
        Debug.Log("Down 버튼 누름");
        //Car의 flag 가져오기
        if (CanMove())
        {
            MakePrefab("downblock");
        }
        else
        {
            Debug.Log("프리팹 생성 실패");
        }
    }
    public void MoveRightOnClicked()
    {
        //SelectedMove = GameObject.Find("rightblock");
        Debug.Log("Right 버튼 누름");
        //Car의 flag 가져오기
        if (CanMove())
        {
            MakePrefab("rightblock");
        }
        else
        {
            Debug.Log("프리팹 생성 실패");
        }
    }
    public void MoveLeftOnClicked()
    {
        //SelectedMove = GameObject.Find("leftblock");
        Debug.Log("Left 버튼 누름");
        //Car의 flag 가져오기
        if (CanMove())
        {
            MakePrefab("leftblock");
        }
        else
        {
            Debug.Log("프리팹 생성 실패");
        }
    }
    //사용자가 동작한 방향이 길이 나있어서 이동할 수 있는 길인지
    public bool CanMove()
    {
        Car carScript = GameObject.Find("Car").GetComponent<Car>();
        return carScript.flag;
    }
    //Push Btn OnClick 될 때 동작시킬 함수
    public void PushOnClicked()
    {
        Canvas.transform.Find("MoveGroup").gameObject.SetActive(true);
        FillArea.transform.Find("PeekPointer").gameObject.SetActive(true);
        //
    }
    //프리팹 - 클릭된 방향의 아이템 생성 (스택으로 들어갈 친구 만들기)
    public void MakePrefab(string blockName)        
    {
        if (storedCount < MAXCOUNT)
        {
            storedCount++;
            //프리팹 생성
            //Euler(0, 180.0f, 0), GameObject.Find(blockName).transform.rotation  
            prefab = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockName), new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
            //프리팹 크기 조정
            prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(88, 88);
            GameObject image = prefab.transform.Find("Image").gameObject;
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 60);
            //프리팹 좌표 조정
            Vector3 startVec = GameObject.Find("StartPosOfArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 arriveVec = GameObject.Find("DstPosOfArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            prefab.GetComponent<RectTransform>().anchoredPosition3D = startVec;     //프리펩 생성 위치 (궤적 이동 시작 위치)
            //프리펩에 무브 함수 달아서 궤적 이동 출발
            prefab.AddComponent<BlockMove>();
            prefab.GetComponent<BlockMove>().letsMove(1, startVec, arriveVec, blockName, storedCount-1);

            //스택에 옮겨서 저장
            //AtStack(blockName);
        } else
        {
            //스택 통 가득 찬 경우 동작
            Debug.Log("STACK이 가득 찼으니 POP 또는 EMPTY 해야 함");
        }
    }
    //스택에 들어갈 작은 프리팹 생성
    public void AtStack(string blockname, int arrIndex)
    {
        //이번에 스택에 들어갈 프리팹                                                                
        InStack[arrIndex] = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockname), FillArea.transform.position, Quaternion.identity, FillArea.transform);
        InStack[arrIndex].transform.SetAsLastSibling();  //UI 상 제일 위에 보이게
        InStack[arrIndex].SetActive(true);

        //현재 위치 : 스택통 맨위 -> 수정 위치 : 현 위치 - 스택 통 길이 + 프리팹 크기 + 스택 통 두께 + (몇번째인지)
        float IconWidth = InStack[arrIndex].GetComponent<RectTransform>().sizeDelta.x;    //프리팹 크기    (44)
        InStack[arrIndex].GetComponent<RectTransform>().anchoredPosition3D += new Vector3((FillAreaWidth / 2) - IconWidth / 2, 0, 0);
        entrancePos = InStack[arrIndex].GetComponent<RectTransform>().anchoredPosition3D;
        //도착 위치
        Vector3 ArrivePos = InStack[arrIndex].GetComponent<RectTransform>().anchoredPosition3D + new Vector3(-FillAreaWidth + 8 + IconWidth * (arrIndex+1), 0, 0); //(0일 떄 -418 )
        InStack[arrIndex].AddComponent<BlockMove>();
        InStack[arrIndex].GetComponent<BlockMove>().letsMove(2, entrancePos, ArrivePos, arrIndex);

        //InStack[storedCount].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        //InStack[storedCount].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
        //InStack[storedCount].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);

        //Canvas.transform.Find("MoveGroup").gameObject.SetActive(false);
    }

    public void PopOnClicked()
    {
        Canvas.transform.Find("MoveGroup").gameObject.SetActive(false);
        //날아가는 모션 추가
        if(storedCount>0)
        {
            storedCount--;  //스택에 저장된 개수 -1
            UpdatePeek(storedCount-1);

            Vector3 startVec = InStack[storedCount].GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 arriveVec = GameObject.Find("DstPosOfPopmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            InStack[storedCount].GetComponent<BlockMove>().letsMove(3, startVec, entrancePos, storedCount);

            /////////////
            //자동차 이전 움직임으로
            Car Car = GameObject.Find("Car").GetComponent<Car>();
            switch (InStack[storedCount].tag)
            {
                case "UpPrefab":
                    {
                        Car.carMoveDown();
                        break;
                    }
                case "DownPrefab":
                    {
                        Car.carMoveUp();
                        break;
                    }
                case "RightPrefab":
                    {
                        Car.carMoveLeft();
                        break;
                    }
                case "LeftPrefab":
                    {
                        Car.carMoveRight();
                        break;
                    }
            }
            //Destroy(InStack[storedCount]);  //프리팹 제거    
            System.Array.Clear(blocknameArr, storedCount, 1);
            //PeekOnClicked();    //피크 갱신
        } else
        {
            Debug.Log("스택에 POP 할 수 있는 것이 없음.");
            EmptyClicked();     //비어 있으면 Empty 안내문 보여주기
            UpdatePeek(storedCount);
        }
        
    }
    public void PeekOnClicked()
    {
        Canvas.transform.Find("MoveGroup").gameObject.SetActive(false);
        if (storedCount > 0)
        {
            //스택 통 길이/2 
            GameObject PeekPointer = FillArea.transform.Find("PeekPointer").gameObject;
            Vector3 PeekPos = new Vector3(InStack[storedCount-1].GetComponent<RectTransform>().anchoredPosition3D.x + InStack[storedCount-1].GetComponent<RectTransform>().rect.width / 2, -30, 0);
            PeekPointer.GetComponent<RectTransform>().anchoredPosition3D = PeekPos;
            PeekPointer.gameObject.SetActive(true);

            //System.Threading.Thread.Sleep(1000);
            //PeekPointer.gameObject.SetActive(false);
        } else
        {
            GameObject PeekPointer = FillArea.transform.Find("PeekPointer").gameObject;
            Vector3 PeekPos = new Vector3(-FillArea.GetComponent<RectTransform>().rect.width/2+8, -30, 0);
            PeekPointer.GetComponent<RectTransform>().anchoredPosition3D = PeekPos;

            PeekPointer.gameObject.SetActive(true);

            //System.Threading.Thread.Sleep(1000);
            Invoke("PeekPointer.gameObject.SetActive(false)", 10.0f);
        }
    }
    public void UpdatePeek(int arrIndex)
    {
        if (storedCount > 0)
        {
            //스택 통 길이/2 
            GameObject PeekPointer = FillArea.transform.Find("PeekPointer").gameObject;
            Vector3 PeekPos = new Vector3(InStack[arrIndex].GetComponent<RectTransform>().anchoredPosition3D.x + InStack[arrIndex].GetComponent<RectTransform>().rect.width / 2, -30, 0);
            PeekPointer.GetComponent<RectTransform>().anchoredPosition3D = PeekPos;
        }
        else
        {
            GameObject PeekPointer = FillArea.transform.Find("PeekPointer").gameObject;
            Vector3 PeekPos = new Vector3(-FillArea.GetComponent<RectTransform>().rect.width / 2 + 8, -30, 0);
            PeekPointer.GetComponent<RectTransform>().anchoredPosition3D = PeekPos;
        }
    }
    public void EmptyDestroy()
    {
        GameObject EmptyText = Canvas.transform.Find("MessageEmpty").gameObject;
        EmptyText.SetActive(false);
    }
    public void EmptyClicked()
    {
        Canvas.transform.Find("MoveGroup").gameObject.SetActive(false);
        if (storedCount == 0)
        {
            //비어있음
            GameObject EmptyText = Canvas.transform.Find("MessageEmpty").gameObject;
            EmptyText.transform.SetAsLastSibling();
            EmptyText.SetActive(true);
            Debug.Log("empty");
            //메시지 다시 지워주기
            //System.Threading.Thread.Sleep(10000);
            Invoke("EmptyDestroy", 3.0f);
        } else
        {
            //비어있지 않음!
            Debug.Log("비어있지 않음");
        }

        /*
        //다 지우려면
        for (int i =0; i<storedCount; i++)
        {
            Destroy(InStack[i]);
        }
        storedCount = 0;

        //차 출발 위치로 되돌리기
        Car Car = GameObject.Find("Car").GetComponent<Car>();
        Car.carGoToStart();
        */
    }


}

