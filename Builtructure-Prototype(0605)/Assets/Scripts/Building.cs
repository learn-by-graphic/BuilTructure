using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    public int populationNeed=5;
    public int energyNeed=10;
    public int woodNeed=30;
    public int stoneNeed=20;
    public int ironNeed=10;
    public bool Placed { get; private set; }
    public BoundsInt area;
    
    #region Build Methods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakearea(areaTemp))
        {
            if (GridBuildingSystem.current.CheckEnoughResource(this))
            {
                return true;
            }
            return false;
        }
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
}
