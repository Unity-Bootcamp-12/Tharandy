using System;
using System.Collections;
using UnityEngine;

public class EndingThanos: MonoBehaviour
{
    public Transform destination { get; set; }

    [SerializeField] private float _speed;
    private Vector3 _direction;
    private Animator _animator;
    private bool _isAlreadySnap = false;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsAppearance", true);
        _direction = (destination.position - transform.position).normalized;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, destination.position) > 0.05f)
        {
            transform.position += _direction * _speed * Time.deltaTime;
        }
        else
        {
            if (_isAlreadySnap == false)
            {
                _isAlreadySnap = true;
                _animator.SetTrigger("IsEnd");
                SoundManager.Instance.PlaySfx("ThanosFinger");
            }
        }
    }
    
    
}
