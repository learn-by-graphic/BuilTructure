using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;
    
    

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public GameObject BtnGroup;
    public GameObject ynBtnGroup;
    public Tilemap Env;
    public Tile[] tiles; //tiles 0: ground / 1: green / 2: red / 3: white
    public GameObject graphmanager;

    private Building temp;
    private Vector3Int prevPos= new Vector3Int(-3,7,0);
    private BoundsInt prevArea;
    private TileBase prevtile;
    private TileBase envtile;
    private TileBase ptile;

    private List<Building> buildings; 
    private bool destroy_building_button = false;
    private bool no_btn = false;
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
        // if (!temp)
        // {
        //     return;
        // }
        graphmanager = GameObject.Find("GraphManager");
        
        for(int i = 0; i< Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if(touch.phase == TouchPhase.Began)
            {
                if(EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    prevPos = new Vector3Int(-3, 7, 0);
                    prevtile = MainTilemap.GetTile(prevPos);
                    return;
                }
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                prevPos = new Vector3Int(-3, 7, 0);
                prevtile = MainTilemap.GetTile(prevPos);
                return;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(destroy_building_button)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                try
                {
                graphmanager.GetComponent<GraphManager>().SearchBuilding(cellPos).destroy_building();

                graphmanager.GetComponent<GraphManager>().RemoveBuilding(cellPos);

                MainTilemap.SetTile(cellPos, tiles[0]);
                }
                catch(NullReferenceException)
                {
                    Debug.Log("삭제할 건물이 없습니다");
                }
                finally
                {
                destroy_building_button = false;
                }


            }
            //mouse
            if (EventSystem.current.IsPointerOverGameObject(-1))
            {
                if(no_btn)
                {
                    if (!temp.Placed)
                    {
                        TempTilemap.SetTile(prevPos, null);
                        MainTilemap.SetTile(prevPos, prevtile);
                        Destroy(temp.gameObject);
                    }
                    BtnGroup.SetActive(true);
                    no_btn = false;
                    return;
                }
                prevPos = new Vector3Int(-3, 7, 0);
                prevtile = MainTilemap.GetTile(prevPos);
                return;
            }
            /*
            // touch
            if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ){
                if(EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    if(no_btn)
                    {
                        if (!temp.Placed)
                        {
    
                            TempTilemap.SetTile(prevPos, null);
                            MainTilemap.SetTile(prevPos, prevtile);
                            Destroy(temp.gameObject);
                        }
                        BtnGroup.SetActive(true);
                        no_btn = false;
                        return;
                    }
                    prevPos = new Vector3Int(-3, 7, 0);
                    prevtile = MainTilemap.GetTile(prevPos);
                    return;
                }
                    
            }
            */

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
                    ynBtnGroup.SetActive(true);
                    
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
                //buildings.Add(temp);
                graphmanager.GetComponent<GraphManager>().AddBuilding(temp);
                BtnGroup.SetActive(true);
            }
        }
        else if (no_btn)
        {

            if (!temp.Placed)
            {
                TempTilemap.SetTile(temp.area.position, null);
                MainTilemap.SetTile(temp.area.position, ptile);
                Destroy(temp.gameObject);
            }
            BtnGroup.SetActive(true);
            no_btn = false;
        }

    }

    #endregion

    public void destroy_build_button()
    {
        this.destroy_building_button = true;

    }

    public void yesBtn()
    {
        if (temp.CanBePlaced())
        {
            temp.Place();
            //buildings.Add(temp);
            graphmanager.GetComponent<GraphManager>().AddBuilding(temp);
            BtnGroup.SetActive(true);
            ynBtnGroup.SetActive(false);
        }

    }
    public void noBtn()
    {
        
        no_btn = true;
        ynBtnGroup.SetActive(false);

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
        ptile = prevtile;
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