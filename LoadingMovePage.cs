using System.Collections;
using UnityEngine;

public class LoadingMovePage : MonoBehaviour {
    public Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveCamera());
        
    }
    private IEnumerator MoveCamera()
    
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
