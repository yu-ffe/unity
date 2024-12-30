using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour {

    public float HP = 100; // 초기 체력, 소재가 다를 때의 문제점은 체력 값 바꾸는 것으로 통일
    public AudioClip destroySound; // 파괴 시 재생할 소리
    private AudioSource audioSource; // 오디오 재생을 위한 AudioSource
    public ParticleSystem particlePrefab; // 파괴 효과를 위한 ParticleSystem 프리팹

    public TMP_Text ScoreBoard; // 점수를 표시할 UI 텍스트
    public int Point = 1000; // 객체의 점수 기본값

    private Rigidbody rb; // 자신의 Rigidbody

    void Start() {
        // 자신의 Rigidbody 가져오기
        rb = GetComponent<Rigidbody>();
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 시작 시 재생하지 않음
        audioSource.clip = destroySound; // 오디오 클립 설정
    }

    void Update() {
        // HP가 0 이하라면 객체를 파괴
        if (HP <= 0) {
            Debug.Log("Entity has been destroyed.");

            // 점수를 ScoreBoard에 추가
            if (ScoreBoard != null) {
                int currentScore = int.Parse(ScoreBoard.text);
                ScoreBoard.text = (currentScore + Point).ToString();
            }

            // 3D 화면에 점수 표시
            ShowScoreIn3D();

            // 파티클 효과 생성 및 재생
            if (particlePrefab != null) {
                ParticleSystem particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constantMax); // 파티클 종료 후 삭제
            }

            // 파괴 사운드 재생
            if (audioSource && audioSource.clip) {
                // 별도의 GameObject 생성 및 AudioSource 복사
                GameObject audioObject = new GameObject("TempAudio");
                AudioSource tempAudioSource = audioObject.AddComponent<AudioSource>();
                tempAudioSource.clip = audioSource.clip;
                tempAudioSource.volume = audioSource.volume;

                // 피치 값에 랜덤 요소 추가 (-0.2 ~ +0.2)
                tempAudioSource.pitch = audioSource.pitch + Random.Range(-0.2f, 0.2f);
                tempAudioSource.spatialBlend = audioSource.spatialBlend;
                tempAudioSource.Play();

                // 재생 시간에 랜덤 요소 추가 (최소 clip 길이, 최대 clip 길이 + 0.5초)
                float randomDuration = audioSource.clip.length + Random.Range(0f, 0.5f);
                Destroy(audioObject, randomDuration);
            }

            Destroy(gameObject); // 객체 파괴
        }
    }

    private void ShowScoreIn3D() {
        GameObject scoreTextObject = new GameObject("ScoreText");
        TextMesh textMesh = scoreTextObject.AddComponent<TextMesh>();
        textMesh.text = $"+{Point}";
        textMesh.fontSize = 30;
        textMesh.color = Color.yellow;

        scoreTextObject.transform.position = transform.position;
        scoreTextObject.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        Destroy(scoreTextObject, 2f); // 2초 후 점수 표시 객체 삭제
    }

    private void OnCollisionEnter(Collision collision) {
        try {
            // 충돌한 객체의 Rigidbody 가져오기
            Rigidbody otherRigidbody = collision.rigidbody;

            // 상대 속도 계산
            Vector3 relativeVelocity = collision.relativeVelocity;

            // 자신의 운동량 변화량 계산
            float selfImpact = rb.mass * relativeVelocity.magnitude;

            float totalImpact = selfImpact;

            // 충돌한 객체가 Rigidbody를 가지고 있는 경우 추가적으로 고려
            if (otherRigidbody != null) {
                // 충돌 객체의 운동량 변화량 계산
                float otherImpact = otherRigidbody.mass * relativeVelocity.magnitude;
                totalImpact += otherImpact;
            }
            else {
                // 충돌 객체가 Rigidbody가 없는 경우 벽이나 고정된 물체로 간주
                totalImpact += rb.mass * relativeVelocity.magnitude;
            }

            // 데미지 계산
            float damage = totalImpact;

            // 데미지가 너무 작으면 무시
            if (damage < 3.0f) return;

            // HP 감소
            HP -= damage;

            Debug.Log($"Damage taken: {damage}, Remaining HP: {HP}");
        }
        catch {
            Debug.Log("충돌 객체를 찾을 수 없습니다.");
        }
    }
}
