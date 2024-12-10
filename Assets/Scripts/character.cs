using UnityEngine;
using UnityEngine.UI;

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

    private Vector3 initialPosition = new Vector3(-19.64f, 7f, -4.13f); // 최초 위치
    private Quaternion initialRotation;  // 초기 회전값
    private Vector3 originalScale;   // 캐릭터의 원래 크기
    private Vector3 jumpSquashScale; // 점프 준비 시 크기
    private Vector3 jumpStretchScale; // 점프 시 크기

    private static float prevX;

    private int level = 1;           // 초기 레벨 설정

    private static string difficulty = "Easy";  // 기본 난이도

    private static bool canJump = true;         // 점프 가능 여부

    public Text levelText;
    public Text difficultyText;

    public GameObject[] spacePrefabs; // 생성할 프리팹

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
        UpdateDifficultyText();
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
            mTransform.position = new Vector3(initialPosition.x, levelBasedY, initialPosition.z);
            mTransform.rotation = initialRotation;
        }

        if (Input.GetKey(KeyCode.E))
        {
            mTransform.rotation = initialRotation;
        }

        // 점프 처리
        if (isGrounded && canJump) // 점프가 가능할 때만 실행
        {
            Debug.Log(canJump);
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
            float levelBasedY = initialPosition.y + (level - 1) * 100f;
            Vector3 newPos = new Vector3(-20, levelBasedY, -1);
            mTransform.position = newPos;
        }
        else if (other.CompareTag("Finish")) // 'Finish' 태그에 도달했을 때
        {
            // 레벨 증가
            level++;
            UpdateLevelText();  // 레벨 텍스트 업데이트

            // 캐릭터 위치 이동 (Y축 +100)
            Vector3 finishPosition = new Vector3(-19.64f, mTransform.position.y + 100f, -4.13f);
            mTransform.position = finishPosition;

            // 레벨 증가 후 텍스트를 갱신하여 화면에 반영
            Debug.Log("Level increased to: " + level);
        }
        else if (other.CompareTag("Space")) // 'Space' 태그에 도달했을 때
        {

            int randomIndex = Random.Range(0, spacePrefabs.Length);

            // 충돌한 스페이스의 위치를 가져오기
            Vector3 spacePosition = other.transform.position;

            // X축을 -0.06으로 조정
            Debug.Log(spacePosition.x);
            Debug.Log(prevX);
            // 프리팹 생성
            if (spacePosition.x != prevX)
            {
                prevX = spacePosition.x;
                spacePosition.x += 8f;
                Instantiate(spacePrefabs[randomIndex], spacePosition, Quaternion.identity);
            }
        }
    }

    // 레벨 텍스트 업데이트 함수
    private void UpdateLevelText()
    {
        levelText.text = "Level " + level;  // 레벨 텍스트 갱신
    }

    // 난이도 텍스트를 업데이트하는 함수
    private void UpdateDifficultyText()
    {
        difficultyText.text = difficulty;
    }

    // Easy 난이도 설정
    public void SetEasyDifficulty()
    {
        difficulty = "Easy";  // 난이도 설정
        speed = 6f;  // Easy 모드의 속도
        lowJumpForce = 5f;  // Easy 모드의 낮은 점프 힘
        highJumpForce = 10f;  // Easy 모드의 높은 점프 힘
        canJump = true; // 점프 가능

        Debug.Log("Easy difficulty activated");

        // 난이도 텍스트 업데이트
        UpdateDifficultyText();
    }

    // Hard 난이도 설정
    public void SetHardDifficulty()
    {
        difficulty = "Hard";  // 난이도 설정
        speed = 4f;  // Hard 모드의 속도
        lowJumpForce = 0f;  // Hard 모드의 낮은 점프 힘
        highJumpForce = 0f;  // Hard 모드의 높은 점프 힘
        canJump = false; // 하드 모드에서는 점프 비활성화

        Debug.Log("Hard difficulty activated");
        Debug.Log(canJump);

        // 난이도 텍스트 업데이트
        UpdateDifficultyText();
    }
}
