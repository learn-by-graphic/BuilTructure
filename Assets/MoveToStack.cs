using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Tilemaps;
using System.Threading;
public class MoveToStack : MonoBehaviour
{
    //car
    GameObject canvas;
    public GameObject Grid;
    public Car car;
    //타일맵
    public Tilemap Ground;
    Tilemap Road;
    Vector3Int cellPosition;

    //프리팹
    public GameObject prefab; //푸쉬 누르기 직전의 방향 프리팹
    public RectTransform prefabRect; // 캔버스 하위의 이동시킬 Rect (HP Bar 등)

    int storedNum = 0;  //스택에 들어있는 개수

    // Start is called before the first frame update
    void Start()
    {
        /*
        Grid = GameObject.Find("Grid");
        car = Grid.GetComponent<Car>();
        Road = transform.parent.GetComponent<Tilemap>();
        cellPosition = Road.WorldToCell(car.transform.position);
        string tmp = cellPosition.ToString();
        Debug.Log(tmp);*/
        //prefabPos = PrefabPos();
        //if 방향 버튼 눌리면, PreviousMove를 그 방향으로
        //if 방향 버튼 클릭

        //btnUp = transform.GetChild(0).GetComponent<MoveUpButton>();
        //btnUp = GameObject.Find("MoveUpButton") as Button;
        //MoveUpBtn.onClickAddListener(MoveBtnClicked);
    }

    bool isRoad(Vector3Int vec)
    {
        cellPosition += vec;
        if (Road.HasTile(cellPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //각 화살표 방향이 클릭 되었을 때 걔네 정보를 가져올 Onclick 함수
    public void MoveUpOnClicked()
    {
        //시작전에 이미 있는 프리펩 지우기
        Debug.Log("UP 버튼 누름");

        MakePrefab("upblock");
        /*
        //차가 이동불가 일 때, 생성 안하는 방법!
        if (isRoad(Vector3Int.up))
        {
            Debug.Log("갈 수 있는 칸 - 스택에 던지기");
            //프리펩 생성, 리소스로 임포트해서 생성
            MakePrefab("upblock");
        }
        else
        {
            Debug.Log("갈 수 없는 칸");
        }*/
    }

    public void MoveDownOnClicked()
    {
        //SelectedMove = GameObject.Find("downblock");
        Debug.Log("Down 버튼 누름");
    }
    public void MoveRightOnClicked()
    {
        //SelectedMove = GameObject.Find("rightblock");
        Debug.Log("Right 버튼 누름");
    }
    public void MoveLeftOnClicked()
    {
        //SelectedMove = GameObject.Find("leftblock");
        Debug.Log("Left 버튼 누름");
    }

    //프리팹 - 클릭된 방향의 아이템 생성 (스택으로 들어갈 친구 만들기)

    public void MakePrefab(string blockName)
    {
        //프리팹 생성
        prefab = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockName), new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        //프리팹 좌표 조정
        //프리펩이 생성되는 위치 조정
        prefabRect = prefab.GetComponent<RectTransform>();
        prefabRect.anchoredPosition3D = new Vector3 (-75, -236, 0);
    }

    public void WhenClicked()
    {
        //클릭되면 그 오브젝트 이름에 따라 오브젝트 생성
    }

    // Update is called once per frame
    void Update()
    {
    }
    //Push Btn OnClick 될 때 동작시킬 함수
    void PushClicked()
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
    void Arrived()
    {
        Debug.Log("도착");
        //Destroy(gameObject);
    }
    //도착한 스택들 관리
    void Dst()
    {
    }
}

