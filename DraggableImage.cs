using UnityEngine;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 dragOffset;

    // Canvas의 RectTransform
    public RawImage RawImage;

    private bool isDragging = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 마우스가 클릭된 상태일 때
        if (isDragging)
        {
            Vector2 mousePosition = Input.mousePosition;

            // 마우스 위치를 드래그 offset만큼 보정
            Vector2 newPosition = mousePosition - dragOffset;

            // Y 좌표를 고정하고 X 좌표만 업데이트
            newPosition.y = rectTransform.position.y;
            
            if(newPosition.x >= 3500) {
                newPosition.x = 3500;
            } else if(newPosition.x <= -(3500- 1920)) {
                newPosition.x = -(3500 - 1920);
            }
            
            // RawImage 위치 업데이트
            RawImage.rectTransform.position = newPosition;
            
        }

        // 마우스 클릭 시작
        if (Input.GetMouseButtonDown(0))
        {
            // 클릭한 지점에서의 상대적인 오프셋 계산
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                RawImage.rectTransform, 
                Input.mousePosition, 
                null, 
                out dragOffset
            );

            isDragging = true;
        }

        // 마우스 클릭 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
