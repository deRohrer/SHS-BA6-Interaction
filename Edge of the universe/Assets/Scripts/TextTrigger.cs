using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public GameObject milly;
    public string text;
    private bool promptShown = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name + ", Tag: " + other.tag);
        if (promptShown) return;

        if (other.CompareTag("Player"))
        {
            PromptManager.Instance.ShowPrompt(text);
            promptShown = true;
        }
    }
}
