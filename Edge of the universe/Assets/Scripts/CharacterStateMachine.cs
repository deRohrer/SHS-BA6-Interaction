using UnityEngine;

// Define the ICharacterState interface
public interface ICharacterState
{
    void OnEnter();
    void Update();
    void OnExit();
}

// Idle state for standing still
public class IdleState : ICharacterState
{
    private readonly CharacterStateMachine character;
    public Animator animator;

    public IdleState(CharacterStateMachine character)
    {
        this.character = character;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Idle State");
        

    }

    public void Update()
    {
        bool isHolding = character.heldObject != null;

        if (isHolding)
        {
            character.animator.Play("Idle holding");
        }
        else
        {
            character.animator.Play("IDLE Milly bob");
        }
        
        // Start walking if horizontal input is pressed
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            character.SetState(new WalkingState(character));
        }

        // Jump if up arrow or W is pressed and grounded
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && character.IsGrounded())
        {
            character.SetState(new JumpingState(character));
        }

        // Pick up if spacebar is pressed and object is nearby
        if (Input.GetKeyDown(KeyCode.Space) && character.IsObjectNearby(out _))
        {
            character.SetState(new PickingUpState(character));
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Idle State");
    }
}

// Walking state for horizontal movement
public class WalkingState : ICharacterState
{
    private readonly CharacterStateMachine character;

    public WalkingState(CharacterStateMachine character)
    {
        this.character = character;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Walking State");

    }

    public void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = character.rb.linearVelocity;
        velocity.x = moveHorizontal * character.moveSpeed;
        character.rb.linearVelocity = velocity;

        bool isHolding = character.heldObject != null;
   

        //change animation based on if theyre holding
        if (isHolding)
        {
           character.animator.Play("walk holding");

        }
        else
        {
            character.animator.Play("WALK Milly");
        }
            // Flip the character's scale to face the movement direction
            if (moveHorizontal != 0)
        {
            character.transform.localScale = new Vector3(Mathf.Sign(moveHorizontal) * Mathf.Abs(character.transform.localScale.x), character.transform.localScale.y, character.transform.localScale.z);
        }


        // Go idle if no movement
        if (moveHorizontal == 0)
        {
            character.SetState(new IdleState(character));
        }

        // Jump if up arrow or W is pressed and grounded
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && character.IsGrounded())
        {
            character.SetState(new JumpingState(character));
        }

        // Pick up if spacebar is pressed and object is nearby
        if (Input.GetKeyDown(KeyCode.Space) && character.IsObjectNearby(out _))
        {
            character.SetState(new PickingUpState(character));
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Walking State");
    }
}

// State for holding an object
public class HoldingState : ICharacterState
{
    private readonly CharacterStateMachine character;

    public HoldingState(CharacterStateMachine character)
    {
        this.character = character;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Holding State");
    }

    public void Update()
    {
        character.SetState(new IdleState(character));
        // Drop object with spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.PutDownObject();
            character.SetState(new IdleState(character));
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            character.SetState(new WalkingState(character));
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Holding State");
    }
}

// State for picking up an object
public class PickingUpState : ICharacterState
{
    private readonly CharacterStateMachine character;
    private float enterTime;

    public PickingUpState(CharacterStateMachine character)
    {
        this.character = character;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Picking Up State");
        enterTime = Time.time;

        if (character.IsObjectNearby(out GameObject nearbyObject))
        {
            character.PickUpObject(nearbyObject);
        }
    }

    public void Update()
    {
        if (Time.time > enterTime + character.stateTransitionDelay)
        {
            character.SetState(new HoldingState(character));
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Picking Up State");
    }
}

// State for jumping
public class JumpingState : ICharacterState
{
    private readonly CharacterStateMachine character;

    public JumpingState(CharacterStateMachine character)
    {
        this.character = character;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Jumping State");
        character.Jump();
    }

    public void Update()
    {
        Debug.Log("Grounded: " + character.IsGrounded());
        // Return to walking when grounded
        if (character.IsGrounded())
        {
            character.SetState(new WalkingState(character));
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Jumping State");
    }
}

// State for putting down an object (not used in this flow, but kept for completeness)
public class PuttingDownState : ICharacterState
{
    private readonly CharacterStateMachine character;
    private float enterTime;

    public PuttingDownState(CharacterStateMachine character)
    {
        this.character = character;
    }

    public void OnEnter()
    {
        Debug.Log("Entered Putting Down State");
        enterTime = Time.time;
        character.PutDownObject();
    }

    public void Update()
    {
        if (Time.time > enterTime + character.stateTransitionDelay)
        {
            character.SetState(new WalkingState(character));
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Putting Down State");
    }
}

// Main state machine MonoBehaviour for 2D
public class CharacterStateMachine : MonoBehaviour
{
    public Rigidbody2D rb; // 2D Rigidbody
    public Animator animator;//milly animations
    public float moveSpeed = 5f; // Horizontal move speed
    public float jumpForce = 7f; // Jump force
    public Transform holdPoint; // Where to hold objects
    public GameObject heldObject; // Currently held object
    public float objectDetectionRadius = 3f; // Pickup radius
    public float stateTransitionDelay = 0.1f; // Delay for state transitions
    public LayerMask pickupLayer; // Layer for pickup objects
    private ICharacterState currentState;

    private void Start()
    {
        // Ensure Rigidbody2D is present
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing! Adding one dynamically.");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Ensure holdPoint is assigned
        if (holdPoint == null)
        {
            Debug.LogError("HoldPoint is not assigned! Please assign it in the Inspector.");
        }

        // Validate object detection radius
        if (objectDetectionRadius <= 0)
        {
            Debug.LogWarning("Object detection radius must be greater than zero. Setting to default value of 1.");
            objectDetectionRadius = 1f;
        }

        SetState(new IdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void SetState(ICharacterState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();
    }

    public void Jump()
    {
        Debug.Log("Jump called");

        // Only jump if grounded
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public bool IsGrounded()
    {
        // Raycast down to check for ground
        Vector2 position = transform.position;
        Vector2 size = GetComponent<Collider2D>().bounds.size;
        float extraHeight = 0.05f;

        RaycastHit2D hit = Physics2D.BoxCast(
            position,
            size,
            0f,
            Vector2.down,
            extraHeight,
            LayerMask.GetMask("Ground") // Make sure your ground objects are on a "Ground" layer
        );
        return hit.collider != null;
    }

    public void PickUpObject(GameObject obj)
    {
        Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
        Collider2D objCollider = obj.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (objRb == null || objCollider == null || playerCollider == null)
        {
            Debug.LogError("Missing Rigidbody2D or Collider2D!");
            return;
        }

        heldObject = obj;

        // Ignore collisions between player and held object to prevent interactions
        Physics2D.IgnoreCollision(playerCollider, objCollider, true);

        // Parent and reset transform
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Set kinematic and freeze rotation to disable physics on object
        objRb.bodyType = RigidbodyType2D.Kinematic;
        objRb.freezeRotation = true;
    }


    public void PutDownObject()
    {
        if (heldObject != null)
        {
            Rigidbody2D objRb = heldObject.GetComponent<Rigidbody2D>();
            Collider2D objCollider = heldObject.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();

            if (objRb != null)
            {
                //objRb.bodyType = RigidbodyType2D.Dynamic;
                objRb.freezeRotation = false;
            }

            if (objCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, objCollider, false);
            }

            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }


    public bool IsObjectNearby(out GameObject nearbyObject)
    {
        Debug.Log("Checking for nearby objects");
        // Use OverlapCircle for 2D detection
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, objectDetectionRadius, pickupLayer);
        Debug.Log($"Found {colliders.Length} colliders in range");

        foreach (var collider in colliders)
        {
            Debug.Log($"Found object: {collider.name}, Tag: {collider.tag}, Layer: {LayerMask.LayerToName(collider.gameObject.layer)}");
            if (collider.CompareTag("PickUp"))
            {
                nearbyObject = collider.gameObject;
                return true;

            }
        }
        nearbyObject = null;
        return false;
    }
}
