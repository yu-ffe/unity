using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextOnOff : MonoBehaviour {
    public Image image;
    public TMP_Text text;
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnButtonClick() {
        Debug.Log("Button Clicked");
        image.enabled = !image.enabled;
        text.enabled = !text.enabled;
    }
}
