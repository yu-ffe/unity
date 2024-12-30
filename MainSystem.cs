using UnityEngine;

public class MainSystem : MonoBehaviour {
    public Canvas level1_CANVAS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    public void Level1_BUTTON() {
        this.gameObject.SetActive(false);
        level1_CANVAS.gameObject.SetActive(true);
    }
}
