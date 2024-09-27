using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public List<BulletAndNumberToSpawn> _bullets;
	public Player _player;
	public GameObject _spawnerBoss;
	public List<GameObject> _bosses;
	public GameObject _actualBossSpawned;
	public string _endScene;
    public float _rotationSpeedSkyBox = 1.0f;
    public Slider _hpSlider;
    public AudioSource _audioSourceSpawn;
    public AudioSource _audioSourceInGame;
    public TextMeshProUGUI _bossCountDown;

    private int _countToSpawn;

	[System.Serializable]
	public struct BulletAndNumberToSpawn
	{
		public int _nbrToSpawn;
		public GameObject _bulletPrefab;
        public BulletType _bulletType;
	}

    public enum BulletType
    {
        LittleBullet,
        BigBullet,
        FragmentingBullet,
        RetardementBullet,
        PlayerBullet,
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
		_countToSpawn = 0;

        _audioSourceSpawn.Play();

        _bossCountDown.gameObject.SetActive(false);

        foreach (var bullet in _bullets)
        {
			for (int i = 0; i < bullet._nbrToSpawn; i++)
			{
				GameObject bulletInstantiated = Instantiate(bullet._bulletPrefab, new Vector3(_countToSpawn * 3, 100, 0), Quaternion.identity);

                switch (bullet._bulletType)
                {
                    case BulletType.LittleBullet:
                        Bullet littleBullet = bulletInstantiated.GetComponent<Bullet>();
                        littleBullet._decelerate = true;
                        BossManager.instance._littleBullet.Add(littleBullet);
                        break;
                    case BulletType.BigBullet:
                        BossManager.instance._bigBullet.Add(bulletInstantiated.GetComponent<Bullet>());
                        break;
                    case BulletType.FragmentingBullet:
                        BossManager.instance._fragmentingBulletList.Add(bulletInstantiated.GetComponent<FragmentingBullet>());
                        break;
                    case BulletType.RetardementBullet:
                        BossManager.instance._retardementBulletList.Add(bulletInstantiated.GetComponent<RetardementBullet>());
                        break;
                    default:
                        break;
                }

                _countToSpawn++;
			}
        }

		SpawningNextBoss(_player._nbrBossKilled);
    }

    private void Update()
    {
        if (_actualBossSpawned != null && _player._nbrBossKilled == 0 && _actualBossSpawned.GetComponent<FirstBoss>()._lifePoint < 0)
        {
			SceneManager.LoadScene(_endScene);
        }
        else if (_actualBossSpawned != null && _player._nbrBossKilled == 0 && _actualBossSpawned.GetComponent<FirstBoss>()._lifePoint >= 0)
        {
			_hpSlider.value = _actualBossSpawned.GetComponent<FirstBoss>()._lifePoint;
        }

        if (!_audioSourceSpawn.isPlaying && !_audioSourceInGame.isPlaying)
        {
            _audioSourceInGame.Play();
        }

        Material skyboxMaterial = RenderSettings.skybox;
        float rotation = Time.time * _rotationSpeedSkyBox;
        skyboxMaterial.SetFloat("_Rotation", rotation);
    }

    public void SpawningNextBoss(int nbrOfBossKilled)
	{
        _actualBossSpawned = Instantiate(_bosses[nbrOfBossKilled], _spawnerBoss.transform.position, Quaternion.identity);
	}

	public void GameEnd()
	{
		PlayerPrefs.SetInt("Score", _player._score);
        if (PlayerPrefs.GetInt("HighScore") <= _player._score)
        {
			PlayerPrefs.SetInt("HighScore", _player._score);
        }

        SceneManager.LoadScene(_endScene);
    }
}
