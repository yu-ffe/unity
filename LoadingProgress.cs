using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgress : MonoBehaviour {

    public Slider progressBar; // ProgressBar를 표시할 Slider
    public float fillSpeed = 0.5f; // ProgressBar 채우는 속도
    private float targetValue = 1f; // ProgressBar가 다 채워질 목표 값 (1f)

    public Canvas MainCanvas_UI;
    
    void Start() {
        progressBar.value = 0f; // ProgressBar는 0부터 시작

    }

    void Update() {
        if (progressBar.value < targetValue)
        {
            if (progressBar.value >= 0.5f)
            {
                StartCoroutine(ResumeFillingAfterDelay(1f));
            }
            else
            {
                progressBar.value += fillSpeed * Time.deltaTime;
            }
        }
    }
    private IEnumerator ResumeFillingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (progressBar.value < targetValue)
        {
            progressBar.value += fillSpeed * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        OnProgressComplete();
    }
    
    private void OnProgressComplete() {
        this.gameObject.SetActive(false);
        this.MainCanvas_UI.gameObject.SetActive(true);
    }
}
