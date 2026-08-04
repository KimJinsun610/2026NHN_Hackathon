using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class AutoDestroyEffect : MonoBehaviour
{
    void Start()
    {
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            float animLength = animator.GetCurrentAnimatorStateInfo(0).length;

            Destroy(gameObject, animLength);
        }
        else
        {
            Destroy(gameObject, 1.0f);
        }
    }
}