using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mTransform = transform;
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

        // 점프 처리
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space)) // 점프 키를 누르고 있는 동안 점프 시간 측정
            {
                if (!isJumping) // 스페이스바가 처음 눌렸을 때
                {
                    jumpPressTime = Time.time; // 누른 시간 기록
                    isJumping = true; // 점프가 시작되었음을 표시
                    jumpKeyReleased = false; // 키가 아직 떼어지지 않았음을 표시
                }
            }

            // 스페이스바를 뗀 경우
            if (Input.GetKeyUp(KeyCode.Space) && !jumpKeyReleased)
            {
                float pressDuration = Time.time - jumpPressTime; // 스페이스바를 누른 시간 계산

                // 점프력을 설정 (0.5초 기준으로 낮은 점프/높은 점프 설정)
                float jumpForce = pressDuration < 0.5f ? lowJumpForce : highJumpForce;

                // 점프 실행
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Y축 속도 초기화
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프 힘 적용
                jumpKeyReleased = true; // 점프가 끝났음을 표시
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 바닥에 닿으면 점프가 가능하도록 설정
        if (collision.contacts[0].normal.y > 0.5f) // 바닥과 부딪쳤을 때
        {
            isGrounded = true;
            isJumping = false; // 바닥에 닿으면 점프 상태가 아니게 설정
        }
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
    }
}
