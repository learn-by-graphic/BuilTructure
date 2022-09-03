using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Threading;
using System.Linq;

public class dragIndicator : MonoBehaviour
{

    private bool button_clicked = false;
    public GameObject graphmanager;


    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public Tilemap Env;
    public GameObject confirm_button;
    public GameObject method_button;
    public Tile[] tiles; // 0: green ,1: red , 2: yellow
    public Tile[] road_tiles; // 0 : x , 1: y , 2: x_left , 3: x_right , 4: y_left , 5: y_right

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return_initial();
            if (start_is_red)
            {
                MainTilemap.SetTile(startPos, start_beforetile);
                TempTilemap.SetTile(startPos, null);
                start_is_red = false;
            }
            cellPos_of_selectedTile.Clear();
            selectedTile.Clear();
            method_button.SetActive(true);
        }
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
                if ((start_beforetile.name == "white") && ReferenceEquals(start_envtile, null))
                {
                    TempTilemap.SetTile(startPos, tiles[0]);
                    MainTilemap.SetTile(startPos, null);
                    //Debug.Log("start: " + startPos);
                    cellPos_of_selectedTile.Add(startPos);
                    selectedTile.Add(start_beforetile);
                }
                else
                {
                    TempTilemap.SetTile(startPos, tiles[1]);
                    MainTilemap.SetTile(startPos, null);
                    button_clicked = false;
                    method_button.SetActive(true);
                    start_is_red = true;
                    Debug.Log("Error(start is red) back to initial");
                    Debug.Log("press esc key or install road button");
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
                TileBase endtile = selectedTile[selectedTile.Count-1];

                if(endtile.name != "white")
                {
                    TempTilemap.SetTile(endPos, tiles[1]);
                }
                else
                {
                    TempTilemap.SetTile(endPos, tiles[0]);
                }

                if(has_red_tile()) //white tile에 대한 조건 추가 필요(check_building_connection)
                {
                    Debug.Log("Error(has red(yellow) tile) back to initial");
                    Debug.Log("press esc key or install road button");
                }
                else if(has_diagonal_path())
                {
                    Debug.Log("Error(has diagonal tile) back to initial");
                    Debug.Log("press esc key or install road button");

                }
                else
                {
                    //print_List_of_path();
                    confirm_button.SetActive(true);
                }
                //need extra button => yes / no 
                
                // 최종 도로 설치시 초록도로를 -> 스프라이트로 변경 
                // 도로 설치 실패 or 거부의사 -> 타일들을 복귀 시킴 

                button_clicked = false;
                
                //Debug.Log("terminated installing road");
            }
        }
    }

    public void btn_click()
    {
        this.button_clicked = true;
        method_button.SetActive(false);
        return_initial();
        //Debug.Log("started installing road");
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
            if (((nexttile.name == "ground")||(nexttile.name == "white")) && ReferenceEquals(envtile, null))
            {

                if(nexttile.name == "white")
                {
                TempTilemap.SetTile(next, tiles[2]);
                MainTilemap.SetTile(next, null); 
                }
                else
                {
                TempTilemap.SetTile(next, tiles[0]);
                MainTilemap.SetTile(next, null);
                }

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
        for(int i=1; i<cellPos_of_selectedTile.Count-1; i++)
        {
            TileBase tmp = TempTilemap.GetTile(cellPos_of_selectedTile[i]);
            if (tmp.name == "yellow")
                return true;
        }
        return false;
    }
    bool has_diagonal_path()
    {
        for(int i=0; i<cellPos_of_selectedTile.Count-1; i++)
        {
            float mag = Vector3Int.Distance(cellPos_of_selectedTile[i], cellPos_of_selectedTile[i + 1]);
            if (mag > 1.0f)
            {
                TempTilemap.SetTile(cellPos_of_selectedTile[i], tiles[1]);
                TempTilemap.SetTile(cellPos_of_selectedTile[i+1], tiles[1]);
                return true;
            }
        }
        return false;
    }


    public void button_yes()
    {
        install_roads();
        graphmanager.GetComponent<GraphManager>().Addroads(cellPos_of_selectedTile[0], cellPos_of_selectedTile[cellPos_of_selectedTile.Count-1],cellPos_of_selectedTile);
        //graphmanager.GetComponent<GraphManager>().show_every_build();
        //Debug.Log("----------------------");
        // Debug.Log("start's road ");
        // graphmanager.nodes[0].print_my_road();
        
        confirm_button.SetActive(false);
        cellPos_of_selectedTile.Clear();
        selectedTile.Clear();
        method_button.SetActive(true);
    }
    public void button_no()
    {
        return_initial();
        confirm_button.SetActive(false);
        method_button.SetActive(true);
    }
    void install_roads()
    {
        if(cellPos_of_selectedTile.Count < 3)
        {
            Debug.Log("Error(too short path) can't install road");
            TempTilemap.SetTile(cellPos_of_selectedTile[0], null);
            MainTilemap.SetTile(cellPos_of_selectedTile[0], selectedTile[0]); //white
            TempTilemap.SetTile(cellPos_of_selectedTile[cellPos_of_selectedTile.Count - 1], null);
            MainTilemap.SetTile(cellPos_of_selectedTile[cellPos_of_selectedTile.Count - 1], selectedTile[selectedTile.Count - 1]);
            return;
        }
        TempTilemap.SetTile(cellPos_of_selectedTile[0], null);
        MainTilemap.SetTile(cellPos_of_selectedTile[0], selectedTile[0]); //white
        TempTilemap.SetTile(cellPos_of_selectedTile[cellPos_of_selectedTile.Count - 1], null);
        MainTilemap.SetTile(cellPos_of_selectedTile[cellPos_of_selectedTile.Count - 1], selectedTile[selectedTile.Count-1]); // white

        Vector3Int prev_vec, mid_vec, next_vec, oper_vec;
        Vector3Int first_dirct, second_dirct;


        for(int i=0; i<cellPos_of_selectedTile.Count-2; i++) //start , end is white tile
        {
            prev_vec = cellPos_of_selectedTile[i];
            mid_vec = cellPos_of_selectedTile[i + 1];
            next_vec = cellPos_of_selectedTile[i + 2];

            oper_vec = new Vector3Int(Mathf.Abs(next_vec.x - prev_vec.x) , Mathf.Abs(next_vec.y - prev_vec.y), 0);
            first_dirct = mid_vec - prev_vec;
            second_dirct = next_vec - mid_vec;

            if(oper_vec == new Vector3Int(2,0,0))
            {
                TempTilemap.SetTile(mid_vec, null);
                MainTilemap.SetTile(mid_vec, road_tiles[0]);
            }
            else if(oper_vec == new Vector3Int(0,2,0))
            {
                TempTilemap.SetTile(mid_vec, null);
                MainTilemap.SetTile(mid_vec, road_tiles[1]);
            }
            else if(oper_vec == new Vector3Int(1,1,0)) // move 2 way
            {
                if(first_dirct == new Vector3Int(1, 0, 0))
                {
                    // 우상단
                    if(second_dirct == new Vector3Int(0,1,0))
                    {
                        //x_left
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[2]);
                    }
                    else
                    {
                        //x_right
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[3]);
                    }
                }

                else if(first_dirct == new Vector3Int(-1, 0, 0))
                {
                    //좌하단
                    if(second_dirct == new Vector3Int(0,1,0))
                    {
                        //y_left
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[4]);
                    }
                    else
                    {
                        //y_right
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[5]);
                    }
                }
                
                else if(first_dirct == new Vector3Int(0, 1, 0))
                {
                    //좌상단
                    if (second_dirct == new Vector3Int(1, 0, 0))
                    {
                        //y_right
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[5]);
                    }
                    else
                    {
                        //x_right
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[3]);
                    }
                }
                else
                {
                    //우하단
                    if (second_dirct == new Vector3Int(1, 0, 0))
                    {
                        //y_left
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[4]);
                    }
                    else
                    {
                        //x_left
                        TempTilemap.SetTile(mid_vec, null);
                        MainTilemap.SetTile(mid_vec, road_tiles[2]);
                    }
                }
            }
            else
            {
                Debug.Log("Error(wrong vector)");
                Debug.Log("press esc key or install road button");
                return;
            }
        }
        //terminate road install
        Debug.Log("road length : " + (cellPos_of_selectedTile.Count-2));
    }

}
