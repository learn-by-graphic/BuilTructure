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
    public Player player;

    public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    //variable for pathfind
    private Vector3Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;

    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;



    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "ground_grass"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green2"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red2"));
        tileBases.Add(TileType.Road, Resources.Load<TileBase>(tilePath + "road2"));
        tileBases.Add(TileType.Black, Resources.Load<TileBase>(tilePath + "black2"));
        tileBases.Add(TileType.Transparency, Resources.Load<TileBase>(tilePath + "transparency"));

    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            if (!temp.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);


                if (prevPos != cellPos)
                {
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
                SetTilesBlock(road, TileType.Empty, TempTilemap);
                SetTilesBlock(road, TileType.Road, MainTilemap);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
                Debug.Log(temp.area);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!temp.Placed)
            {
                Destroy(temp.gameObject);
                ClearArea();
            }

        }
    }

    #endregion

    #region Tilemap Management

    public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }


    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }


    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject building)
    {
        if (!temp || temp.Placed)
        {
            temp = Instantiate(building, new Vector3(0f, 0.5f, 0f), Quaternion.identity).GetComponent<Building>();
            FollowBuilding();
        }
        else
        {
            Debug.Log("현재 건물 먼저 배치해 주세요");
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakearea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("여기에 설치할 수 없습니다.");
                return false;
            }
        }

        return true;
    }
    public bool CheckEnoughResource(Building building)
    {
        if ((player.population >= building.populationNeed)
            && (player.energy >= building.energyNeed)
            && (player.wood >= building.woodNeed)
            && (player.stone >= building.stoneNeed)
            && (player.iron >= building.ironNeed))
        {
            player.population -= building.populationNeed;
            player.energy -= building.energyNeed;
            player.wood -= building.woodNeed;
            player.stone -= building.stoneNeed;
            player.iron -= building.ironNeed;
            return true;
        }
        return false;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Transparency, MainTilemap);
    }

    #endregion

    #region Pathfinding

    public void Settings()
    {


        //Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3Int cellPos = gridLayout.LocalToCell(touchPos); 
        // start / dst need to update by mouse input

        Building start_building = GameObject.Find("start").GetComponent<Building>();
        BoundsInt start_pos = start_building.area;
        startPos = start_pos.position;


        Building dst_building = GameObject.Find("dst").GetComponent<Building>();
        BoundsInt dst_pos = dst_building.area;
        targetPos = dst_pos.position;




        // 항상 전체 맵크기를 떠 올 수없음 
        // 출발 과 도착지 에서 특정 벡터를 구하고 맵 범위를 지정하는 과정이 필요함
        bottomLeft = new Vector3Int(-10, -10, 0);
        topRight = new Vector3Int(10, 10, 0);

        Debug.Log("start : " + startPos);
        Debug.Log("dst : " + targetPos);
        PathFinding();
    }

    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        BoundsInt tiles_area = new BoundsInt(bottomLeft, size: new Vector3Int(sizeX, sizeY, 1)); // 좌표 환산할 맵 크기를 결정하는 부분임 
        //BoundsInt tiles_area = MainTilemap.    
        TileBase[] tiles = GetTilesBlock(tiles_area, MainTilemap);

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = true;

                if (tiles[(j * sizeX) + i] == tileBases[TileType.Road])
                {
                    //Debug.Log("detect road");
                    isWall = false;
                }
                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);



            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count; i++)
                {
                    Vector3Int pathroad = new Vector3Int(FinalNodeList[i].x, FinalNodeList[i].y, 1);
                    BoundsInt road_area = new BoundsInt(pathroad, size: new Vector3Int(1, 1, 1));
                    // SetTilesBlock(road_area, TileType.Empty, TempTilemap); //이게 지금 색이 road보다 덮어져 쓰여지지가 않는데 이유를 모르겠음 
                    SetTilesBlock(road_area, TileType.Black, TempTilemap);
                    //print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                }
                Debug.Log("shortest path: " + (FinalNodeList.Count - 2));
                return;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;
                OpenList.Add(NeighborNode);
            }
        }
    }

    [System.Serializable]
    public class Node
    {
        public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

        public bool isWall;
        public Node ParentNode;

        // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
        public int x, y, G, H;
        public int F { get { return G + H; } }
    }

    #endregion 
}

public enum TileType
{
    Empty,
    White,
    Green,
    Red,
    Road,
    Black,
    Transparency
}
