using UnityEngine;

public class Object : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;// objects begin as kinematic and turn dynamic when dropped

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
