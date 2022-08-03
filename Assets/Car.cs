using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;

public class Car : MonoBehaviour
{
    Vector3Int dstpoint;
    Tilemap tilemap;
    
    // Start is called before the first frame update
    void Start()
    {
        tilemap = transform.parent.GetComponent<Tilemap>();
        dstpoint = getDstPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3Int getDstPoint()
    {
        GameObject dstPoint = GameObject.Find("DstPoint");
        Vector3Int dstCellp = tilemap.WorldToCell(dstPoint.transform.position);

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
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        cellPosition += vec * steps;
        transform.position = tilemap.GetCellCenterWorld(cellPosition) + (Vector3.up * 0.62f);

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
}
