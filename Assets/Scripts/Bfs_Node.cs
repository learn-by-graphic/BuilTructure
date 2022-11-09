using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bfs_Node
{

    public Vector3Int node_position;
    public string node_color;
    public Bfs_Node parent;
    public List<Bfs_Node> childs;


    public Bfs_Node(Vector3Int pos, string color, Bfs_Node p)
    {
        this.node_position = pos;
        this.node_color = color;
        this.parent = p;
    }
    ~Bfs_Node()
    {
    }
    // Start is called before the first frame update 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
