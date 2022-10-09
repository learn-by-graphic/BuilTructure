using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelcSort_Manager : MonoBehaviour
{
    private int click_count;
    private string box_string;
    private GameObject[] boxes;
    // Start is called before the first frame update
    void Start()
    {
        click_count = 0;
        box_string = "";
        boxes = new GameObject[8];
        for(int i=1; i<9; i++)
        {
            box_string = "box" + "" + i;
            boxes[i-1] = GameObject.Find(box_string);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play_button()
    {
        click_count++;
        switch(click_count)
        {
            case 1:
            boxes[0].GetComponent<Animation>().Play();
            break;

            case 2:
            boxes[1].GetComponent<Animation>().Play();
            break;

            case 3:
            boxes[2].GetComponent<Animation>().Play();
            break;

            case 4:
            boxes[3].GetComponent<Animation>().Play();
            break;

            case 5:
            boxes[4].GetComponent<Animation>().Play();
            break;

            case 6:
            boxes[5].GetComponent<Animation>().Play();
            break;

            case 7:
            boxes[6].GetComponent<Animation>().Play();
            break;

            case 8:
            boxes[7].GetComponent<Animation>().Play();
            Invoke("complete_sort_invoke",1.3f);   //   sort_complete function will execute when last animation end  
            break;
            
        }
    }

    private void complete_sort_invoke()
    {
        Debug.Log("complete selc sort");
        GameObject.Find("PlayButton").SetActive(false);
    }
}
