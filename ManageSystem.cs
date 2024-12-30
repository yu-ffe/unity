using UnityEngine;

public class ManageSystem : MonoBehaviour {

    public Canvas mainUI_CANVAS;
    public Canvas levelSelectUI_CANVAS;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameStart_BUTTON() {
        this.mainUI_CANVAS.gameObject.SetActive(false);    
    }

    public void LevelWorld_BUTTON(int level) {
        this.mainUI_CANVAS.gameObject.SetActive(false);
        this.levelSelectUI_CANVAS.gameObject.SetActive(true);
        // this.levelSelectUI_CANVAS.GetComponent<RectTransform>().anchoredPosition += new Vector2(level * 100, 0);
    }
}
