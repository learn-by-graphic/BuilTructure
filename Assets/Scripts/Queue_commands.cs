using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue_commands : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject Parent;
    private Vector2 create_point;
    private RectTransform rect_obj , Parent_rect;
    
    private int count = 0;
    private int size = 12; // width 에 따라 변경?
    

    bool sizeCheck()
    {
        if(count<12){
            return true;
        }
        else{
            Debug.Log("생성불가");
            return false;
        }
    }


    public void create_upblocks()
    {
        if(sizeCheck()){
            GameObject obj = Instantiate(prefabs[0], Parent.transform);
            rect_obj = obj.GetComponent<RectTransform>();
            Parent_rect = Parent.GetComponent<RectTransform>();
            float P_width = Parent_rect.rect.width;
            float C_width = rect_obj.rect.width;

            rect_obj.anchoredPosition = new Vector2(P_width/2 - C_width/2 - count*81f, 0);
            count++;
        }
        else return;
    }
    public void create_rightblocks()
    {
        if(sizeCheck()){
            GameObject obj = Instantiate(prefabs[1], Parent.transform);
            rect_obj = obj.GetComponent<RectTransform>();
            Parent_rect = Parent.GetComponent<RectTransform>();
            float P_width = Parent_rect.rect.width;
            float C_width = rect_obj.rect.width;

            rect_obj.anchoredPosition = new Vector2(P_width/2 - C_width/2 - count*81f, 0);
            count++;
        }
        else return;
    }
    public void create_leftblocks()
    {
        if(sizeCheck()){
            GameObject obj = Instantiate(prefabs[2], Parent.transform);
            rect_obj = obj.GetComponent<RectTransform>();
            Parent_rect = Parent.GetComponent<RectTransform>();
            float P_width = Parent_rect.rect.width;
            float C_width = rect_obj.rect.width;

            rect_obj.anchoredPosition = new Vector2(P_width/2 - C_width/2 - count*81f, 0);
            count++;
        }
        else return;
    }
    public void create_downblocks()
    {
        if(sizeCheck()){
            GameObject obj = Instantiate(prefabs[3], Parent.transform);
            rect_obj = obj.GetComponent<RectTransform>();
            Parent_rect = Parent.GetComponent<RectTransform>();
            float P_width = Parent_rect.rect.width;
            float C_width = rect_obj.rect.width;

            rect_obj.anchoredPosition = new Vector2(P_width/2 - C_width/2 - count*81f, 0);
            count++;
        }
        else return;
    }
}
