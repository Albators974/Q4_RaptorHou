using UnityEngine;

public class LittleBullet : MonoBehaviour
{
    public bool _isLunched;
    public float _speed = 3f;
    public float _decelerationFactor = 0.1f;
    public Rigidbody2D _rb;

    private Vector3 _initialPos;

    void Start()
    {
        BossManager.instance._littleBullet.Add(this);
        _isLunched = false;
        _rb = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
    }

    private void Update()
    {
        _rb.velocity *= (1 - _decelerationFactor * Time.deltaTime);
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
        _rb.gravityScale = 0;
        Debug.Log(collision.gameObject.name);
    }
}
