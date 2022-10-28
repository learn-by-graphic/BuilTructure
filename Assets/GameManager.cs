using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public TalkManager talkManager;

    public Tilemap DarkMap;

    public Tilemap Ground;

    public Tile DarkTile;
    
    //대화창 
    public GameObject talkPanel;
    public TextMeshProUGUI UITalkText;
    public GameObject scanObject;
    public bool isAction; //대화창 활성화 상태 
    public int talkIndex;
    // Start is called before the first frame update
    void Awake()
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
    
    public void Action(GameObject scanObj)
    {

        scanObject = scanObj;
        //UITalkText.text = "이것은 "+scanObject.name+"이다.";
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id,objData.isNPC);
        

        talkPanel.SetActive(isAction); //대화창 활성화 상태에 따라 대화창 활성화 변경
    }


    void Talk(int id, bool isNPC){

        string talkData = talkManager.GetTalk(id, talkIndex);

        if(talkData == null) //반환된 것이 null이면 더이상 남은 대사가 없으므로 action상태변수를 false로 설정 
        {
            isAction = false;
            talkIndex=0; //talk인덱스는 다음에 또 사용되므로 초기화해야함 
            return; //void에서의 return 함수 강제종료 (밑의 코드는 실행되지 않음)
        }

        if(isNPC){
            UITalkText.text = talkData.Split(':')[0]; //구분자로 문장을 나눠줌  0: 대사 1:portraitIndex

        }else{
            UITalkText.text = talkData;
        }

        //다음 문장을 가져오기 위해 talkData의 인덱스를 늘림
        isAction=true; //대사가 남아있으므로 계속 진행되어야함 
        talkIndex++;
    }
}
