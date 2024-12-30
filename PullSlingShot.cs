using UnityEngine;
using UnityEngine.UI;

public class PullSlingShot : MonoBehaviour {
    public GameObject piv1, piv2;
    public GameObject midPiv;
    public GameObject ball;
    public GameObject ball2; // 추가된 ball2

    private LineRenderer lineRenderer1;
    private LineRenderer lineRenderer2;
    private LineRenderer trajectoryLineRenderer; // 경로를 그릴 LineRenderer

    private Camera mainCamera;

    public AudioClip shootSound; // 사운드 파일을 참조할 변수
    public AudioClip birdSound; // 사운드 파일을 참조할 변수
    private AudioSource audioSource; // 오디오 재생을 위한 AudioSource
    private AudioSource audioSource2; // 오디오 재생을 위한 AudioSource

    public Canvas canavas;

    public RawImage rawImage;
    public RawImage rawImage2;

    public Button button;

    private bool isDragging;
    private bool hasCollided = false; // 충돌 상태 확인 변수
    private bool canShoot = true; // 공 발사 가능 여부

    public float maxDistance = 5f; // 원의 최대 거리
    private float currentDistance = 2f; // 현재 거리
    public float distanceAdjustmentSpeed = 1f; // 마우스 휠 조절 속도
    public int trajectoryPointCount = 10; // 경로에 표시할 점 개수

    private GameObject activeBall; // 현재 활성화된 공 (ball 또는 ball2)

    void Start() {
        // LineRenderer 초기화
        lineRenderer1 = piv1.AddComponent<LineRenderer>();
        lineRenderer2 = piv2.AddComponent<LineRenderer>();
        trajectoryLineRenderer = gameObject.AddComponent<LineRenderer>(); // 경로 LineRenderer 추가

        InitializeLineRenderer(lineRenderer1);
        InitializeLineRenderer(lineRenderer2);
        InitializeTrajectoryLineRenderer();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 시작 시 재생하지 않음
        audioSource.clip = shootSound; // 오디오 클립 설정

        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.playOnAwake = false; // 시작 시 재생하지 않음
        audioSource2.clip = birdSound; // 오디오 클립 설정

        // 메인 카메라 참조
        mainCamera = Camera.main;

        // 시작 시 midPiv에 연결
        DrawLinesToMidPiv();

        // ball을 활성화된 공으로 설정
        activeBall = ball;

        Invoke("EnableClick", 4f);
    }

    private void PlayShootSound() {
        if (audioSource != null && shootSound != null) {
            audioSource.Play(); // 사운드 재생
            audioSource2.Play(); // 사운드 재생
        }
    }

    void Update() {
        if (hasCollided || !canShoot) return; // 충돌하거나 발사 불가능 상태면 처리하지 않음

        HandleMouseWheel(); // 거리 조절

        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시작
        {
            if (IsMouseOverBall(activeBall)) // 활성화된 공 위에서 클릭한 경우
            {
                isDragging = true;
            }
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제
        {
            if (isDragging) // 공을 드래그한 후에만 발사
            {
                isDragging = false;
                DrawLinesToMidPiv(); // midPiv로 선 연결
                ShootBall(); // 공 발사
                rawImage2.transform.position = rawImage.transform.position;
                rawImage.enabled = false;
                foreach (Transform child in rawImage.transform) {
                    child.gameObject.SetActive(false);
                }
                Invoke("ActivateButton", 1f);
            }
        }

        if (isDragging) {
            DrawLinesToMouse(); // 마우스 위치로 선 연결

            // 예상 경로 업데이트
            Vector3 direction = midPiv.transform.position - activeBall.transform.position;
            direction = direction.normalized * currentDistance * 10; // 초기 속도와 거리 반영
            DrawTrajectory(activeBall.transform.position, direction);
        }
    }

    private void ActivateButton() {
        if (!check)
            button.gameObject.SetActive(true);
    }

    private void HandleMouseWheel() {
        // 마우스 휠 입력으로 거리 조절
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance + scroll * distanceAdjustmentSpeed, 0.5f, maxDistance);
    }

    private void InitializeLineRenderer(LineRenderer lineRenderer) {
        lineRenderer.positionCount = 2; // 시작점과 끝점
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 단순한 머티리얼
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    private void InitializeTrajectoryLineRenderer() {
        trajectoryLineRenderer.positionCount = trajectoryPointCount;
        trajectoryLineRenderer.startWidth = 0.02f;
        trajectoryLineRenderer.endWidth = 0.02f;
        trajectoryLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trajectoryLineRenderer.startColor = Color.red;
        trajectoryLineRenderer.endColor = Color.red;
    }

    private void DrawLinesToMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Vector3 clampedPosition = ClampToCircle(worldMousePosition);

        if (check) {
            activeBall.transform.position = midPiv.transform.position - (midPiv.transform.position - clampedPosition);
        }
        else {
            activeBall.transform.position = midPiv.transform.position - (clampedPosition - midPiv.transform.position);

        }
        lineRenderer1.SetPosition(0, piv1.transform.position);
        lineRenderer1.SetPosition(1, activeBall.transform.position);

        lineRenderer2.SetPosition(0, piv2.transform.position);
        lineRenderer2.SetPosition(1, activeBall.transform.position);
    }

    private void DrawLinesToMidPiv() {
        lineRenderer1.SetPosition(0, piv1.transform.position);
        lineRenderer1.SetPosition(1, midPiv.transform.position);

        lineRenderer2.SetPosition(0, piv2.transform.position);
        lineRenderer2.SetPosition(1, midPiv.transform.position);
    }

    private void DrawTrajectory(Vector3 startPosition, Vector3 initialVelocity) {
        trajectoryLineRenderer.positionCount = trajectoryPointCount;
        float timeStep = 0.1f;
        Vector3 gravity = Physics.gravity;

        Vector3 currentPosition = startPosition;
        Vector3 velocity = initialVelocity;

        int actualPointCount = trajectoryPointCount;

        for (int i = 0; i < trajectoryPointCount; i++) {
            trajectoryLineRenderer.SetPosition(i, currentPosition);

            velocity += gravity * timeStep;
            currentPosition += velocity * timeStep;

            RaycastHit hit;
            if (Physics.Raycast(currentPosition, velocity.normalized, out hit, velocity.magnitude * timeStep)) {
                trajectoryLineRenderer.SetPosition(i, hit.point);
                actualPointCount = i + 1;
                break;
            }
        }

        trajectoryLineRenderer.positionCount = actualPointCount;
    }

    private void ShootBall() {
        Vector3 direction = midPiv.transform.position - activeBall.transform.position;
        direction = direction.normalized * currentDistance * 10;

        Rigidbody ballRigidbody = activeBall.GetComponent<Rigidbody>();
        ballRigidbody.isKinematic = false;
        ballRigidbody.linearVelocity = direction;

        trajectoryLineRenderer.positionCount = 0;

        PlayShootSound();

        canShoot = false;
        if (check) {
            Invoke("ActivateCanvas", 5f);
        }
    }
    private void ActivateCanvas() {
        canavas.gameObject.SetActive(true);

    }

    private Vector3 ClampToCircle(Vector3 targetPosition) {
        Vector3 direction = targetPosition - midPiv.transform.position;
        float distance = direction.magnitude;

        if (distance > currentDistance) {
            direction = direction.normalized * currentDistance;
        }

        return midPiv.transform.position - direction;
    }

    private bool IsMouseOverBall(GameObject ballObject) {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.transform.gameObject == ballObject) {
                return true;
            }
        }

        return false;
    }

    private bool check = false;

    public void SwitchToBall2() {
        activeBall = ball2;

        activeBall.transform.position = midPiv.transform.position;
        check = true;

        Rigidbody ballRigidbody = activeBall.GetComponent<Rigidbody>();
        if (ballRigidbody != null) {
            ballRigidbody.isKinematic = true;
            ballRigidbody.linearVelocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }

        DrawLinesToMidPiv();

        canShoot = true;
        isDragging = false;
    }
}
