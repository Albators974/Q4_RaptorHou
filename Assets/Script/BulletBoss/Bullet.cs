using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool _isLunched;
    public float _speed = 3f;
    public float _decelerationFactor = 0.1f;
    public Rigidbody2D _rb;
    public bool _decelerate = false;

    private Vector3 _initialPos;

    void Start()
    {
        _isLunched = false;
        _rb = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
    }

    private void Update()
    {
        if (_decelerate)
        {        
            _rb.velocity *= 1 - _decelerationFactor * Time.deltaTime;
        }
    }

    public void LunchingBullet(Vector2 directionNorNormalized, Vector3 lunchingPosition)
    {
        _isLunched = true;
        transform.position = lunchingPosition;
        _rb.velocity = directionNorNormalized.normalized * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isLunched = false;
        transform.position = _initialPos;
        _rb.velocity = Vector2.zero;
    }
}
