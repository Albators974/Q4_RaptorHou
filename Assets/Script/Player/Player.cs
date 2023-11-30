using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float _speed = 5f;
    public List<PlayerBullet> _bullets;
    public List<Vector3> _lunchingPos;
    public Vector3 _pos1;
    public Vector3 _pos2;
    public int _nbrBossKilled = 0;
    public int _maxHp = 5;
    public int _score = 4000;
    public TextMeshProUGUI _hpIndication;
    public TextMeshProUGUI _scoreText;
    public float _invincibleTime;
    public CircleCollider2D _coreCollider;
    public AudioSource _shipTouched;
    public AudioSource _firing;
    public bool _canShoot;
    public string _typeOfBonus;
    public Image _bonusImage;
    public GameObject _shield;

    private int _hp;
    private Rigidbody2D _rb;
    private bool _autoMod;
    private SpriteRenderer _spriteRenderer;
    private bool _shielded;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lunchingPos = new List<Vector3>();
        _pos1 = transform.position - new Vector3(0.2f, -0.2f, 0);
        _pos2 = transform.position + new Vector3(0.2f, 0.2f, 0);
        _lunchingPos.Add(_pos1);
        _lunchingPos.Add(_pos2);
        _hp = _maxHp;
        _hpIndication.text = "Life point : " + _hp + " / " + _maxHp;
        _scoreText.text = "Score : " + _score;
        _shield.SetActive(false);
        Cursor.visible = false;
        _bonusImage.color = Color.black;
    }

    private void Update()
    {
        _lunchingPos.Clear();
        _pos1 = transform.position - new Vector3(0.2f, -0.2f, 0);
        _pos2 = transform.position + new Vector3(0.2f, 0.2f, 0);
        _lunchingPos.Add(_pos1);
        _lunchingPos.Add(_pos2);
    }

    public void Mooving(InputAction.CallbackContext ctx)
    {
        Vector2 _direction = ctx.ReadValue<Vector2>();

        _rb.velocity = _direction.normalized * _speed;
    }

    public void UseBonus()
    {
        _bonusImage.color = Color.black;
        switch (_typeOfBonus)
        {
            case "Shield":
                Shield();
                break;
            default:
                break;
        }
    }

    public void Switching()
    {
        _autoMod = !_autoMod;
        if (_autoMod)
        {
            foreach (var pos in _lunchingPos)
            {
                if (_canShoot)
                {
                    StartCoroutine(BulletFire(pos, _lunchingPos.IndexOf(pos)));
                }
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void Firing(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            foreach (var pos in _lunchingPos)
            {
                if (_canShoot)
                {
                    StartCoroutine(BulletFire(pos, _lunchingPos.IndexOf(pos)));
                }
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private void UpdateLunchingPos(int index)
    {
        StartCoroutine(BulletFire(_lunchingPos[index], index));
    }

    public void AddBullet(PlayerBullet bullet)
    {
        _bullets.Add(bullet);
    }

    IEnumerator BulletFire(Vector3 lunchingPos, int index)
    {
        foreach (var bullet in _bullets)
        {
            if (!bullet._isLunched)
            {
                bullet.gameObject.transform.position = lunchingPos;
                bullet.Lunching();
                break;
            }
        }
        
        yield return new WaitForSeconds(0.1f);
        _firing.Play();
        UpdateLunchingPos(index);
    }

    public void Shield()
    {
        _bonusImage.sprite = null;
        _typeOfBonus = null;
        _shielded = true;
        _shield.SetActive(true);
        Invoke("StopShield", 2f);
    }

    public void StopShield()
    {
        _shielded = false;
        _shield.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !_shielded)
        {
            _hp--;
            _hpIndication.text = "Life point : " + _hp + " / " + _maxHp;
            _shipTouched.Play();
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0.8f, 0.8f);
                Invoke("StopVibratingGamepad", 0.5f);
            }
            InvincibleTimeTouchStart();
        }

        if (_hp <= 0)
        {
            GameManager.instance.GameEnd();
        }
    }

    public void StopVibratingGamepad()
    {
        Gamepad.current.ResetHaptics();
    }

    public void InvincibleTimeTouchStart()
    {
        _coreCollider.enabled = false;
        _spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        Invoke("InvincibleTimeTouchEnd", _invincibleTime);
    }

    public void InvincibleTimeTouchEnd()
    {
        _coreCollider.enabled = true;
        _spriteRenderer.color = new Color(1, 1, 1, 1);

    }
}
