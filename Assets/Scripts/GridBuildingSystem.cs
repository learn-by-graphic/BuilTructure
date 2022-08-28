using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public GameObject BtnGroup;
    public Tilemap Env;
    public Tile[] tiles; //tiles 0: ground / 1: green / 2: red / 3: white

    private Building temp;
    private Vector3Int prevPos= new Vector3Int(-3,7,0);
    private BoundsInt prevArea;
    private TileBase prevtile;
    private TileBase envtile;

    private List<Building> buildings = new List<Building>(); 
    private bool destroy_building_button = false;
    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //tiles 0: ground / 1: green / 2: red / 3: white
    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            if(destroy_building_button)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                for(int i=0; i<buildings.Count; i++)
                {
                    if(cellPos == buildings[i].area.position)
                    {
                        buildings[i].GetComponent<Building>().destroy_building();
                        MainTilemap.SetTile(cellPos, tiles[0]);
                        buildings.RemoveAt(i);
                        destroy_building_button = false;
                        break;
                    }
                }
            }
            if (EventSystem.current.IsPointerOverGameObject(-1))
            {
                prevPos = new Vector3Int(-3, 7, 0);
                prevtile = MainTilemap.GetTile(prevPos);
                return;
            }

            else if (!temp.Placed && temp)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);


                if (prevPos != cellPos)
                {
                    if(prevPos != new Vector3Int(-3,7,0))
                    {
                        TempTilemap.SetTile(prevPos, null);
                        MainTilemap.SetTile(prevPos, prevtile);
                    }
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos +
                        new Vector3(0.5f, 0.5f, 0f));
                    
                    prevPos = cellPos;
                    FollowBuilding();
                    
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            if (prevPos != cellPos)
            {
                prevPos = cellPos;
                TileBase TileB = MainTilemap.GetTile(cellPos);
                BoundsInt road = new BoundsInt(cellPos, size: new Vector3Int(1, 1, 1));
                // 도로도 건물처럼 겹치게 깔리면 안되니까 그 처리가 필요함 
                // 현재는 그냥 깔리기만 하는 상태
                Debug.Log("tile setting");
                //SetTilesBlock(road, TileType.Empty, TempTilemap);
                //SetTilesBlock(road, TileType.Road, MainTilemap);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
                Debug.Log(temp.area);
                buildings.Add(temp);
                BtnGroup.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {


            if (!temp.Placed)
            {
                TempTilemap.SetTile(prevPos, null);
                MainTilemap.SetTile(prevPos, prevtile);
                Destroy(temp.gameObject);
            }
            BtnGroup.SetActive(true);
        }

    }

    #endregion

    public void destroy_build_button()
    {
        this.destroy_building_button = true;
    }


    #region Building Placement

    public void InitializeWithBuilding(GameObject building)
    {
        if (!temp || temp.Placed)
        {
            temp = Instantiate(building, new Vector3(-14.15f, 4.19f, 0f), Quaternion.identity).GetComponent<Building>();
            temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
            temp.transform.position += new Vector3(0, 0.46f, 0);
            //초기 건물 생성위치 지정
            //버튼 숨김 
            BtnGroup.SetActive(false);
        }
        else
        {
            Debug.Log("현재 건물 먼저 배치해 주세요");
        }
    }


    private void FollowBuilding()
    {

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        temp.transform.position += new Vector3(0, 0.46f, 0);
        BoundsInt buildingArea = temp.area;
        
        prevtile = MainTilemap.GetTile(temp.area.position);
        envtile = Env.GetTile(temp.area.position);
        if((prevtile.name == "ground") && ReferenceEquals(envtile , null))
        {
            TempTilemap.SetTile(temp.area.position , tiles[1]);
            MainTilemap.SetTile(temp.area.position, null);
        }
        else
        {
            TempTilemap.SetTile(temp.area.position , tiles[2]);
            MainTilemap.SetTile(temp.area.position, null);
        }
        prevArea = buildingArea;
    }

    public bool CanTakearea(BoundsInt area)
    {
        TileBase temptile = TempTilemap.GetTile(area.position);
        if(temptile.name == "green")
        {
            return true;
        }
        return false;
    }

    public void TakeArea(BoundsInt area)
    {
        TempTilemap.SetTile(area.position, null);
        MainTilemap.SetTile(area.position, tiles[3]);
    }

    #endregion
}