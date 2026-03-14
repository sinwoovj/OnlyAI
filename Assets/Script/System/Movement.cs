using UnityEngine;
public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 MoveDirection { get; set; } = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (MoveDirection *
            PlayerController.Instance.moveSpeed * Time.fixedDeltaTime));
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * PlayerController.Instance.jumpForce, ForceMode.Impulse);
    }
}
