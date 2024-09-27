using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstBoss : MonoBehaviour
{
    private float _timeToWAitForFirtsAttackSchemeToEnd = 1f;
    private int _waveNumber;
    private int _numberOfPointsFirstAttack = 40;
    private int _numberOfPointsSecondAttack = 32;
    private int _numberOfPointsThirdAttack = 4;
    private int _numberOfPointsFourthAttack = 3;
    private int _numberOfWaveSixthAttack = 10;
    private float _timeTorReachPlayerSixthAttack = 1.5f;
    private int _valeurOffset = 5;
    private int _fragmentingNbr;
    private float _distanceToParkour;
    private int _duplicationToDO;
    private float _distanceSpanwBoss;
    private int _currentDeplacementNbrSecondAttackScheme = 0;
    private int _random;
    private int _modulingBonus = 0;
    private int _numberHpToDecreasToSpawnBonus;

    private TextMeshProUGUI _countDown;
    private List<LittleBullet> _littleBulletList;
    private List<Vector3> _positionSecondAttackScheme;
    private List<BigBullet> _bigBulletList;
    private List<RetardementBullet> _retardementBulletList;
    private List<FragmentingBullet> _fragmentingBulletList;
    private CircleCollider2D _circleCollider2;
    private List<Vector3> _thirdAttackBulletSpawn;
    private GameObject _light;
    private List<GameObject> _listBonus;

    public int _lifePoint = 1000000;

    void Start()
    {
        _littleBulletList = BossManager.instance._littleBullet;
        _bigBulletList = BossManager.instance._bigBullet;
        _retardementBulletList = BossManager.instance._retardementBullet;
        _fragmentingBulletList = BossManager.instance._fragmentingBulletList;
        _positionSecondAttackScheme = BossManager.instance._firstBossPositionSecondAttackScheme;
        _positionSecondAttackScheme.Add(transform.position);
        _thirdAttackBulletSpawn = new List<Vector3>();
        _fragmentingNbr = BossManager.instance._fragmentingNbrFirstBoss;
        _distanceToParkour = BossManager.instance._distanceToParkourFirstBoss;
        _duplicationToDO = BossManager.instance._duplicationToDOFirstBoss;
        _distanceSpanwBoss = BossManager.instance._distanceSpanwBossFirstBoss;
        _circleCollider2.enabled = false;
        _light.SetActive(false);
        _countDown = GameManager.instance._bossCountDown;
        GameManager.instance._player._canShoot = false;
        _lifePoint = PlayerPrefs.GetInt("BossMaxHp");
        GameManager.instance._hpSlider.maxValue = _lifePoint;

        if (PlayerPrefs.GetInt("Impossible") == 0)
        {         
            _numberHpToDecreasToSpawnBonus = _lifePoint / 4;
        }
        else
        {
            _numberHpToDecreasToSpawnBonus = 100;
        }

        StartCoroutine(SpawningTime());
    }

    public void FirstAttackScheme()
    {
        StartCoroutine(SpawningBulletFirstAttackScheme());
        StartCoroutine(TimeToWaitForEndingAttackScheme(_timeToWAitForFirtsAttackSchemeToEnd));
    }


    public void SecondAttackScheme()
    {
        if (_currentDeplacementNbrSecondAttackScheme == _positionSecondAttackScheme.Count)
        {
            ResetSecondAttack();
            return;
        }
        
        BossDestinationAttack(_currentDeplacementNbrSecondAttackScheme);
    }
    private void BossDestinationAttack(int posIndex)
    {
        transform.position = _positionSecondAttackScheme[posIndex];
        _currentDeplacementNbrSecondAttackScheme++;
        StartCoroutine(SpawningBulletSecondAttackScheme(_valeurOffset));
    }

    private void ResetSecondAttack()
    {
        StopAllCoroutines();
        _currentDeplacementNbrSecondAttackScheme = 0;
        NextAttackScheme();
    }

    public void ThirdAttackScheme()
    {
        StartCoroutine(SpawningBulletThirdAttackScheme());
    }

    public void FourthAttackScheme()
    {
        StartCoroutine(SpawningBulletFourthAttackScheme(_valeurOffset));
    }

    public void FifthAttackScheme() 
    {
        StartCoroutine(SpawningBulletFifthAttackScheme(_fragmentingNbr,_distanceToParkour,_fragmentingBulletList, _duplicationToDO, _distanceSpanwBoss));
    }

    public void SixthAttackScheme()
    {
        StartCoroutine(SpawningBulletSixthAttackScheme(_timeTorReachPlayerSixthAttack));
    }

    public void NextAttackScheme()
    {
        _random = Random.Range(1, 7);
        switch (_random)
        {
            case 1:
                FirstAttackScheme();
                break;
            case 2:
                SecondAttackScheme();
                break;
            case 3:
                ThirdAttackScheme();
                break;
            case 4:
                FourthAttackScheme();
                break;
            case 5:
                FifthAttackScheme();
                break;
            case 6:
                StartCoroutine(WaitBeforLunchingSixthAttack());
                break;
            default:
                break;
        }
        if (PlayerPrefs.GetInt("Impossible") == 1)
        {
            _random = Random.Range(1, 7);
            switch (_random)
            {
                case 1:
                    FirstAttackScheme();
                    break;
                case 2:
                    SecondAttackScheme();
                    break;
                case 3:
                    ThirdAttackScheme();
                    break;
                case 4:
                    FourthAttackScheme();
                    break;
                case 5:
                    FifthAttackScheme();
                    break;
                case 6:
                    StartCoroutine(WaitBeforLunchingSixthAttack());
                    break;
                default:
                    break;
            }
        }
    }

    public void LightFlashing()
    {
        _light.SetActive(!_light.activeSelf);
        Invoke("LightFlashing", 1f);
    }

    IEnumerator SpawningTime()
    {
        StartCoroutine(TextCountDown());
        yield return new WaitForSeconds(18f);
        LightFlashing();
        _circleCollider2.enabled = true;
        NextAttackScheme();
    }

    IEnumerator WaitBeforLunchingSixthAttack()
    {
        yield return new WaitForSeconds(3f);
        SixthAttackScheme();
    }

    IEnumerator TextCountDown()
    {
        int countDown = 10;
        yield return new WaitForSeconds(8f);
        _countDown.gameObject.SetActive(true);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 9;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 8;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 7;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 6;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 5;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 4;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 3;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 2;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        countDown = 1;
        yield return new WaitForSeconds(1f);
        _countDown.text = "Boss active in : " + countDown;
        yield return new WaitForSeconds(1f);
        _countDown.gameObject.SetActive(false);
        GameManager.instance._player._canShoot = true;
    }

    IEnumerator TimeToWaitForEndingAttackScheme(float time)
    {
        yield return new WaitForSeconds(time);
        StopAllCoroutines();
        _waveNumber = 0;
        NextAttackScheme();
    }

    IEnumerator SpawningBulletFirstAttackScheme()
    {
        yield return new WaitForSeconds(0.6f);

        float circleRadius = 0.5f;
        int offset = Random.Range(-_valeurOffset, _valeurOffset);

        for (int i = 0; i < _numberOfPointsFirstAttack; i++)
        {
            float angle = i * (360f / _numberOfPointsFirstAttack);

            if (angle % 45 == 0 && _waveNumber % 2 == 0)
            {
                angle += offset;

                float x = transform.position.x + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = transform.position.y + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

                Vector3 spawnPosition = new Vector3(x, y, 0f);
                FiringBigBullet(spawnPosition);
            }
            else
            {
                angle += offset;

                float x = transform.position.x + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = transform.position.y + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

                Vector3 spawnPosition = new Vector3(x, y, 0f);

                FiringLittleBullet(spawnPosition);
            }
        }

        _waveNumber++;
        StartCoroutine(SpawningBulletFirstAttackScheme());
    }

    IEnumerator SpawningBulletSecondAttackScheme(int offset)
    {
        float circleRadius = 0.5f;

        offset++;

        for (int i = 0; i < _numberOfPointsSecondAttack; i++)
        {
            float angle = i * (360f / _numberOfPointsSecondAttack);
          
            angle += offset;

            float x = transform.position.x + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = transform.position.y + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector3 spawnPosition = new Vector3(x, y, 0f);

            FiringLittleBullet(spawnPosition);
        }

        _waveNumber++;

        yield return new WaitForSeconds(0.2f);

        if (_waveNumber > 15)
        {
            StopAllCoroutines();
            SecondAttackScheme();
            _waveNumber = 0;
        }
        else
        {
            StartCoroutine(SpawningBulletSecondAttackScheme(offset));
        }
    }

    IEnumerator SpawningBulletThirdAttackScheme()
    {
        float circleRadius = 0.5f;

        for (int i = 0; i < _numberOfPointsThirdAttack; i++)
        {
            float angle = i * (360f / _numberOfPointsThirdAttack);

            float x = transform.position.x + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = transform.position.y + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector3 spawnPosition = new Vector3(x, y, -1f);

            CreatAttack(new Vector3(transform.position.x, transform.position.y, 0));

            List<Vector3> _tempList = TurnAttack(_thirdAttackBulletSpawn, angle + 90, new Vector3(transform.position.x, transform.position.y, 0));

            _thirdAttackBulletSpawn.Clear();

            foreach (var _pos in _tempList)
            {
                FiringRetardementBullet(_pos, 1f);
            }
        }

        yield return new WaitForSeconds(2.5f);

        StopAllCoroutines();
        NextAttackScheme();
    }

    IEnumerator SpawningBulletFourthAttackScheme(int offset)
    {
        float circleRadius = 0.5f;

        offset += 9;

        for (int i = 0; i < _numberOfPointsFourthAttack; i++)
        {
            float angle = i * (360f / _numberOfPointsFourthAttack);

            angle += offset;

            float x = transform.position.x + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = transform.position.y + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector3 spawnPosition = new Vector3(x, y, 0f);

            FiringBigBullet(spawnPosition);
        }

        _waveNumber++;

        yield return new WaitForSeconds(0.3f);

        if (_waveNumber > 50)
        {
            yield return new WaitForSeconds(2f);
            StopAllCoroutines();
            NextAttackScheme();
            _waveNumber = 0;
        }
        else
        {
            StartCoroutine(SpawningBulletFourthAttackScheme(offset));
        }
    }

    IEnumerator SpawningBulletFifthAttackScheme(int fragmentingNbr, float distanceToParkour, List<FragmentingBullet> bullet, int duplicationToDO, float distanceSpanwBoss)
    {
        for (int i = 0; i < fragmentingNbr; i++)
        {
            float angle = i * (360f / fragmentingNbr);

            float x = transform.position.x + distanceToParkour * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = transform.position.y + distanceToParkour * Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector3 targetPosition = new Vector3(x, y, 0f);

            FiringFragmentingBullet(fragmentingNbr, duplicationToDO, bullet, transform.position, targetPosition, distanceToParkour);
        }

        yield return new WaitForSeconds(18.0f);
        StopAllCoroutines();
        NextAttackScheme();
    }

    IEnumerator SpawningBulletSixthAttackScheme(float timeToReachPlayer)
    {
        Vector2 playerPos = GameManager.instance._player.transform.position;

        FiringRetardementBullet(playerPos, timeToReachPlayer);

        _waveNumber++;

        yield return new WaitForSeconds(0.5f);

        if (_waveNumber > _numberOfWaveSixthAttack)
        {
            yield return new WaitForSeconds(1.5f);
            StopAllCoroutines();
            _waveNumber = 0;
            NextAttackScheme();
        }
        else
        {
            StartCoroutine(SpawningBulletSixthAttackScheme(timeToReachPlayer));
        }
    }

    public void CreatAttack(Vector3 spawnPoint)
    {
        _thirdAttackBulletSpawn.Add(spawnPoint);
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x, spawnPoint.y - 1f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 0.5f + 0.125f, spawnPoint.y - 1.75f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 1.25f, spawnPoint.y - 2.5f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 1.5f + 0.375f, spawnPoint.y - 3.25f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 2.5f, spawnPoint.y - 4.5f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x, spawnPoint.y - 2.25f - 1.75f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x, spawnPoint.y - 3.25f - 2.25f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x, spawnPoint.y - 7f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x, spawnPoint.y - 4.75f - 3.25f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 1f, spawnPoint.y - 7f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 1.3f, spawnPoint.y - 5f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 2f, spawnPoint.y - 5.75f, spawnPoint.z));
        _thirdAttackBulletSpawn.Add(new Vector3(spawnPoint.x + 1f, spawnPoint.y - 3.6f, spawnPoint.z));

        List<Vector3> _tempList = new List<Vector3>();

        for (int i = 1; i < _thirdAttackBulletSpawn.Count; i++)
        {
            if (_thirdAttackBulletSpawn[0].x != _thirdAttackBulletSpawn[i].x)
            {
                _tempList.Add(new Vector3(_thirdAttackBulletSpawn[0].x - (_thirdAttackBulletSpawn[i].x - _thirdAttackBulletSpawn[0].x), _thirdAttackBulletSpawn[i].y, _thirdAttackBulletSpawn[i].z));
            }
        }

        foreach (var pos in _tempList)
        {
            _thirdAttackBulletSpawn.Add(pos);
        }
    }

    public List<Vector3> TurnAttack(List<Vector3> points, float angle, Vector3 origin)
    {
        List<Vector3> nouveauxPoints = new List<Vector3>();

        foreach (var point in points)
        {
            // Translate the point to the origin
            float xTranslated = point.x - origin.x;
            float yTranslated = point.y - origin.y;

            float angleRad = Mathf.Deg2Rad * angle;

            // Rotation matrix
            float xNouveau = Mathf.Cos(angleRad) * xTranslated - Mathf.Sin(angleRad) * yTranslated;
            float yNouveau = Mathf.Sin(angleRad) * xTranslated + Mathf.Cos(angleRad) * yTranslated;

            // Translate the point back to its original position
            xNouveau += origin.x;
            yNouveau += origin.y;

            nouveauxPoints.Add(new Vector3(xNouveau, yNouveau, -1));
        }

        return nouveauxPoints;
    }

    public void FiringLittleBullet(Vector3 lunchingPos)
    {
        foreach (var littleBullet in _littleBulletList)
        {
            if (!littleBullet._isLunched)
            {
                littleBullet.LunchingBullet(new Vector3(lunchingPos.x - transform.position.x, lunchingPos.y - transform.position.y, -1), lunchingPos);
                break;
            }
        }
    }

    public void FiringBigBullet(Vector3 lunchingPos)
    {
        foreach (var bigBullet in _bigBulletList)
        {
            if (!bigBullet._isLunched)
            {
                bigBullet.LunchingBullet(new Vector2(lunchingPos.x - transform.position.x, lunchingPos.y - transform.position.y), lunchingPos);
                break;
            }
        }
    }

    public void FiringRetardementBullet(Vector3 targetPos, float timeToReachTarget)
    {
        foreach (var retardementBullet in _retardementBulletList)
        {
            if (!retardementBullet._isLunched && retardementBullet._canBeLunchedAgain)
            {
                retardementBullet.Lunch(targetPos, transform.position, timeToReachTarget);
                break;
            }
        }
    }

    public void FiringFragmentingBullet(int fragmentingNbr, int duplicationToDO, List<FragmentingBullet> bullet, Vector3 spanwPos, Vector3 targetPosition,  float distanceToParkour)
    {
        foreach (var oneBullet in bullet)
        {
            if (!oneBullet._isLunched)
            {
                oneBullet.Lunch(fragmentingNbr, duplicationToDO, bullet, spanwPos, targetPosition, distanceToParkour);
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            _lifePoint--;
            PlayerPrefs.SetInt("BossHealth", _lifePoint);

            _modulingBonus++;
            if (_modulingBonus == _numberHpToDecreasToSpawnBonus)
            {
                _modulingBonus = 0;
                int random = Random.Range(0, _listBonus.Count - 1);
                Instantiate(_listBonus[random], transform.position, Quaternion.identity);
            }
        }

        if (_lifePoint == 0)
        {
            GameManager.instance.GameEnd();
        }
    }
}
