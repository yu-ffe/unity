using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Add this for UI.Text

public class RepeatingText : MonoBehaviour {
    public List<string> message;
    public TextMeshProUGUI  uiText; // Declare a reference to the Text component

    public Canvas LoadedCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        StartCoroutine(ChangeMessage());
    }

    // Update is called once per frame
    void Update() {

    }
    
    int count = 0; // 이건 그냥 여러번 보여주고 끝낼때 쓸꺼임
    private IEnumerator ChangeMessage() {
        int index = 0;
        while (true) {
            if (!gameObject.activeInHierarchy) {
                yield break; // Exit the coroutine if the object is no longer active
            }
            count++;
            if (count >= 10) {
                if(LoadedCanvas != null) {
                    LoadedCanvas.gameObject.SetActive(true);
                }
                this.gameObject.SetActive(false);
                yield break;
            }

            // Update the text of the UI Text component
            uiText.text = message[index];
            index = (index + 1) % message.Count;

            // Wait for 1 second before changing the message
            yield return new WaitForSeconds(0.4f);
        }
    }
}
