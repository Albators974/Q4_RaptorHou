using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public bool _isLunched;

    private Player _player;
    private Vector2 _initialPosition;
    private Rigidbody2D _rb;

    void Start()
    {
        _player = GameManager.instance._player;
        _player.AddBullet(this);
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Lunching()
    {
        _isLunched = true;
        _rb.velocity = new Vector2(0, 20);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isLunched = false;
        transform.position = _initialPosition;
        _rb.velocity = Vector2.zero;

        if (collision.gameObject.tag != "Boss" ) 
        {
            _player._score -= 5;
            _player._scoreText.text = "Score : " + _player._score;
        }
        else
        {
            _player._score += 2;
            _player._scoreText.text = "Score : " + _player._score;
        }
    }
}
