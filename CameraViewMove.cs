using System;
using UnityEngine;

public class CameraViewMove : MonoBehaviour {
    [Header("Camera Settings")]
    public Camera camera; // 카메라 참조
    public GameObject midPiv; // 중심점(Sphere의 중심)

    [Header("Movement Settings")]
    public float rotationSpeed = 1f; // 회전 속도
    public float zoomSpeed = 2f; // 줌 속도

    [Header("Zoom Limits")]
    public float maxDistance = 13f; // 최대 거리
    public float currentDistance = 10f; // 카메라와 중심점 간의 초기 거리
    public float minDistance = 7f; // 최소 거리

    private Vector2 virtualPosition; // 가상 X, Y 좌표

    void Start() {
        InitializeCamera();
    }

    void Update() {
        HandleMouseInput();
        UpdateCameraPosition();
        HandleZoom();
    }

    // 카메라 초기화
    private void InitializeCamera() {
        virtualPosition = new Vector2(0f, 0f); // 가상의 X, Y 좌표 초기화
        virtualPosition.y = -15.0f; // 초기 Y값 설정
        UpdateCameraPosition();
    }

    // 마우스 입력 처리
    private void HandleMouseInput() {
        if (Input.GetMouseButton(0)) {  // 왼쪽 마우스 버튼 클릭
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            virtualPosition.x -= mouseX * rotationSpeed * Time.deltaTime;
            virtualPosition.y += mouseY * rotationSpeed * Time.deltaTime;

            // Y축 회전 제한 (카메라가 위아래로 너무 돌지 않도록 제한)
            virtualPosition.y = Mathf.Clamp(virtualPosition.y, -45f, 45f);
        }
    }

    // 카메라 위치 업데이트
    private void UpdateCameraPosition() {
        // 가상의 X, Y 좌표를 기반으로 회전 계산
        Quaternion rotation = Quaternion.Euler(virtualPosition.y, virtualPosition.x, 0);

        // midPiv에서 거리만큼 떨어진 위치 계산
        Vector3 offset = rotation * Vector3.back * currentDistance;
        camera.transform.position = midPiv.transform.position + offset;

        // 카메라가 항상 중심점을 바라보게 유지
        camera.transform.LookAt(midPiv.transform.position);
    }

    // 줌 처리
    private void HandleZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }
}
