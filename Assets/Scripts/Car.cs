using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;



public class Car : MonoBehaviour
{
    public Sprite[] spriteArray;
    public Tilemap DarkMap;
    public Tile WhiteTile;
    Vector3Int dstpoint;
    Tilemap Road;
    SpriteRenderer spriteRenderer;
    public bool flag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Road = transform.parent.GetComponent<Tilemap>();
        dstpoint = getDstPoint();
        DarkMap.SetTile(dstpoint, WhiteTile);
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
            Debug.Log("이동불가");
            flag = false;
            return;
        }
        
        setWhiteTile(cellPosition , vec);
        transform.position = Road.GetCellCenterWorld(cellPosition) + (Vector3.up * 0.62f);
        flag = true;



        if (cellPosition == dstpoint)
        {
            Debug.Log("도착");
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

    private void setWhiteTile(Vector3Int cellpos, Vector3Int vec)
    {
        DarkMap.SetTile(cellpos, WhiteTile);
        if (vec == Vector3Int.right) //우상단 이동 -> 우하단, 좌상단 시야
        {
            spriteRenderer.sprite = spriteArray[0];
            DarkMap.SetTile(cellpos + Vector3Int.up, WhiteTile);
            DarkMap.SetTile(cellpos + Vector3Int.down, WhiteTile);
        }
        else if(vec == Vector3Int.up) //좌상단 이동
        {
            spriteRenderer.sprite = spriteArray[2];
            DarkMap.SetTile(cellpos + Vector3Int.right, WhiteTile);
            DarkMap.SetTile(cellpos + Vector3Int.left, WhiteTile);
        }
        else if (vec == Vector3Int.down) // 우하단
        {
            spriteRenderer.sprite = spriteArray[3];
            DarkMap.SetTile(cellpos + Vector3Int.right, WhiteTile);
            DarkMap.SetTile(cellpos + Vector3Int.left, WhiteTile);
        }
        else if (vec == Vector3Int.left) // 좌하단
        {
            spriteRenderer.sprite = spriteArray[1];
            DarkMap.SetTile(cellpos + Vector3Int.up, WhiteTile);
            DarkMap.SetTile(cellpos + Vector3Int.down, WhiteTile);
        }
    }
}
