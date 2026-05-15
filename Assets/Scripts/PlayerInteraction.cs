using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 1f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -9.81f;
    Vector3 velocity;
    bool isGrounded;

    public float interactRange = 2f;
    public TextMeshProUGUI interactText;

    [Header("Physics Grab Settings")]
    public Rigidbody holdPoint;
    public float breakDistance = 3.5f;

    [Header("Visuals")]
    public LineRenderer lineRenderer;
    [Tooltip("How much the magical line sags downwards")]
    public float lineSagAmount = 0.5f;

    private Animator[] animators;
    private GameObject currentTarget;
    private GameObject heldItem;
    private SpringJoint currentJoint;

    private InputAction interactAction;
    private InputAction dropAction;
    private InputAction moveAction;

    void Start()
    {
        animators = GetComponentsInChildren<Animator>();

        interactAction = new InputAction("Interact", binding: "<Keyboard>/e");
        interactAction.Enable();

        dropAction = new InputAction("Drop", binding: "<Keyboard>/q");
        dropAction.Enable();

        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();

        // Prepare the line renderer for a smooth curve
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 15; // 15 points makes a very smooth curve
        }
    }

    void OnDestroy()
    {
        interactAction.Dispose();
        dropAction.Dispose();
        moveAction.Dispose();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) { velocity.y = -2f; }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * this.speed * Time.deltaTime);

            foreach (Animator anim in animators) anim.SetFloat("Speed", direction.magnitude);
        }
        else
        {
            foreach (Animator anim in animators) anim.SetFloat("Speed", 0f);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (heldItem == null)
        {
            FindInteractable();
            if (currentTarget != null && interactAction.triggered) PickUpItem();
        }
        else
        {
            interactText.gameObject.SetActive(false);

            // Draw the curved magical line
            DrawCurvedLine();

            if (dropAction.triggered)
            {
                DropItem();
                return;
            }

            float currentDistance = Vector3.Distance(holdPoint.position, heldItem.transform.position);
            if (currentDistance > breakDistance) DropItem();
        }
    }

    void DrawCurvedLine()
    {
        if (lineRenderer == null || heldItem == null) return;

        lineRenderer.enabled = true;

        // Pushes it up to chest level (1.5f), further forward (0.8f), and slightly to the right (0.3f) near the hand
        Vector3 startPos = transform.position + (Vector3.up * 1.5f) + (transform.forward * 0.8f) + (transform.right * 0.3f);

        Collider itemCollider = heldItem.GetComponent<Collider>();
        Vector3 endPos;
        if (itemCollider != null)
        {
            endPos = itemCollider.bounds.center;
        }
        else
        {
            endPos = heldItem.transform.position;
        }

        // Find the middle point and push it up to make the arc
        Vector3 midPoint = startPos + (endPos - startPos) / 2f + (Vector3.up * lineSagAmount);

        // Calculate all 15 points along the curve
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = i / (float)(lineRenderer.positionCount - 1);
            Vector3 pointOnCurve = CalculateQuadraticBezierPoint(t, startPos, midPoint, endPos);
            lineRenderer.SetPosition(i, pointOnCurve);
        }
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    void FindInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange);
        currentTarget = null;

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Interactable"))
            {
                currentTarget = col.gameObject;
                break;
            }
        }
        interactText.gameObject.SetActive(currentTarget != null);
    }

    void PickUpItem()
    {
        foreach (Animator anim in animators) anim.SetBool("IsHolding", true);

        heldItem = currentTarget;
        Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();

        currentJoint = heldItem.AddComponent<SpringJoint>();
        currentJoint.connectedBody = holdPoint;

        currentJoint.spring = 50f;
        currentJoint.damper = 5f;
        currentJoint.maxDistance = 0f;
        currentJoint.autoConfigureConnectedAnchor = false;
        currentJoint.connectedAnchor = Vector3.zero;
        currentJoint.anchor = Vector3.zero;

        itemRb.mass = 0.1f;
        itemRb.constraints = RigidbodyConstraints.FreezeRotation;
        itemRb.linearDamping = 5f;
    }

    void DropItem()
    {
        foreach (Animator anim in animators) anim.SetBool("IsHolding", false);

        if (currentJoint != null) Destroy(currentJoint);

        if (heldItem != null)
        {
            Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();
            itemRb.mass = 1f;
            itemRb.constraints = RigidbodyConstraints.None;
            itemRb.linearDamping = 0f;
            itemRb.linearVelocity = Vector3.zero;
            itemRb.angularVelocity = Vector3.zero;
            heldItem = null;
        }
        if (lineRenderer != null) lineRenderer.enabled = false;
    }
}