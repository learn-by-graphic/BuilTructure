using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;
using UnityEngine.SceneManagement;



public class Car_Queue : MonoBehaviour
{
    public GameObject complete;
    public GameObject warning;
    private AudioSource completeSound;
    public Sprite[] spriteArray;
    Vector3Int dstpoint;
    Tilemap Road;
    SpriteRenderer spriteRenderer;
    Vector3 startpoint;

    // Start is called before the first frame update
    void Start()
    {
        startpoint = transform.position;    //자동차 시작 위치 좌표
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Road = transform.parent.GetComponent<Tilemap>();
        dstpoint = getDstPoint();
        completeSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    /*    void Update() //도착지점 보이게 한건데 Update 순서 수정 가능
        {
            DarkMap.SetTile(dstpoint, WhiteTile);
        }*/

    Vector3Int getDstPoint()
    {
        GameObject dstPoint = GameObject.Find("DstPoint");
        Vector3Int dstCellp = Road.WorldToCell(dstPoint.transform.position);
        
        return dstCellp;
    }

    //이동불가 될 때 초기 위치로 되돌리기
    void carMoveToStartPoint()
    {
        transform.position = startpoint;            //자동차 시작 위치로
        spriteRenderer.sprite = spriteArray[0];     //자동차 머리 방향 처음 방향대로
    }

    void carMove(Vector3Int vec, int steps)
    {
        /*
         * 0.62f (차 이동 보정수치)

        Vector3Int.right -> 우상단 이동
        Vector3Int.Left -> 좌하단 이동
        Vector3Int.up -> 좌상단 이동
        Vector3Int.down -> 우하단 이동
         */

        Vector3Int cellPosition = Road.WorldToCell(transform.position);
        cellPosition += vec * steps;
        if (!Road.HasTile(cellPosition))
        {
            cellPosition -= vec * steps;
            warning.SetActive(true);
            Debug.Log("이동불가"); 
            completeSound.Play();
            carMoveToStartPoint();  //이동불가 되면 원위치로 
            return;
        }
        setSprite(cellPosition, vec);
        transform.position = Road.GetCellCenterWorld(cellPosition) + (Vector3.up * 0.62f);



        if (cellPosition == dstpoint)
        {
            Debug.Log("도착");
            complete.SetActive(true);
            completeSound.Play();
            return;
        }

    }
    public void carMoveRight()
    {
        carMove(Vector3Int.right, 1);
    }
    public void carMoveLeft()
    {
        carMove(Vector3Int.left, 1);
    }
    public void carMoveUp()
    {
        carMove(Vector3Int.up, 1);
    }
    public void carMoveDown()
    {
        carMove(Vector3Int.down, 1);
    }

    private void setSprite(Vector3Int cellpos, Vector3Int vec)
    {
        if (vec == Vector3Int.right) //우상단 이동 -> 우하단, 좌상단 시야
        {
            spriteRenderer.sprite = spriteArray[0];
        }
        else if (vec == Vector3Int.up) //좌상단 이동
        {
            spriteRenderer.sprite = spriteArray[2];
        }
        else if (vec == Vector3Int.down) // 우하단
        {
            spriteRenderer.sprite = spriteArray[3];
        }
        else if (vec == Vector3Int.left) // 좌하단
        {
            spriteRenderer.sprite = spriteArray[1];
        }
    }
}
