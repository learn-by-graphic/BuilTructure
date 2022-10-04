using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortScript : MonoBehaviour
{
    public GameObject BlockToMove;  //현재 블록, 자리를 옮겨 줄
    public GameObject BlockToBeMoved;   //위치가 바뀌어질 대상 블록



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void SwitchLocation()
    {
        //when block 1,2 is Clicked OR switch button 'YES' is Clicked
        //위치 바꿔줄 블록을 선택하기 위한 사용자 동작

        //Block GameObjects that have to be switched
        //위치 바꿀 블록 선정
        BlockToMove = GameObject.Find("box1");
        BlockToBeMoved = GameObject.Find("box2");

        BlockToMove.AddComponent<SortBlockMove>();
        BlockToMove.GetComponent<SortBlockMove>().letsMove(1, BlockToMove.transform.position, BlockToBeMoved.transform.position);

        BlockToBeMoved.AddComponent<SortBlockMove>();
        BlockToBeMoved.GetComponent<SortBlockMove>().letsMove(1, BlockToBeMoved.transform.position, BlockToMove.transform.position);



    }
}




