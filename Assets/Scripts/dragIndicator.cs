using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class dragIndicator : MonoBehaviour
{

    private bool button_clicked = false;


    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public Tilemap Env;
    public GameObject confirm_button; 
    public Tile[] tiles; // 0: green ,1: red

    private Vector3Int prevPos;
    private Vector3Int startPos, endPos;
    private List<Vector3Int> cellPos_of_selectedTile = new List<Vector3Int>();
    private List<TileBase> selectedTile = new List<TileBase>();
    private int already_index = 0;

    private bool start_is_red = false;
    private TileBase start_envtile;
    private TileBase start_beforetile;

    void Start()
    {
       
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))
        {
            return;
        }
        if (button_clicked)
        {
            
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                prevPos = cellPos;
                startPos = cellPos;

                start_beforetile = MainTilemap.GetTile(startPos);
                start_envtile = Env.GetTile(startPos);
                if ((start_beforetile.name == "ground") && ReferenceEquals(start_envtile, null)) // start before tile은 차후 white tile과 검사하게 됨
                {
                    TempTilemap.SetTile(startPos, tiles[0]);
                    MainTilemap.SetTile(startPos, null);
                    Debug.Log("start: " + startPos);
                    cellPos_of_selectedTile.Add(startPos);
                    selectedTile.Add(start_beforetile);
                }
                else
                {
                    TempTilemap.SetTile(startPos, tiles[1]);
                    MainTilemap.SetTile(startPos, null);
                    button_clicked = false;
                    start_is_red = true;
                    Debug.Log("Error(start is red) back to initial");
                }
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                if(prevPos != cellPos && !is_already_path(cellPos))
                {
                    draw_temptile(cellPos);

                    cellPos_of_selectedTile.Add(cellPos);
                }
                else if(is_already_path(cellPos))
                {
                    go_to_returning_point();
                }
                prevPos = cellPos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                endPos = cellPos;
                

                if(has_red_tile()) //white tile에 대한 조건 추가 필요(check_building_connection)
                {
                    Debug.Log("Error(has red tile) back to initial");
 
                }
                else
                {
                    print_List_of_path();
                    confirm_button.SetActive(true);
                }
                //need extra button => yes / no 
                
                // 최종 도로 설치시 초록도로를 -> 스프라이트로 변경 
                // 도로 설치 실패 or 거부의사 -> 타일들을 복귀 시킴 

                button_clicked = false;
                Debug.Log("terminate installing road");
            }
        }
    }

    public void btn_click()
    {
        this.button_clicked = true;
        return_initial();
        Debug.Log("starting install road");
         // 도로 배치 후 false 로 바꿔줘야함
        if(start_is_red)
        {
            MainTilemap.SetTile(startPos, start_beforetile);
            TempTilemap.SetTile(startPos, null);
            start_is_red = false;
        }
        cellPos_of_selectedTile.Clear();
        selectedTile.Clear();
    }

    void draw_temptile(Vector3Int next)
    {
        TileBase nexttile = MainTilemap.GetTile(next);
        TileBase envtile = Env.GetTile(next);
        if (!ReferenceEquals(nexttile, null))
        {
            selectedTile.Add(nexttile);
            if ((nexttile.name == "ground") && ReferenceEquals(envtile, null))
            {
                TempTilemap.SetTile(next, tiles[0]);
                MainTilemap.SetTile(next, null);

            }
            else
            {
                TempTilemap.SetTile(next, tiles[1]);
                MainTilemap.SetTile(next, null);
            }
        }
        else
        {
            return;
        }

    }
    void print_List_of_path()
    {
        for (int k = 0; k < cellPos_of_selectedTile.Count; k++)
            Debug.Log(cellPos_of_selectedTile[k]);
        Debug.Log("-----------------------------------------------");
        for (int k = 0; k < selectedTile.Count; k++)
            Debug.Log(selectedTile[k]);
    }
    bool is_already_path(Vector3Int Pos)
    {
        for(int index = 0; index<cellPos_of_selectedTile.Count; index++)
        {
            if(cellPos_of_selectedTile[index] == Pos)
            {
                already_index = index;
                return true;
            }
        }
        return false;
    }
    void go_to_returning_point()
    {
        for(int i = cellPos_of_selectedTile.Count-1; i>already_index; i--)
        {
            MainTilemap.SetTile(cellPos_of_selectedTile[i], selectedTile[i]);
            TempTilemap.SetTile(cellPos_of_selectedTile[i], null);
        }
        cellPos_of_selectedTile.RemoveRange(already_index + 1, cellPos_of_selectedTile.Count - (already_index + 1));
        selectedTile.RemoveRange(already_index + 1, selectedTile.Count - (already_index + 1));
        already_index = 0;
    }
    void return_initial()
    {
        for (int i = 0; i< cellPos_of_selectedTile.Count; i++)
        {
            MainTilemap.SetTile(cellPos_of_selectedTile[i], selectedTile[i]);
            TempTilemap.SetTile(cellPos_of_selectedTile[i], null);
        }
    }
    bool has_red_tile()
    {
        for(int i=0; i<cellPos_of_selectedTile.Count; i++)
        {
            TileBase tmp = TempTilemap.GetTile(cellPos_of_selectedTile[i]);
            if (tmp.name == "red")
                return true;
        }
        return false;
    }
    /*
    bool check_building_connection()
    {

    }
    */

    public void button_yes()
    {
        confirm_button.SetActive(false);
    }
    public void button_no()
    {
        return_initial();
        confirm_button.SetActive(false);
    }
    void direction_change()
    {

    }
}
