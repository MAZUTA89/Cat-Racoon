using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;
using System;
using UnityEngine;
using Zenject;

public class TopDownMovement : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float _smoothTime = 1f;
    Rigidbody2D _rb;
    InputService _inputService;

    Vector2 _input;
    Vector2 _velocity;
    Vector2 _currPos;
    [Inject]
    public void Constructor(InputService inputService)
    {
        _inputService = inputService;
    }
    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        _input = _inputService.GetMovement();
    }

    private void FixedUpdate()
    {
        Vector2 newPos = _rb.position + _input;
        Communicator.SendData?.UpdatePosition(newPos);
        _currPos = _rb.position;
        newPos = Vector2.SmoothDamp(_currPos, newPos, ref _velocity,
            Time.fixedDeltaTime /** MovementSpeed*/, MovementSpeed);
        _rb.MovePosition(newPos);
    }

    
}


