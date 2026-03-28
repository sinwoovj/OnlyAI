using UnityEngine;
public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 MoveDirection { get; set; } = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 원신처럼 캐릭터가 넘어지지 않게 회전축 고정 (X, Z축)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        // 이동 입력이 있을 때만 위치 이동
        if (MoveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + (MoveDirection * PlayerController.Instance.moveSpeed * Time.fixedDeltaTime));
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * PlayerController.Instance.jumpForce, ForceMode.Impulse);
    }
}
