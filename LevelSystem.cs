using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour {

    public GameObject levelPlay;
    public TMP_Text levelPlayText;
    public Button levelPlayButton;
    public GameObject level1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void LoadLevelPlay(int level) {
        levelPlayText.text = levelPlayText.text.Replace("${n}", level.ToString());
        
        levelPlayButton.onClick.AddListener(() => LoadLevel(level));
        this.levelPlay.SetActive(true);
    }
    
    public void LoadLevel(int level) {
        if (level == 1) {
            gameObject.SetActive(false);
            level1.SetActive(true);
        }

    }
}
