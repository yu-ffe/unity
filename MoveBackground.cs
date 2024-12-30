using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    // 이동할 목표 좌표
    private Vector3 targetPosition = Vector3.zero;

    public float yPos = 0.0f;

    // 이동 속도 조정 변수
    public float moveDuration = 2.0f; // 움직이는 데 걸리는 시간(초)
    public float delay = 1.0f; // 이동 시작까지의 지연 시간
    private float elapsedTime = 0f;

    // 초기 위치 저장
    private Vector3 startPosition;

    // Start는 첫 실행 시 호출
    void Start() {
        targetPosition.y = yPos;
        // 초기 위치를 현재 위치로 설정
        startPosition = transform.localPosition;
    }

    // Update는 매 프레임 호출
    void Update()
    {
        // 딜레이 동안 대기
        if (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            return;
        }

        // 딜레이를 초과한 시간만큼 이동 경과 시간 계산
        float moveElapsedTime = elapsedTime - delay;

        if (moveElapsedTime < moveDuration)
        {
            // 이동 경과 시간 누적
            elapsedTime += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float progress = moveElapsedTime / moveDuration;

            // Easing 적용
            float easedProgress = Mathf.SmoothStep(0, 1, progress);

            // 위치 업데이트
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, easedProgress);
        }
    }
}
