using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    public List<List<Vector3Int>> connected_roads = new List<List<Vector3Int>>();

    #region Build Methods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakearea(areaTemp))
        {
            return true;
        }
        Debug.Log("배치할 수 없습니다");
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
    }

    #endregion

    // public int getconnectionCount()
    // {
    //     return this.connected_roads.Count;
    // }
    public void destroy_building()
    {
        Destroy(this.gameObject);

    }

    public void add_connected_roads(List<Vector3Int> roads)
    {
        connected_roads.Add(roads);
    }

    public void destroy_connected_roads(Vector3Int connected_pos)
    {
        int i = 0;
        foreach(List<Vector3Int> road in connected_roads)
        {
            if(road[0] == connected_pos || road[road.Count-1] == connected_pos)
            {
                connected_roads.RemoveAt(i);
                break;
            }
            i++;
        }
    }

    public void print_my_road()
    {
        for(int i=0; i<connected_roads.Count; i++)
        {
            for(int j=0; j<connected_roads[i].Count; j++)
            {
                Debug.Log(connected_roads[i][j]+ " ");
            }
        }
    }
}
