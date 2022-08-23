using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class dragIndicator : MonoBehaviour
{

    private bool button_clicked = false;


    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    private Vector3Int prevPos;

    void Start()
    {
    }
    void Update()
    {
        if (button_clicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                Debug.Log(cellPos);
                prevPos = cellPos;
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                if(prevPos != cellPos)
                {
                    Debug.Log("moved to" +cellPos);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {

            }
        }
    }

    public void btn_click()
    {
        this.button_clicked = true;
         // 도로 배치 후 false 로 바꿔줘야함
    }

}
