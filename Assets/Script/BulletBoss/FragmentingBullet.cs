using System.Collections.Generic;
using UnityEngine;

public class FragmentingBullet : MonoBehaviour
{
    public bool _isLunched;

    public Vector3 _initialPos;
    public Rigidbody2D _rb;
    public Vector3 _targetPos;
    public Vector3 _spawnPos;
    public float _tempsEcoule = 0f;
    public bool _positioned;

    public int _fragmentingNbr;
    public int _duplicationToDO;
    public List<FragmentingBullet> _bullet;
    public Vector3 _targetPosLunch;
    public float _distanceToParkour;

    void Start()
    {
        _initialPos = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        BossManager.instance._fragmentingBulletList.Add(this);
        _positioned = false;
    }

    private void Update()
    {
        if (_isLunched)
        {
            _tempsEcoule += Time.deltaTime;

            float t = Mathf.Clamp01(_tempsEcoule / 5.0f);

            transform.position = Vector3.Lerp(_spawnPos, _targetPos, t);

            if (_tempsEcoule >= 5)
            {
                _isLunched = false;
                LunchComplete();
                transform.position = _initialPos;
                _rb.velocity = Vector2.zero;
                _tempsEcoule = 0f;
            }
        }
    }

    public void Lunch(int fragmentingNbr,int duplicationToDO, List<FragmentingBullet> bullet, Vector3 spawnPos, Vector3 targetPos, float distanceToParkour)
    {
        _isLunched = true;
        _spawnPos = spawnPos;
        _targetPos = targetPos;

        _fragmentingNbr = fragmentingNbr;
        _duplicationToDO = duplicationToDO;
        _bullet = bullet;
        _targetPosLunch = targetPos;
        _distanceToParkour = distanceToParkour;
    }

    public void LunchComplete()
    {
        if (_duplicationToDO > 0)
        {
            int dupli = _duplicationToDO - 1;

            for (int i = 0; i < _fragmentingNbr + 1; i++)
            {
                float angle = i * (360f / _fragmentingNbr);

                float x = transform.position.x + _distanceToParkour * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = transform.position.y + _distanceToParkour * Mathf.Sin(Mathf.Deg2Rad * angle);

                Vector3 targetPosition = new Vector3(x, y, 0f);

                foreach (var oneBullet in _bullet)
                {
                    if (!oneBullet._isLunched)
                    {
                        oneBullet.Lunch(_fragmentingNbr, dupli, _bullet, transform.position, targetPosition, _distanceToParkour);
                        break;
                    }
                }
            }
        }
    }
}
