using UnityEngine;

public class SnowfallEffect : MonoBehaviour
{
    // 눈송이 프리팹
    public GameObject snowflakePrefab;

    // 눈송이 개수
    public int snowflakeCount = 50;

    // 스폰 영역
    public RectTransform spawnArea;

    // 눈송이 이동 속도 범위
    public float minSpeed = 50f;
    public float maxSpeed = 200f;

    private GameObject[] snowflakes;
    private float[] speeds;

    void Start()
    {
        // 눈송이 배열 초기화
        snowflakes = new GameObject[snowflakeCount];
        speeds = new float[snowflakeCount];

        // 눈송이 생성
        for (int i = 0; i < snowflakeCount; i++)
        {
            // 눈송이 인스턴스 생성
            snowflakes[i] = Instantiate(snowflakePrefab, spawnArea);

            // 눈송이 처음에는 비활성화
            snowflakes[i].SetActive(false);

            // 랜덤 위치 지정
            ResetSnowflake(i);
        }

        // 2초 뒤에 눈송이가 내리기 시작하도록 설정
        Invoke("StartSnowfall", 2f);
    }

    void Update()
    {
        for (int i = 0; i < snowflakeCount; i++)
        {
            if (snowflakes[i] != null && snowflakes[i].activeSelf)
            {
                // 눈송이 이동
                RectTransform rt = snowflakes[i].GetComponent<RectTransform>();
                rt.anchoredPosition -= new Vector2(0, speeds[i] * Time.deltaTime);

                // 화면 아래로 사라지면 위치 초기화
                if (rt.anchoredPosition.y < -spawnArea.rect.height / 2)
                {
                    ResetSnowflake(i);
                }
            }
        }
    }

    // 눈송이 위치와 속도 초기화
    private void ResetSnowflake(int index)
    {
        RectTransform rt = snowflakes[index].GetComponent<RectTransform>();

        // 랜덤 X 좌표
        float randomX = Random.Range(-spawnArea.rect.width / 2, spawnArea.rect.width / 2);

        // Y 좌표를 화면 위쪽으로 설정
        float startY = spawnArea.rect.height / 2;

        // 위치 설정
        rt.anchoredPosition = new Vector2(randomX, startY);

        // 랜덤 속도 설정
        speeds[index] = Random.Range(minSpeed, maxSpeed);
    }

    // 눈송이 내리기 시작
    private void StartSnowfall()
    {
        // 눈송이 활성화
        for (int i = 0; i < snowflakeCount; i++)
        {
            snowflakes[i].SetActive(true);
        }
    }
}
