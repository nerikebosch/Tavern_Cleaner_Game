using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // This array will hold the 4 brains (Head, Body, Legs, Feet)
    private Animator[] animators;
    private InputAction moveAction;

    void Start()
    {
        // This instantly finds all 4 Animators inside your Visuals folder when the game starts!
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
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            // Tell all 4 body parts to walk!
            foreach (Animator anim in animators)
            {
                anim.SetFloat("Speed", direction.magnitude);
            }
        }
        else
        {
            // Tell all 4 body parts to idle!
            foreach (Animator anim in animators)
            {
                anim.SetFloat("Speed", 0f);
            }
        }
    }
}