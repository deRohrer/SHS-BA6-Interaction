using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2f;
    public KeyCode movefwdKey = KeyCode.RightArrow;
    public KeyCode movebwdKey = KeyCode.LeftArrow;
    public KeyCode moveuwdKey = KeyCode.UpArrow;

    private Rigidbody2D rb;

    void Start()
    {
        // Ensure the GameObject has a Rigidbody component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Optional: Freeze rotation to prevent tipping over
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(movefwdKey))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (Input.GetKey(movebwdKey))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (Input.GetKey(moveuwdKey))
        {
            transform.Translate(Vector3.up * 2 * speed * Time.deltaTime);
        }
    }
}
