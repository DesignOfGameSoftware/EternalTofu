using UnityEngine;
using UnityEngine.UI;  // UI 관련 네임스페이스 추가

public class Character : MonoBehaviour
{
    private Transform mTransform;    // 이동을 위한 트랜스폼
    private Rigidbody rb;            // Rigidbody 컴포넌트

    public float speed = 6f;         // 이동 속도
    public float lowJumpForce = 5f;  // 낮은 점프 힘
    public float highJumpForce = 10f; // 높은 점프 힘
    private bool isGrounded;         // 바닥에 있는지 확인
    private float jumpPressTime;     // 스페이스바 누른 시간
    private bool isJumping;          // 점프 중인지 확인
    private bool jumpKeyReleased;    // 점프 키가 떼어졌는지 확인

    private Vector3 initialPosition = new Vector3(-19.64f, 2.69f, -4.13f); // 최초 위치
    private Quaternion initialRotation;  // 초기 회전값
    private Vector3 originalScale;   // 캐릭터의 원래 크기
    private Vector3 jumpSquashScale; // 점프 준비 시 크기
    private Vector3 jumpStretchScale; // 점프 시 크기

    private int level = 1;           // 초기 레벨 설정
    public Text levelText;           // 레벨을 표시할 Text UI 요소

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mTransform = transform;
        mTransform.position = initialPosition; // 시작 위치 설정

        initialRotation = mTransform.rotation; // 초기 회전값 저장

        // 캐릭터 크기 설정
        originalScale = mTransform.localScale; // 원래 크기 저장
        jumpSquashScale = new Vector3(originalScale.x, originalScale.y * 0.8f, originalScale.z * 1.2f); // 납작한 크기
        jumpStretchScale = new Vector3(originalScale.x, originalScale.y * 1.2f, originalScale.z * 0.8f); // 길쭉한 크기

        // 초기 레벨 UI에 표시
        UpdateLevelText();
    }

    void Update()
    {
        // 이동
        if (Input.GetKey(KeyCode.W))
        {
            mTransform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            mTransform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            mTransform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            mTransform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.R))
        {
            // 레벨에 따른 Y축 위치 계산 (레벨 1일 때 Y값 2.69, 그 후 100씩 증가)
            float levelBasedY = initialPosition.y + (level - 1) * 100f;

            // 최초 위치로 돌아가면서 레벨에 맞는 Y축 위치로 설정
            mTransform.position = new Vector3(initialPosition.x, levelBasedY+10, initialPosition.z);
            mTransform.rotation = initialRotation;
        }

        // 점프 처리
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space)) // 점프 키를 누르고 있는 동안
            {
                if (!isJumping) // 점프가 처음 시작될 때
                {
                    jumpPressTime = Time.time; // 누른 시간 기록
                    isJumping = true; // 점프가 시작되었음을 표시
                    jumpKeyReleased = false; // 키가 아직 떼어지지 않았음을 표시

                    // 점프 준비 모션 (납작하게)
                    mTransform.localScale = jumpSquashScale;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space) && !jumpKeyReleased) // 점프 키를 떼었을 때
            {
                float pressDuration = Time.time - jumpPressTime; // 스페이스바를 누른 시간 계산

                // 점프력을 설정 (0.5초 기준으로 낮은 점프/높은 점프 설정)
                float jumpForce = pressDuration < 0.5f ? lowJumpForce : highJumpForce;

                // 점프 실행
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Y축 속도 초기화
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프 힘 적용
                jumpKeyReleased = true; // 점프가 끝났음을 표시

                // 점프 모션 (길쭉하게)
                mTransform.localScale = jumpStretchScale;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌 시 점프 가능 상태로 전환
        isGrounded = true;
        isJumping = false;

        // 충돌 시 크기를 원래 크기로 복구
        mTransform.localScale = originalScale;
    }

    void OnCollisionExit(Collision collision)
    {
        // 바닥을 떠나면 점프가 안 되도록 설정
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jeep"))
        {
            Vector3 newPos = new Vector3(-20, 3, -1);
            mTransform.position = newPos;
        }
        else if (other.CompareTag("Finish")) // 'Finish' 태그에 도달했을 때
        {
            // 레벨 증가
            level++;
            UpdateLevelText();  // 레벨 텍스트 업데이트

            // 캐릭터 위치 이동 (Y축 +100)
            Vector3 finishPosition = new Vector3(-19.64f, mTransform.position.y + 120f, -4.13f);
            mTransform.position = finishPosition;
        }
    }

    // 레벨 텍스트 업데이트 함수
    private void UpdateLevelText()
    {
        levelText.text = "Level " + level;  // 레벨 텍스트 갱신
    }
}
