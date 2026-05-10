using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // --- NEW GRAVITY VARIABLES ---
    public float gravity = -9.81f;
    Vector3 velocity;
    bool isGrounded;
    // -----------------------------

    private Animator[] animators;
    private InputAction moveAction;

    void Start()
    {
        animators = GetComponentsInChildren<Animator>();

        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.Enable();
    }

    void OnDestroy()
    {
        moveAction.Dispose();
    }

    void Update()
    {
        // --- NEW GRAVITY CHECK ---
        // The Character Controller has a built-in sensor to check if it's touching the floor
        isGrounded = controller.isGrounded;

        // If we are touching the floor, stop building up downward speed
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // We use -2f instead of 0 to ensure she stays firmly snapped to the ground
        }
        // -------------------------

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Move horizontally
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            foreach (Animator anim in animators)
            {
                anim.SetFloat("Speed", direction.magnitude);
            }
        }
        else
        {
            foreach (Animator anim in animators)
            {
                anim.SetFloat("Speed", 0f);
            }
        }

        // --- APPLY GRAVITY ---
        // Constantly pull her down over time
        velocity.y += gravity * Time.deltaTime;

        // Move vertically
        controller.Move(velocity * Time.deltaTime);
        // ---------------------
    }
}