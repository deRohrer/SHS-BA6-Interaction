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

        //Animate based on holding state
         if (isHolding)
        {
            character.animator.Play("Idle holding");
        }
        else
        {
            character.animator.Play("IDLE Milly bob");
        }

        // Walk
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            character.SetState(new WalkingState(character));
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && character.IsGrounded())
        {
            character.SetState(new JumpingState(character));
        }

        // Pick up or put down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHolding)
            {
                character.PutDownObject();
            }
            else if (character.IsObjectNearby(out GameObject nearbyObject))
            {
                character.PickUpObject(nearbyObject);
            }
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
        // Animate
        if (isHolding)
        {
            character.animator.Play("walk holding");
        }
        else
        {
            character.animator.Play("WALK Milly");
        }

        // Flip
        if (moveHorizontal != 0)
        {
            character.transform.localScale = new Vector3(Mathf.Sign(moveHorizontal) * Mathf.Abs(character.transform.localScale.x), character.transform.localScale.y, character.transform.localScale.z);
        }

        // Idle if stopped
        if (moveHorizontal == 0)
        {
            character.SetState(new IdleState(character));
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && character.IsGrounded())
        {
            character.SetState(new JumpingState(character));
        }

        // Pick up or put down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHolding)
            {
                character.PutDownObject();
            }
            else if (character.IsObjectNearby(out GameObject nearbyObject))
            {
                character.PickUpObject(nearbyObject);
            }
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Walking State");
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
};



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
        // Stop the object's movement
        objRb.linearVelocity = Vector2.zero;
        objRb.angularVelocity = 0f;


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

            GameObject destination = FindMatchingDestination(heldObject);
            if (destination != null)
            {
                heldObject.transform.SetParent(null);

                DestinationTarget destinationTarget = destination.GetComponent<DestinationTarget>();
                if (destinationTarget != null && destinationTarget.itemDropPoint != null)
                {
                    Debug.Log("putting in item drop point");
                    heldObject.transform.position = destinationTarget.itemDropPoint.position;
                }
                else
                {
                    Debug.Log("falling back on putting item on destination position");
                    heldObject.transform.position = destination.transform.position;
                }

                if (objCollider != null)
                    objCollider.enabled = false;

                DestinationTarget target = destination.GetComponent<DestinationTarget>();

                HoldableItem holdableItem = heldObject.GetComponent<HoldableItem>();

                if (holdableItem != null)
                {
                    Debug.Log("Calling OnItemPlaced on: " + target.name);

                    target.OnItemPlaced(holdableItem);//start sequence of things that happen when object is placed
                }
            }
            else
            {
                if (objRb != null)
                {
                    objRb.bodyType = RigidbodyType2D.Dynamic;
                    objRb.freezeRotation = false;
                }

                if (objCollider != null && playerCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, objCollider, false);
                }

                heldObject.transform.SetParent(null);
            }

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
    private GameObject FindMatchingDestination(GameObject item)
    {
        if (item == null) return null;

        HoldableItem holdable = item.GetComponent<HoldableItem>();
        if (holdable == null) return null;

        float detectionRadius = 2f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var col in colliders)
        {
            DestinationTarget destination = col.GetComponent<DestinationTarget>();
            if (destination != null && destination.acceptedType == holdable.itemType)
            {
                return col.gameObject;
            }
        }

        return null;
    }

}
