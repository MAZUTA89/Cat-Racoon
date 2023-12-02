using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Data;
using UnityEngine;
using Zenject;

public class TopDownMovement : MonoBehaviour
{
    public float MovementSpeed = 5f;
    Rigidbody2D _rb;
    InputService _inputService;

    Vector2 _input;

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
        _rb.MovePosition(_rb.position + MovementSpeed
            * Time.fixedDeltaTime * _input);
        User.Instance.SendPlayerData.UpdatePosition(_rb.position);
    }
}


