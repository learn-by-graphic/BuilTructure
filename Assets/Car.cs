using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Car : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        carMove(Vector3Int.up , 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void carMove(Vector3Int vec , int steps)
    {
        /*
         * 0.62f (차 이동 보정수치)

        Vector3Int.right -> 우상단 이동
        Vector3Int.Left -> 좌하단 이동
        Vector3Int.up -> 좌상단 이동
        Vector3Int.down -> 우하단 이동
         */
        Tilemap tilemap = transform.parent.GetComponent<Tilemap>();
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

        cellPosition += vec * steps;
        transform.position = tilemap.GetCellCenterWorld(cellPosition) + (Vector3.up * 0.62f);
    }
}
