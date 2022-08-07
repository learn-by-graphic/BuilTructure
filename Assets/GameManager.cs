using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public Tilemap DarkMap;

    public Tilemap Ground;

    public Tile DarkTile;
    // Start is called before the first frame update
    void Start()
    {
        DarkMap.origin = Ground.origin;
        DarkMap.size = Ground.size;

        foreach (Vector3Int p in DarkMap.cellBounds.allPositionsWithin)
        {
            DarkMap.SetTile(p, DarkTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
