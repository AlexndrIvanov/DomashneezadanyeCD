using UnityEngine;

public class SpringMassController : MonoBehaviour
{
    [Header("References")]
    public Transform anchorPoint;

    [Header("Spring Parameters")]
    public float mass = 1f;
    public float stiffness = 10f;
    public float damping = 0.5f;
    public float initialDisplacement = 2f;

    [Header("Stop Thresholds")]
    public float stopVelocityThreshold = 0.02f;
    public float stopDisplacementThreshold = 0.02f;

    [Header("Simulation State")]
    public bool isRunning = false;

    private Rigidbody2D rb;
    private Vector2 equilibriumPosition;
    private float elapsedTime = 0f;
    private float maxDisplacement = 0f;

    public float ElapsedTime => elapsedTime;
    public float MaxDisplacement => maxDisplacement;
    public float CurrentDisplacement => transform.position.y - equilibriumPosition.y;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        equilibriumPosition = transform.position;
        ResetSystem();
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;

        Vector2 currentPosition = rb.position;
        float displacement = currentPosition.y - equilibriumPosition.y;
        float velocity = rb.linearVelocity.y;

        float springForce = -stiffness * displacement;
        float dampingForce = -damping * velocity;
        float totalForce = springForce + dampingForce;

        float acceleration = totalForce / mass;

        float newVelocityY = velocity + acceleration * Time.fixedDeltaTime;
        float newPositionY = currentPosition.y + newVelocityY * Time.fixedDeltaTime;

        rb.MovePosition(new Vector2(currentPosition.x, newPositionY));
        rb.linearVelocity = new Vector2(0f, newVelocityY);

        elapsedTime += Time.fixedDeltaTime;

        float absDisplacement = Mathf.Abs(displacement);
        if (absDisplacement > maxDisplacement)
            maxDisplacement = absDisplacement;

        // јвтоостановка симул€ции:
        if (Mathf.Abs(newVelocityY) < stopVelocityThreshold &&
            Mathf.Abs(newPositionY - equilibriumPosition.y) < stopDisplacementThreshold)
        {
            rb.linearVelocity = Vector2.zero;
            rb.MovePosition(new Vector2(currentPosition.x, equilibriumPosition.y));
            isRunning = false;
        }
    }

    public void StartSimulation(float newMass, float newStiffness, float newDamping, float newInitialDisplacement)
    {
        mass = newMass;
        stiffness = newStiffness;
        damping = newDamping;
        initialDisplacement = newInitialDisplacement;

        ResetSystem();

        Vector2 startPos = equilibriumPosition + new Vector2(0f, initialDisplacement);
        rb.position = startPos;
        transform.position = startPos;
        rb.linearVelocity = Vector2.zero;

        maxDisplacement = Mathf.Abs(initialDisplacement);
        isRunning = true;
    }

    public void ResetSystem()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.position = equilibriumPosition;
        rb.position = equilibriumPosition;

        elapsedTime = 0f;
        maxDisplacement = 0f;
        isRunning = false;
    }
}