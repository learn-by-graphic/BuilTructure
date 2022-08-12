using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue_commands : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject Parent;
    public GameObject car;
    private Vector2 create_point;
    private RectTransform rect_obj , Parent_rect;

    private static int count = 0;
    private int size = 12; // width 에 따라 변경?
    private int i = 0;
    private int[] queue; // 0 :up , 1: right , 2: left , 3: down

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
    void Awake()
    {
        queue = new int[size];
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
            queue[count] = 0;
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
            queue[count] = 1;
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
            queue[count] = 2;
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
            queue[count] = 3;
            count++;
        }
        else return;
    }

    public void move_car()
    {
        
        if(i <= count-1)
        {
            switch (queue[i])
            {
                case 0:
                    car.GetComponent<Car_Queue>().carMoveUp();
                    break;
                case 1:
                    car.GetComponent<Car_Queue>().carMoveRight();
                    break;
                case 2:
                    car.GetComponent<Car_Queue>().carMoveLeft();
                    break;
                case 3:
                    car.GetComponent<Car_Queue>().carMoveDown();
                    break;
            }
            Destroy(transform.GetChild(0).gameObject);
            i++;
            if (i == count)
            {
                CancelInvoke("move_car");
                Debug.Log("이동 종료");
                return;
            }
            
            Invoke("move_car", 1.34f);
        }
        else
        {
            
        }
    }
}
