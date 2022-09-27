using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap MainTilemap;
    public GameObject[] tree_buildings_sprites;
    public Tile[] T_tiles; //0 : white 1: road_x 2: road_y 3: ground


    public int distance_of_cityhall = 5;
    public int distance_of_house = 4;
    public int distance_of_house2 = 3;
    private Tree_building tmp; // root
    private Tree_building tmp_node = null;
    public Vector3Int cityhall_position = new Vector3Int(0,0,0);

    private List<Tree_building> tree = new List<Tree_building>();
    private bool on_build_button = false;
    private bool on_del_button = false;
    private GameObject arrows , arrow_1, arrow_2, arrow_3, arrow_4;
    // Start is called before the first frame update
    void Start()
    {
        tmp = Instantiate(tree_buildings_sprites[0], grid.GetCellCenterWorld(cityhall_position)+new Vector3(0f,0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
        MainTilemap.SetTile(cityhall_position, T_tiles[0]);
        tmp.m_position = cityhall_position;
        tmp.p_position = new Vector3Int(0,0,0);
        tree.Add(tmp); // root
        arrows = GameObject.Find("Tree_Arrows");
        arrow_1 = GameObject.Find("LeftUp");
        arrow_2 = GameObject.Find("RightDown");
        arrow_3 = GameObject.Find("RightUp");
        arrow_4 = GameObject.Find("LeftDown");
        arrows.SetActive(false);
    }
    void Update()
    {
        if(on_build_button)
        {   
            
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = grid.LocalToCell(touchPos);
                


                build_button(cellPos);
                on_build_button = false;
                //arrows.SetActive(false);
            }
        }
        if(on_del_button)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = grid.LocalToCell(touchPos);
                
                del_button(cellPos);
                on_del_button = false;
            }
        }
    }

    public void push_build_button()
    {
        on_build_button = true;
    }
    public void push_del_button()
    {
        on_del_button = true;
    }

#region build
    public void build_button(Vector3Int cellPos)
    {
        
        if(MainTilemap.GetTile(cellPos).name != "white")
        {
            Debug.Log("건물이 설치된 타일만 선택해 주세요");
            return;
        }
        //arrow active
        arrows.transform.position = grid.GetCellCenterWorld(cellPos)+new Vector3(0f,0.45f,0f);
        //arrows.SetActive(true);
        arrows.SetActive(true);

        //search tmp node
        for(int i=0; i<tree.Count; i++)
        {
            if(tree[i].m_position == cellPos){
                tmp_node = tree[i];
                break;
            }
        }
        //tmp node size check
        if(!can_build(tmp_node)){
            Debug.Log("더이상 자식노드를 추가할 수 없습니다.");
            return;
        }
        if(tmp_node.name == "Tree_house2(Clone)")
        {
            Debug.Log("리프노드에는 자식노드를 추가할 수 없습니다");
            buttons_active();
            arrows.SetActive(false);
            return;
        }
        //build instance
        //build(tmp_node);
        build_arrow(tmp_node);
        
        //exit
    }

    private bool can_build(Tree_building tmp)
    {
        if(tmp.name == "Tree_cityhall(Clone)")
        {
            if(tmp.connected_buildings.Count < 4){
                return true;
            }
            else{
                return false;
            }
        }
        else //name == Tree_wardoffice
        {
            if(tmp.connected_buildings.Count < 3){
                return true;
            }
            else{
                return false;
        }   
        }
    }
/*
    private void build(Tree_building tmp)        //좌상단 0 1 0 우하단 0 -1 0 우상단 1 0 0 좌하단 -1 0 0
    {
        Vector3Int install_position = new Vector3Int(0,0,0);
        if(tmp.name == "Tree_cityhall(Clone)") 
        {
            if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(0 , 1 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(0 , distance_of_cityhall ,0);
            }
            else if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(0 , -1 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(0 , -distance_of_cityhall ,0);
            }
            else if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(1 , 0 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(distance_of_cityhall , 0 ,0);
            }
            else
            {
                install_position = tmp.m_position + new Vector3Int(-distance_of_cityhall , 0 ,0);
            }
            Tree_building installing_node = Instantiate(tree_buildings_sprites[1], grid.GetCellCenterWorld(install_position)+new Vector3(0f,0.3f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = cityhall_position;
            tree.Add(installing_node);
            tmp.connected_buildings.Add(install_position);
        }
        else if(tmp.name == "Tree_wardoffice(Clone)")
        {
            if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(0 , 1 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(0 , distance_of_house ,0);
            }
            else if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(0 , -1 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(0 , -distance_of_house ,0);
            }
            else if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(1 , 0 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(distance_of_house , 0 ,0);
            }
            else
            {
                install_position = tmp.m_position + new Vector3Int(-distance_of_house , 0 ,0);
            }
            Tree_building installing_node = Instantiate(tree_buildings_sprites[2], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp.m_position;
            tree.Add(installing_node);
            tmp.connected_buildings.Add(install_position);
        }
        else if(tmp.name == "Tree_house(Clone)")
        {
            if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(0 , 1 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(0 , distance_of_house2 ,0);
            }
            else if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(0 , -1 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(0 , -distance_of_house2 ,0);
            }
            else if(MainTilemap.GetTile(tmp.m_position + new Vector3Int(1 , 0 ,0)).name == "ground")
            {
                install_position = tmp.m_position + new Vector3Int(distance_of_house2 , 0 ,0);
            }
            else
            {
                install_position = tmp.m_position + new Vector3Int(-distance_of_house2 , 0 ,0);
            }
            Tree_building installing_node = Instantiate(tree_buildings_sprites[3], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp.m_position;
            tree.Add(installing_node);
            tmp.connected_buildings.Add(install_position);
        }
        else{ //tmp name == Tree_house
            Debug.Log("리프노드에는 자식노드를 추가할 수 없습니다");
            return;
        }

        build_road(tmp.m_position , install_position);
    }
*/
    private void buttons_active()
    {
        arrow_1.SetActive(true);
        arrow_2.SetActive(true);
        arrow_3.SetActive(true);
        arrow_4.SetActive(true);
    }
    private void build_arrow(Tree_building tmp2)
    {
        // int count = 0;
        if(MainTilemap.GetTile(tmp2.m_position + new Vector3Int(0 , 1 ,0)).name != "ground")
        {
            arrow_1.SetActive(false);
        }
        if(MainTilemap.GetTile(tmp2.m_position + new Vector3Int(0 , -1 ,0)).name != "ground")
        {
            arrow_2.SetActive(false);
        }
        if(MainTilemap.GetTile(tmp2.m_position + new Vector3Int(1 , 0 ,0)).name != "ground")
        {
            arrow_3.SetActive(false);
        }
        if(MainTilemap.GetTile(tmp2.m_position + new Vector3Int(-1 , 0 ,0)).name != "ground")
        {
            arrow_4.SetActive(false);
        }
        // if(count == 4) return -1;
        // else return 0;
    }

    public void build_LeftUp()
    {
        Vector3Int install_position = new Vector3Int(0,0,0);
        if(tmp_node.name == "Tree_cityhall(Clone)") 
        {
            install_position = tmp_node.m_position + new Vector3Int(0 , distance_of_cityhall ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[1], grid.GetCellCenterWorld(install_position)+new Vector3(0f,0.3f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = cityhall_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_wardoffice(Clone)")
        {
            install_position = tmp_node.m_position + new Vector3Int(0 , distance_of_house ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[2], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_house(Clone)")
        {
            install_position = tmp_node.m_position + new Vector3Int(0 , distance_of_house2 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[3], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else{ //tmp name == Tree_house
            Debug.Log("리프노드에는 자식노드를 추가할 수 없습니다");
            buttons_active();
            arrows.SetActive(false);
            return;
        }

        build_road(tmp_node.m_position , install_position);
        buttons_active();
        arrows.SetActive(false);
    }

    public void build_LeftDown()
    {
        Vector3Int install_position = new Vector3Int(0,0,0);
        if(tmp_node.name == "Tree_cityhall(Clone)") 
        {
            install_position = tmp_node.m_position + new Vector3Int(-distance_of_cityhall , 0 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[1], grid.GetCellCenterWorld(install_position)+new Vector3(0f,0.3f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = cityhall_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_wardoffice(Clone)")
        {
            install_position = tmp_node.m_position + new Vector3Int(-distance_of_house , 0 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[2], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_house(Clone)")
        { 
            install_position = tmp_node.m_position + new Vector3Int(-distance_of_house2 , 0 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[3], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else{ //tmp name == Tree_house
            Debug.Log("리프노드에는 자식노드를 추가할 수 없습니다");
            buttons_active();
            arrows.SetActive(false);
            return;
        }

        build_road(tmp_node.m_position , install_position);
        buttons_active();
        arrows.SetActive(false);
    }

    public void build_RightUp()
    {
        Vector3Int install_position = new Vector3Int(0,0,0);
        if(tmp_node.name == "Tree_cityhall(Clone)") 
        {
            install_position = tmp_node.m_position + new Vector3Int(distance_of_cityhall , 0 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[1], grid.GetCellCenterWorld(install_position)+new Vector3(0f,0.3f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = cityhall_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_wardoffice(Clone)")
        {
            install_position = tmp_node.m_position + new Vector3Int(distance_of_house , 0 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[2], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_house(Clone)")
        { 
            install_position = tmp_node.m_position + new Vector3Int(distance_of_house2 , 0 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[3], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else{ //tmp name == Tree_house
            Debug.Log("리프노드에는 자식노드를 추가할 수 없습니다");
            buttons_active();
            arrows.SetActive(false);
            return;
        }

        build_road(tmp_node.m_position , install_position);
        buttons_active();
        arrows.SetActive(false);
    }
    

    public void build_RightDown()
    {
        Vector3Int install_position = new Vector3Int(0,0,0);
        if(tmp_node.name == "Tree_cityhall(Clone)") 
        {
            install_position = tmp_node.m_position + new Vector3Int(0 , -distance_of_cityhall ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[1], grid.GetCellCenterWorld(install_position)+new Vector3(0f,0.3f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = cityhall_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_wardoffice(Clone)")
        {
            install_position = tmp_node.m_position + new Vector3Int(0 , -distance_of_house ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[2], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else if(tmp_node.name == "Tree_house(Clone)")
        {
            install_position = tmp_node.m_position + new Vector3Int(0 , -distance_of_house2 ,0);
            Tree_building installing_node = Instantiate(tree_buildings_sprites[3], grid.GetCellCenterWorld(install_position)+new Vector3(0f,-0.18f,0f), Quaternion.identity).GetComponent<Tree_building>();
            MainTilemap.SetTile(install_position, T_tiles[0]);
            installing_node.m_position = install_position;
            installing_node.p_position = tmp_node.m_position;
            tree.Add(installing_node);
            tmp_node.connected_buildings.Add(install_position);
        }
        else{ //tmp name == Tree_house
            Debug.Log("리프노드에는 자식노드를 추가할 수 없습니다");
            buttons_active();
            arrows.SetActive(false);
            return;
        }

        build_road(tmp_node.m_position , install_position);
        buttons_active();
        arrows.SetActive(false);
    }


    private void build_road(Vector3Int start, Vector3Int end)
    {
        Vector3Int direction_vec = end - start;
        int length = (int)direction_vec.magnitude;
        Vector3Int unit_vec = (direction_vec / length);
        Tile road_type = T_tiles[0];
        if((direction_vec.x == 0) && (direction_vec.y != 0))
        {
            road_type = T_tiles[2];
        }
        else{ //오류나면 여기 조건 추가 해야함
            road_type = T_tiles[1];
        }

        for(int i=1; i<length; i++)
        {
            MainTilemap.SetTile((start+(unit_vec*i)) , road_type);
        }
        //buttons_active();
    }
#endregion build

#region del
    public void del_button(Vector3Int cellPos)
    {
        Tree_building curr_node = null;
        Tree_building leaf_node = null;
        if(MainTilemap.GetTile(cellPos).name != "white")
        {
            Debug.Log("건물이 설치된 타일만 선택해 주세요");
            return;
        }
        int k =0;
        for(k = 0; k<tree.Count; k++)
        {
            if(tree[k].m_position == cellPos){
                curr_node = tree[k];
                break;
            }
        }
        if(!can_del(curr_node)){
            return;
        }
        for(int i=curr_node.connected_buildings.Count-1; i >= 0; i--)
        {
            Vector3Int removed_pos = curr_node.connected_buildings[i];
            MainTilemap.SetTile(removed_pos ,T_tiles[3]);
            for(int o=0; o<tree.Count; o++)
            {
                if(tree[o].m_position == removed_pos){
                    leaf_node = tree[o];
                    break;
                }
            }
            
            for(int t=leaf_node.connected_buildings.Count-1; t>=0; t--)
            {
                Vector3Int removed_pos2 = leaf_node.connected_buildings[t];
                MainTilemap.SetTile(removed_pos2 ,T_tiles[3]);

                //del_road with c_node
                del_road(removed_pos2 , leaf_node.m_position);
                //del building
                for(int j= tree.Count-1; j>=0; j--)
                {
                    if(tree[j].m_position == removed_pos2){
                        Destroy(tree[j].gameObject);
                        tree.RemoveAt(j);
                    }
                }

                leaf_node.connected_buildings.RemoveAt(t);
            }
        
        
            //del_road with c_node
            del_road(removed_pos , curr_node.m_position);
            //del building
            for(int j= tree.Count-1; j>=0; j--)
            {
                if(tree[j].m_position == removed_pos){
                    Destroy(tree[j].gameObject);
                    tree.RemoveAt(j);
                }
            }

            curr_node.connected_buildings.RemoveAt(i);
        }
        MainTilemap.SetTile(curr_node.m_position , T_tiles[3]);
        
        //del_road with p_node
        del_road(curr_node.m_position , curr_node.p_position);
        //del bulding
        for(int j= tree.Count-1; j>=0; j--)
        {
            if(tree[j].m_position == curr_node.p_position){
                for(int i=tree[j].connected_buildings.Count-1; i>=0; i--){
                    if(curr_node.m_position == tree[j].connected_buildings[i]){
                        tree[j].connected_buildings.RemoveAt(i);
                        break;
                    }
                }
                break;
            }
        }
        Destroy(curr_node.gameObject);
        tree.RemoveAt(k);
        
        //print_tree();
    }

    private bool can_del(Tree_building tmp)
    {
        if(tmp.name == "Tree_cityhall(Clone)")
        {
            Debug.Log("루트노드는 삭제할 수 없습니다.");
            return false;
        }
        return true;
    }

    private void del_road(Vector3Int start , Vector3Int end)
    {
        Vector3Int direction_vec = end - start;
        int length = (int)direction_vec.magnitude;
        Vector3Int unit_vec = (direction_vec / length);
        Tile road_type = T_tiles[3];

        for(int i=1; i<length; i++)
        {
            MainTilemap.SetTile((start+(unit_vec*i)) , road_type);
        }
    }
#endregion del


    public void print_tree()
    {
        for(int i=0; i<tree.Count; i++)
        {
            Debug.Log("node name : " + tree[i].name +", parent pos : "+tree[i].p_position + ", child count : "+ tree[i].connected_buildings.Count);
        }
    }
}
