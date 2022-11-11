using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BfsManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap MainTilemap;
    public Tilemap DarkMap;
    public Tile DarkTile;
    public GameObject current_pos;

    public GameObject greenMask;
    public GameObject[] greenTR;
    public GameObject[] greenTL;
    public GameObject[] greenBR;
    public GameObject[] greenBL;
    public GameObject btn_green;
    public GameObject btn_yellow;

    public Vector3Int root_position = new Vector3Int(-1,-1,0);
    private Vector3Int cellPos;
    public List<Vector3Int> yellow_positions;
    private int phase_count = 0;
    private string tile_name;

    void Awake()
    {
        DarkMap.origin = MainTilemap.origin;
        DarkMap.size = MainTilemap.size;
        foreach (Vector3Int p in DarkMap.cellBounds.allPositionsWithin)
        {
            DarkMap.SetTile(p, DarkTile);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cellPos = grid.LocalToCell(touchPos);
            
            Debug.Log(cellPos);
            if(cellPos == root_position)
            {
                greenMask.SetActive(true);
                current_pos.transform.position = grid.GetCellCenterWorld(cellPos);
                if(phase_count == 0) 
                    phase_count = 1;
            }


            if(phase_count >= 1 && cellPos == new Vector3Int(4,-1,0))
            {
                greenTR[0].SetActive(true);
                current_pos.transform.position = grid.GetCellCenterWorld(cellPos);
                btn_green.SetActive(true);
            }
            else if(phase_count >= 1 && cellPos == new Vector3Int(-1,4,0))
            {
                greenTL[0].SetActive(true);
                current_pos.transform.position = grid.GetCellCenterWorld(cellPos);
                btn_green.SetActive(true);
            }
            else if(phase_count >= 1 && cellPos == new Vector3Int(-1,-6,0))
            {
                greenBR[0].SetActive(true);
                current_pos.transform.position = grid.GetCellCenterWorld(cellPos);
                btn_green.SetActive(true);
            }
            else if(phase_count >= 1 && cellPos == new Vector3Int(-6,-1,0))
            {
                greenBL[0].SetActive(true);
                current_pos.transform.position = grid.GetCellCenterWorld(cellPos);
                btn_green.SetActive(true);
            }
            else if(phase_count == 2){
                  last_phase(cellPos);
            }
            

            phase_count = checkphase(phase_count, cellPos);
            //Debug.Log(phase_count);
            //정답 입력 란 오픈해서 값 들어올때 phase_count 3으로 바꾸고 종료 페이즈로 뛰면됨
        }
    }

    int checkphase(int count, Vector3Int cell)
    {
        if(count == 1)
        {
            if(greenTR[0].activeSelf && greenTL[0].activeSelf && greenBR[0].activeSelf && greenBL[0].activeSelf)
            {
                return 2;
            }
            else{
                if(!((cell == new Vector3Int(4,-1,0)) || (cell == new Vector3Int(-1,4,0)) || (cell == new Vector3Int(-1,-6,0)) || (cell == new Vector3Int(-6,-1,0)) ||(cell == root_position))){
                    Debug.Log("BFS 순서를 지켜주세요");
                }
                return 1;
            }
        }
        else if(count >= 2){
            if(greenTR[2].activeSelf)
                return 2;
            else
                return 2;
        }
        else{
            return 0;
        }
    }

    void last_phase(Vector3Int pos)
    {
        if(pos == new Vector3Int(9,2,0))
        {
            greenTR[1].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(8,-5,0))
        {
            greenTR[2].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(3,8,0))
        {
            greenTL[1].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(-5,8,0))
        {
            greenTL[2].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(-5,-10,0))
        {
            greenBR[1].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(3,-10,0))
        {
            greenBR[2].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(-5,-6,0))
        {
            greenBR[3].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(-10,3,0))
        {
            greenBL[1].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(-10,-5,0))
        {
            greenBL[2].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
        else if(pos == new Vector3Int(-6,3,0))
        {
            greenBL[3].SetActive(true);
            current_pos.transform.position = grid.GetCellCenterWorld(pos);
            btn_yellow.SetActive(true);
        }
    }

    public void btn_green_push()
    {
        current_pos.transform.position = grid.GetCellCenterWorld(root_position);
        btn_green.SetActive(false);
        
    }
    
    public void btn_yellow_push()
    {
        if(cellPos == new Vector3Int(9,2,0) || cellPos == new Vector3Int(8,-5,0))
        {
            current_pos.transform.position = grid.GetCellCenterWorld(new Vector3Int(4,-1,0));
            btn_yellow.SetActive(false);
            btn_green.SetActive(true);
        }
        else if(cellPos == new Vector3Int(3,8,0) || cellPos == new Vector3Int(-5,8,0))
        {
            current_pos.transform.position = grid.GetCellCenterWorld(new Vector3Int(-1,4,0));
            btn_yellow.SetActive(false);
            btn_green.SetActive(true);
        }
        else if(cellPos == new Vector3Int(-5,-6,0) || cellPos == new Vector3Int(-5,-10,0) || cellPos == new Vector3Int(3,-10,0))
        {
            current_pos.transform.position = grid.GetCellCenterWorld(new Vector3Int(-1,-6,0));
            btn_yellow.SetActive(false);
            btn_green.SetActive(true);
        }
        else if(cellPos == new Vector3Int(-6,3,0) || cellPos == new Vector3Int(-10,3,0) || cellPos == new Vector3Int(-10,5,0))
        {
            current_pos.transform.position = grid.GetCellCenterWorld(new Vector3Int(-6,-1,0));
            btn_yellow.SetActive(false);
            btn_green.SetActive(true);
        }
    }
}
