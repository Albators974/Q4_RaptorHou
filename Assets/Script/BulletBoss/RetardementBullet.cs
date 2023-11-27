using System.Collections;
using UnityEngine;

public class RetardementBullet : MonoBehaviour
{
    public bool _isLunched;

    private Vector3 _initialPos;
    private Rigidbody2D _rb;
    private Vector3 _targetPos;
    private Vector3 _spawnPos;
    private float _tempsEcoule = 0f;
    private Animator _animator;

    void Start()
    {
        BossManager.instance._retardementBullet.Add(this);
        _isLunched = false;
        _rb = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isLunched)
        {
            _tempsEcoule += Time.deltaTime;

            float t = Mathf.Clamp01(_tempsEcoule / 1.0f);

            transform.position = Vector3.Lerp(_spawnPos, _targetPos, t);

            if (_tempsEcoule >= 1)
            {
                _isLunched = false;
                _animator.Play("RetardementBulletExplosion");
                StartCoroutine(WaitForInitialPos(1.5f));
            }
        }
    }

    public void Lunch(Vector3 pos, Vector3 spawn)
    {
        transform.position = spawn;
        _spawnPos = spawn;
        _tempsEcoule = 0;
        _targetPos = pos;
        _isLunched = true;
    }

    IEnumerator WaitForInitialPos(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = _initialPos;
        _rb.velocity = Vector2.zero;
    }
}
