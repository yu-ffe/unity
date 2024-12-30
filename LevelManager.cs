using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public GameObject level;
    public GameObject slingshot;
    public GameObject blackBall;
    public GameObject midPiv;
    public RawImage rawImage;
    public Button button;

    public void nextLevel() {
        StartCoroutine(MoveSlingshotCoroutine());
        StartCoroutine(MoveRawImageCoroutine());
        Destroy(button.gameObject);
        
    }

    private IEnumerator MoveSlingshotCoroutine() {
        Vector3 startPosition = slingshot.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0, 180);
        float duration = 0.3f;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            slingshot.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        blackBall.transform.position = midPiv.transform.position;

        slingshot.transform.position = endPosition;
        rawImage.transform.position += new Vector3(100, 0, 0);
        
        yield return new WaitForSeconds(1.0f);
        level.SetActive(true);
        blackBall.SetActive(true);

    }
    
    private IEnumerator MoveRawImageCoroutine() {
        Vector3 startPosition = rawImage.transform.position;
        Vector3 endPosition = startPosition + new Vector3(100, 0, 0);
        float duration = 0.3f;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            rawImage.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rawImage.transform.position = endPosition;
    }
}
