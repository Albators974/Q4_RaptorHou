using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string _mainMenu;
    public string _gameScene;
    public TextMeshProUGUI _scoreText;
    public TextMeshProUGUI _highScoreText;
    public Slider _bossSlider;
    public Canvas _difficultyCanvas;
    public int _difficultyEasyHp;
    public int _difficultyMediumHp;
    public int _difficultyHardHp;
    public int _difficultyImpossibleHp;
    public EventSystem _eventSystem;
    public GameObject _startButton;
    public GameObject _difficultyEasyButton;

    private void Start()
    {
        if (_scoreText != null)
        {
            _scoreText.text = "Score : " + PlayerPrefs.GetInt("Score");            
        }

        if (_bossSlider != null)
        {
            _bossSlider.maxValue = PlayerPrefs.GetInt("BossMaxHp");
            _bossSlider.value = PlayerPrefs.GetInt("BossHealth");
        }

        if (Gamepad.current != null)
        {
            Gamepad.current.ResetHaptics();
        }

        if (_difficultyCanvas != null)
        {
            _difficultyCanvas.enabled = false;
        }

        Cursor.visible = true;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            PlayerPrefs.SetInt("BossMaxHp", _difficultyMediumHp);
        }

        _highScoreText.text = "Hight score : " + PlayerPrefs.GetInt("HighScore");
    }
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void ChangeDifficulty()
    {
        _difficultyCanvas.enabled = true;
        _eventSystem.SetSelectedGameObject(_difficultyEasyButton);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Easy()
    {
        PlayerPrefs.SetInt("BossMaxHp", _difficultyEasyHp);
        PlayerPrefs.SetInt("Impossible", 0);
        _difficultyCanvas.enabled = false;
        _eventSystem.SetSelectedGameObject(_startButton);
    }

    public void Medium()
    {
        PlayerPrefs.SetInt("BossMaxHp", _difficultyMediumHp);
        PlayerPrefs.SetInt("Impossible", 0);
        _difficultyCanvas.enabled = false;
        _eventSystem.SetSelectedGameObject(_startButton);
    }

    public void Hard()
    {
        PlayerPrefs.SetInt("BossMaxHp", _difficultyHardHp);
        PlayerPrefs.SetInt("Impossible", 0);
        _difficultyCanvas.enabled = false;
        _eventSystem.SetSelectedGameObject(_startButton);
    }

    public void Impossible()
    {
        PlayerPrefs.SetInt("BossMaxHp", _difficultyImpossibleHp);
        PlayerPrefs.SetInt("Impossible", 1);
        _difficultyCanvas.enabled = false;
        _eventSystem.SetSelectedGameObject(_startButton);
    }

    public void QuitDifficulty()
    {
        _difficultyCanvas.enabled = false;
        _eventSystem.SetSelectedGameObject(_startButton);
    }

    public void StartingGame()
    {
        SceneManager.LoadScene(_gameScene);
    }
}
