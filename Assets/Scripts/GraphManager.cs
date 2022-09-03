using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class GraphManager : MonoBehaviour
{
    public static List<Building> nodes = new List<Building>();
    private Building temp;
    public Tile ground_tile;
    public Tilemap MainTilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBuilding(Building building)
    {
        nodes.Add(building);
    }

    public List<Building> GetAllBuildings()
    {
        return nodes;
    }

    public void RemoveBuilding(Vector3Int cellpos)
    {

        for(int i=0; i<nodes.Count; i++)
        {
            if(cellpos == nodes[i].area.position)
            {
                destroying_my_road(i);
                nodes.RemoveAt(i);

                break;
            }
        }

    }
    public void destroying_my_road(int i)
    {
        for(int k=0; k<nodes[i].connected_roads.Count; k++)
        {
            for(int j=1; j<nodes[i].connected_roads[k].Count-1; j++)
            {
                if((MainTilemap.GetTile(nodes[i].connected_roads[k][j])).name != "ground")
                {
                    MainTilemap.SetTile(nodes[i].connected_roads[k][j], ground_tile);
                }
            }
        }

        nodes[i].connected_roads.Clear();


    }


    public Building SearchBuilding(Vector3Int cellpos)
    {
        for(int i=0; i<nodes.Count; i++)
        {
            if(cellpos == nodes[i].area.position)
            {
                return nodes[i];
            }
        }
        return null;
        
    }
    public void Addroads(Vector3Int start, Vector3Int end , List<Vector3Int> roads)
    {
        // SearchBuilding(start).add_connected_roads(roads);
        // SearchBuilding(end).add_connected_roads(roads);
        List<Vector3Int> deep_copy = roads.ToList();
        for(int i=0; i<nodes.Count; i++)
        {
            if(start == nodes[i].area.position)
            {

                nodes[i].connected_roads.Add(deep_copy);
            }
            else if(end == nodes[i].area.position)
            {
                nodes[i].connected_roads.Add(deep_copy);
            }
        }
        // SearchBuilding(start).print_my_road();
        // SearchBuilding(end).print_my_road();
        
    }

    public void show_every_build()
    {
        Debug.Log("node count" + nodes.Count);
        for(int i=0; i<nodes.Count; i++)
        {
            Debug.Log("connected road count" +nodes[i].connected_roads.Count);
            for(int k=0; k<nodes[i].connected_roads.Count; k++)
            {
                Debug.Log("road length per connected road count"+ nodes[i].connected_roads[k].Count);
                for(int j=0; j<nodes[i].connected_roads[k].Count; j++)
                {
                    //Debug.Log(nodes[i].connected_roads[k][j]+ " ");
                }
            }
        }
    }
}
