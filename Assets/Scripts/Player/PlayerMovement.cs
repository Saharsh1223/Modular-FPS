//Basic Player movement

using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Drag")]
    [SerializeField] private float drag = 6f;

    private Vector3 moveDirection;

    private float h;
    private float v;

    private void Start() => rb.freezeRotation = true;

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        moveDirection = (orientation.forward * v + orientation.right * h).normalized;

        rb.drag = drag;
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
    }
}