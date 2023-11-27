using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public List<BulletAndNumberToSpawn> _listAllBullet;
	public Player _player;
	public GameObject _spawnerBoss;
	public List<GameObject> _bossList;
	public GameObject _actualBossSpawned;
	public string _endScene;
	public Slider _hpSlider;

    private int _countToSpawn;

	[System.Serializable]
	public struct BulletAndNumberToSpawn
	{
		public int _nbrToSpawn;
		public GameObject _bulletPrefab;
	}

    private void Start()
    {
		instance = this;
		_countToSpawn = 0;

        foreach (var bullet in _listAllBullet)
        {
			for (int i = 0; i < bullet._nbrToSpawn; i++)
			{
				Instantiate(bullet._bulletPrefab, new Vector3(_countToSpawn * 3, 100, 0), Quaternion.identity);
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
    }

    public void SpawningNextBoss(int nbrOfBossKilled)
	{
        _actualBossSpawned = Instantiate(_bossList[nbrOfBossKilled], _spawnerBoss.transform.position, Quaternion.identity);
	}

	public void GameEnd()
	{
        SceneManager.LoadScene(_endScene);
    }
}
