using UnityEngine;

public class VillagerAnimationController : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // A key to trigger gathering (you can change this input as needed)
    public KeyCode gatherKey = KeyCode.M; // 'M' key as an example

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for gathering input (change this to other conditions as needed)
        if (Input.GetKeyDown(gatherKey))
        {
            // Set isGathering to true, starting the gathering animation
            animator.SetBool("isGathering", true);
        }

        // You can stop gathering with another key or condition if needed (like after a certain time)
        if (Input.GetKeyUp(gatherKey))
        {
            // Set isGathering to false, returning to idle animation
            animator.SetBool("isGathering", false);
        }
    }
}
