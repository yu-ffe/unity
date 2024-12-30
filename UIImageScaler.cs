using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIImageScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform targetImage; // 이미지 RectTransform
    [SerializeField] private Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f); // 마우스를 올렸을 때의 크기
    [SerializeField] private Vector3 clickScale = new Vector3(0.9f, 0.9f, 1f); // 누르고 있을 때의 크기
    private Vector3 originalScale; // 원래 크기

    private bool isPointerDown = false; // 마우스가 눌린 상태인지 확인

    private void Awake()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<RectTransform>();
        }
        originalScale = targetImage.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPointerDown)
        {
            // 마우스를 올렸을 때 크기 변경
            targetImage.localScale = hoverScale;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPointerDown)
        {
            // 마우스가 벗어났을 때 원래 크기로 복귀
            targetImage.localScale = originalScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 마우스를 클릭했을 때 크기 변경
        isPointerDown = true;
        targetImage.localScale = clickScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 마우스를 뗐을 때 원래 크기 또는 Hover 크기로 복귀
        isPointerDown = false;
        targetImage.localScale = hoverScale;
    }
}
