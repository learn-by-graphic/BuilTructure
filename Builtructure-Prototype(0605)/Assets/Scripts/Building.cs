using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    public string buildingName;
    public int populationNeed;
    public int energyNeed;
    public int woodNeed;
    public int stoneNeed;
    public int ironNeed;
    
    public int populationProduce;
    public int energyProduce;
    public int woodNeedProduce;
    public int stoneProduce;
    public int ironProduce;
    
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
