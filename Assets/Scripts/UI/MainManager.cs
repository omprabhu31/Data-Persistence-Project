using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;

    private bool m_Started = false;
    private int m_Points;

    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] bool m_GamePaused = false;
    [SerializeField] GameObject GameOverCanvas;
    public static bool m_GameOver = false;

    private void Awake()
    {
        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        UpdateInstanceDetails();
    }

    void UpdateInstanceDetails()
    {
        if (GameManager.Instance != null)
        {
            GameObject.Find("ScoreText (1)").GetComponent<Text>().text = GameManager.Instance.highScorePlayer + " has the high score: " + GameManager.Instance.highScore;
            GameManager.Instance.SaveDetailsToFile();
        }
        else
        {
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.25f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenu();

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    void PauseMenu()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) && !m_GameOver)
        {
            PauseToggle();
        }
    }

    public void PauseToggle()
    {
        mainCanvas.SetActive(m_GamePaused);
        pauseCanvas.SetActive(!m_GamePaused);

        if (!m_GamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        m_GamePaused = !m_GamePaused;
    }

    public void LoadMenu()
    {
        m_GameOver = false;
        m_GamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    public void LoadMain()
    {
        m_GameOver = false;
        m_GamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        if (m_Points > GameManager.Instance.highScore)
        {
            GameManager.Instance.highScore = m_Points;
            GameManager.Instance.highScorePlayer = GameManager.Instance.playerName;
        }
        m_GameOver = true;
        GameOverCanvas.SetActive(true);
        GameManager.Instance.SaveDetailsToFile();
    }
}
