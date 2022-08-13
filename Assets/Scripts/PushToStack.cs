using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Tilemaps;
using System.Threading;


public class PushToStack : MonoBehaviour
{
    //프리팹
    public GameObject prefab; //푸쉬 누르기 직전의 방향 프리팹

    public bool moveflag = false;
    Vector3 startVec = new Vector3(-380, -70, 0);
    Vector3 arriveVec = new Vector3(370, 250, 0);

    //스택 안에 관리
    public GameObject[] InStack = new GameObject[11];
    public int storedCount;  //스택에 들어있는 개수
    public const int MAXCOUNT = 11;
    GameObject StackIndicator; //스택을 담을 공간
    float topY;     //현재 담겨있는 프리팹 중에 가장 높은 Y    (스택의 Top의 Y좌표
    Vector3 entrancePos;     //스택 통의 꼭대기 (입구의 Y좌표)

    // Start is called before the first frame update
    void Start()
    {
        storedCount = 0;
        StackIndicator = GameObject.Find("StackIndicator");  //스택을 담을 공간
    }
    //프리팹 - 클릭된 방향의 아이템 생성 (스택으로 들어갈 친구 만들기)
    public void MakePrefab(string blockName)
    {
        if (storedCount < MAXCOUNT)
        {
            //프리팹 생성
            //Euler(0, 180.0f, 0), GameObject.Find(blockName).transform.rotation  
            prefab = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockName), new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
            //프리팹 좌표 조정
            prefab.GetComponent<RectTransform>().anchoredPosition3D = startVec;     //프리펩 생성 위치 (궤적 이동 시작 위치)
            prefab.AddComponent<PushMove>();
            moveflag = true;
            prefab.GetComponent<PushMove>().letsMove(moveflag);

            //스택에 옮겨서 저장
            AtStack(prefab, blockName);
        } else
        {
            //스택 통 가득 찬 경우 동작
            Debug.Log("STACK이 가득 찼으니 POP 또는 EMPTY 해야 함");
        }
    }

    public void AtStack(GameObject pf, string blockName)
    {
        //모션 이동했던 큰 프리팹 제거
        //Destroy(pf);

        //스택에 들어갈 작은 프리팹 생성
        // 앵커로부터 길이/2 + 아이콘 크기*(밑에 개수)
        Debug.Log(storedCount.ToString());
        
        //이번에 스택에 들어갈 프리팹                                                                
        InStack[storedCount] = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockName), StackIndicator.transform.position, Quaternion.identity, StackIndicator.transform);
        InStack[storedCount].transform.SetAsLastSibling();  //UI 상 제일 위에 보이게
        float IconWidth = InStack[storedCount].GetComponent<RectTransform>().rect.width;    //프리팹 크기
        //출발 위치 (스택 통 꼭대기)
        InStack[storedCount].GetComponent<RectTransform>().anchoredPosition3D += new Vector3((StackIndicator.GetComponent<RectTransform>().rect.width / 2)-IconWidth/2, 0, 0);

        InStack[storedCount].SetActive(true);

        //스택통따라 움직이기

        //현재 위치 : 스택통 맨위 -> 수정 위치 : 현 위치 - 스택 통 길이 + 프리팹 크기 + (몇번째인지)
        InStack[storedCount].GetComponent<RectTransform>().anchoredPosition3D += new Vector3(-StackIndicator.GetComponent<RectTransform>().rect.width + IconWidth + IconWidth*storedCount, 0, 0);

        //InStack[storedCount].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        //InStack[storedCount].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
        //InStack[storedCount].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
        storedCount++;
    }

    // Update is called once per frame
    void Update()
    {
    }
    //Push Btn OnClick 될 때 동작시킬 함수
    public void PushOnClicked()
    {
        //
    }

    public void Test()
    {
        Debug.Log("스택으로 움직이기 위한 함수 호출");

        //클릭 되었던 방향 버튼 정보 가져와서 확대 Or 색 변경으로 강조
        //Debug.Log(blockName);
        //
        //클릭된 방향 버튼 객체 자체나 Transform을 인자로 받아올 수 있을지?
        //받아왔다면 그 좌표가 궤적이동 시작점, 그 객체 자체가 궤적이동 대상 객체
        //객체 복사해서 던지기
        //궤적 이동
        //목표 위치값까지 포물선 궤적으로 오브젝트 날리기
        //스택 박스에 저장 - 이미 담겨있는 것에 따라 구분 필요
        //이동 기록의 Text Input 숫자에 따라서 배치 될 공간 설정.
    }
    //도착한 스택들 관리
    void Dst()
    {
    }

    //Finish

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
}

