using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Image StageImage;
    public Sprite StackImage;
    public Sprite QueueImage;
    public Sprite GraphImage;
    public Sprite TreeImage;
    public TextMeshProUGUI MainText;
    public TextMeshProUGUI SubText;
    
    private int StageIndicator = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // stage button down methods
    public void StackStageDown()
    {
        StageIndicator = 1;
        StageImage.sprite = StackImage;
        MainText.text = "스택";
        SubText.text = "스택을 활용해 어둠 속에서 목적지를 찾으세요!";
    }
    public void QueueStageDown()
    {
        StageIndicator = 2;
        StageImage.sprite = QueueImage;
        MainText.text = "큐";
        SubText.text = "목적지에 큐 방식으로 안전하게 도착해봐요";
    }
    public void GraphStageDown()
    {
        StageIndicator = 3;
        StageImage.sprite = GraphImage;
        MainText.text = "그래프";
        SubText.text = "도시에 건물과 도로를 건설하며 그래프 구조를 익혀요";
    }
    public void TreeStageDown()
    {
        StageIndicator = 4;
        StageImage.sprite = TreeImage;
        MainText.text = "트리";
        SubText.text = "스택을 활용해 어둠 속에서 목적지를 찾으세요!";
    }
    
    // start button down methods
    public void StartButtonDown()
    {
        switch (StageIndicator)
        {
            case 1 :
                SceneManager.LoadScene("GameScene");
                break;
            case 2 :
                SceneManager.LoadScene("QueueStage");
                break;
            case 3 :
                SceneManager.LoadScene("GraphScene");
                break;
            case 4 :
                SceneManager.LoadScene("TreeScene");
                break;
        }
    }
}
