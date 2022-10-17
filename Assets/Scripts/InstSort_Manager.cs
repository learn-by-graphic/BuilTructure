using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstSort_Manager : MonoBehaviour
{
    
    public float move_speed = 2f;
    private GameObject[] boxes;
    private GameObject play_button;
    private Vector3[] target_positions;
    private string box_string = "";
    private int phase_count;
    // Start is called before the first frame update
    void Start()
    {
        phase_count = 0;
        play_button = GameObject.Find("PlayButton");
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
        if (phase_count == 1)
        {
            vector_move(boxes[6], target_positions[1]);
        }
        else if (phase_count == 2)
        {
            vector_move(boxes[6], target_positions[2]);
            vector_move(boxes[4], target_positions[1]);
        }
        else if (phase_count == 3)
        {
            vector_move(boxes[6], target_positions[3]);
            vector_move(boxes[4], target_positions[2]);
            vector_move(boxes[2], target_positions[1]);
        }
        else if (phase_count == 4)
        {
            vector_move(boxes[6], target_positions[4]);
            vector_move(boxes[4], target_positions[3]);
            vector_move(boxes[2], target_positions[2]);
        }
        else if (phase_count == 5)
        {
            vector_move(boxes[6], target_positions[5]);
            vector_move(boxes[4], target_positions[4]);
        }
        else if (phase_count == 6)
        {
            vector_move(boxes[6], target_positions[6]);
        }
        else
        {

        }
    }

    public void animation_move()
    {
        phase_count++;
        switch(phase_count)
        {
            case 1:
                boxes[4].GetComponent<Animation>().Play();
                play_button.SetActive(false);
                break;
            case 2:
                boxes[2].GetComponent<Animation>().Play();
                play_button.SetActive(false);
                break;
            case 3:
                boxes[0].GetComponent<Animation>().Play();
                play_button.SetActive(false);
                break;
            case 4:
                boxes[1].GetComponent<Animation>().Play();
                play_button.SetActive(false);
                break;
            case 5:
                boxes[3].GetComponent<Animation>().Play();
                play_button.SetActive(false);
                break;
            case 6:
                boxes[5].GetComponent<Animation>().Play();
                play_button.SetActive(false);
                break;
            case 7:
                boxes[7].GetComponent<Animation>().Play();
                Invoke("complete_sort_invoke", 1.8f);
                break;
        }
        
    }

    public void vector_move(GameObject box, Vector3 tar_pos)
    {
        box.transform.position = Vector3.MoveTowards(box.transform.position , tar_pos, move_speed * Time.deltaTime);
        if(box.transform.position == tar_pos)
            play_button.SetActive(true);
    }

    private void complete_sort_invoke()
    {
        Debug.Log("complete selc sort");
        play_button.SetActive(false);
    }
}
