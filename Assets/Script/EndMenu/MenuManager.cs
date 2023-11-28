using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string _mainMenu;
    public string _gameScene;
    public TextMeshProUGUI _scoreText;
    public TextMeshProUGUI _highScoreText;
    public Slider _bossSlider;

    private void Start()
    {
        if (_scoreText != null)
        {
            _scoreText.text = "Score : " + PlayerPrefs.GetInt("Score");            
        }

        if (_bossSlider != null)
        {
            _bossSlider.value = PlayerPrefs.GetInt("BossHealth");
        }

        _highScoreText.text = "Hight score : " + PlayerPrefs.GetInt("HighScore");
    }
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartingGame()
    {
        SceneManager.LoadScene(_gameScene);
    }
}
