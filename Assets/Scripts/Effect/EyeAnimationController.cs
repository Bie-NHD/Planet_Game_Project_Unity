using UnityEngine;

public class EyeAnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetCloseEye(bool isClose)
    {
        if (_animator != null)
            _animator.SetBool("IsCloseEye", isClose);
    }
}
