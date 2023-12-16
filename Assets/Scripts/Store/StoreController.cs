using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class StoreController : MonoBehaviour
{
    public Animator animator;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleStore()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
}
