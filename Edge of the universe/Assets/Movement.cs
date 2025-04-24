using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2f;
    public KeyCode movefwdKey = KeyCode.RightArrow;
    public KeyCode movebwdKey = KeyCode.LeftArrow;


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
    }
}
