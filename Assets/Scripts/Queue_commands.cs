﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//transform = Fillarea
public class Queue_commands : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject prefab;
    public GameObject Parent;
    public GameObject car;
    private Vector2 create_point;
    private RectTransform rect_obj , Parent_rect;

    private static int count = 0;
    private int size = 20; // width 에 따라 변경?

    private GameObject[] queue; // 0 :up , 1: right , 2: left , 3: down

    GameObject FillArea;    //스택을 담을 공간
    float FillAreaWidth;    //공간의 너비
    Vector3 entrancePos;    //스택 통의 꼭대기 (입구의 Y좌표)

    void Awake()
    {
        //Car
        car = GameObject.Find("Car");
        prefabs = new GameObject[4];
        queue = new GameObject[size];

        //AtStack Method
        FillArea = GameObject.Find("FillArea");  //스택을 담을 공간
        FillAreaWidth = FillArea.GetComponent<RectTransform>().rect.width;           //스택 통의 전체 길이      (896)

    }
    public void createBlock(string blockName, int prefabIndex)
    {
        count++;
        queue[count-1] = Instantiate(Resources.Load<GameObject>("StackPrefabs/" + blockName), new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        //프리팹 크기 조정
        queue[count-1].GetComponent<RectTransform>().sizeDelta = new Vector2(88, 88);
        GameObject image = queue[count-1].transform.Find("Image").gameObject;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 60);
        //프리팹 좌표 조정
        Vector3 startVec = GameObject.Find("StartInArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
        Vector3 arriveVec = GameObject.Find("DstInArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
        queue[count-1].GetComponent<RectTransform>().anchoredPosition3D = startVec;     //프리펩 생성 위치 (궤적 이동 시작 위치)
        //프리펩에 무브 함수 달아서 궤적 이동 출발
        queue[count-1].AddComponent<BlockMove>();
        queue[count-1].GetComponent<BlockMove>().letsMove(4, startVec, arriveVec, blockName, count - 1);

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
    public void BtnOnClicked()      //꺼내는 버튼 눌렀을 때
    {
        if (count > 0)
        {
            count--;
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
            
            //젤 앞에 꺼 버리는 궤적 이동
            Vector3 startVec = GameObject.Find("StartOutArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 arriveVec = GameObject.Find("DstOutArcmove").transform.GetComponent<RectTransform>().anchoredPosition3D;
            queue[0].transform.SetParent(GameObject.Find("Canvas").transform);
            queue[0].GetComponent<RectTransform>().anchoredPosition3D = startVec;
            queue[0].GetComponent<BlockMove>().letsMove(6, startVec, arriveVec);
            //***필요하면 크기 조정
            //큐 정렬 (옆으로 한칸씩 이동)
            for (int i = 0; i < count; i++)
            {
                queue[i] = queue[i + 1];
                queue[i].GetComponent<RectTransform>().anchoredPosition3D += new Vector3(44, 0, 0);
            }
        }
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
