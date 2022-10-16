using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstSort_Manager : MonoBehaviour
{
    public bool move_on = false;
    public float move_speed = 2f;
    public GameObject[] boxes;
    private Vector3[] target_positions;
    private string box_string = "";
    private int phase_count;
    // Start is called before the first frame update
    void Start()
    {
        float x_pos = -6f;
        boxes = new GameObject[8];
        target_positions = new Vector3[8];
        for (int i = 1; i < 9; i++)
        {
            box_string = "box" + "" + i;
            target_positions[i-1] = new Vector3(x_pos + (1.5f * (i-1)), -2, 0);
            boxes[i - 1] = GameObject.Find(box_string);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move_on)
        {
            vector_move(boxes[6], target_positions[1]);
        }
    }

    public void animation_move()
    {
        move_on = true;
        boxes[4].GetComponent<Animation>().Play();
    }

    public void vector_move(GameObject box, Vector3 tar_pos)
    {
        box.transform.position = Vector3.MoveTowards(box.transform.position , tar_pos, move_speed * Time.deltaTime);
        if (box.transform.position == tar_pos)
            move_on = false;
    }
}
