using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    public Sprite[] portraitArr;

    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        //대사 생성 
        talkData.Add(1000,
            new string[]
            {
                "어두운 길에서 집을 찾아가야해용", "어떻게 \n처리하면 좋을까요?:0"
            });
    }

    public string GetTalk(int id, int talkIndex) //Object의 id , string배열의 index
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex]; //해당 아이디의 해당
    }
}
