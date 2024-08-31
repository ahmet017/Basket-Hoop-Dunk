using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityUtils;
using UnityUtils.AdmobAdManager;
using UnityUtils.UI;

public class GameManager : MonoBehaviour
{
    public static bool canDraw;
    public static bool paused;
    public static int BestScore
    {
        get
        {
            return PlayerPrefs.GetInt("bestScore", 0);
        }
        set
        {
            PlayerPrefs.SetInt("bestScore", value);
            PlayerPrefs.Save();
        }
    }
    private bool RandomBool
    {
        get
        {
            return Random.Range(0f, 1f) < 0.5f;
        }
    }
    public static int GamesPlayed
    {
        get
        {
            return PlayerPrefs.GetInt("GamesPlayed", 0);
        }
        set
        {
            PlayerPrefs.SetInt("GamesPlayed", value);
            PlayerPrefs.Save();
        }
    }

    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Hoop hoopPrefab;
    [SerializeField] private Bomb bombPrefab;
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private DrawingCounter drawingCounter;
    [SerializeField] private GameObject dunkParticlesPrefab;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private ReviveScreen reviveScreen;
    [SerializeField] private AddedScoreText addedScorePrefab;
    [SerializeField] private ScoreAnim scoreAnim;

    private AudioManager audioManager;
    private Vector2 screenBounds;
    private Vector2 ballSpawnPos;
    private Vector2 hoopSpawnPos;
    private Vector2 bombSpawnPos;
    private Vector2 velocity;
    private Vector2 velocity2;
    private int score = 0;
    private List<Ball> balls = new List<Ball>();
    private List<Hoop> hoops = new List<Hoop>();
    private List<Bomb> bombs = new List<Bomb>();
    private int numberOfBalls;
    private int progress;
    private Coroutine checkIfGameOver;
    private GameObject dunkParticles;
    private int rand;
    private bool isRevived;
    private int _bestScore;
    private bool gameOver;
    private float hoopYLowerLimit, hoopYUpperLimit;
    private AddedScoreText addedScoreText;
    private int minScoreForBomb = 14;

    void Start()
    {
        Application.targetFrameRate = 120;
        Time.timeScale = 1;
        audioManager = FindObjectOfType<AudioManager>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        ballSpawnPos.y = -screenBounds.y - 1f;
        bombSpawnPos.y = -screenBounds.y - 1f;
        canDraw = false;
        isRevived = false;
        velocity = new Vector2(0, 15);
        velocity2 = new Vector2(10, 16);
        hoopYLowerLimit = -screenBounds.y * 0.5f;
        hoopYUpperLimit = screenBounds.y * 0.2f;
        Physics2D.IgnoreLayerCollision(6, 6);
        AddGameOverTrigger();

        drawingCounter.onDrawingsOver.AddListener(() =>
        {
            canDraw = false;
            checkIfGameOver = StartCoroutine(CheckIfGameOver());
        });
    }


    #region Public methods
    public void StartGame()
    {
        _bestScore = BestScore;
        StartCoroutine(NewRound());
    }
    public void Dunk(Vector2 hoopPos, bool touchedToHoop)
    {
        audioManager.Dunk();
        dunkParticles = Instantiate(dunkParticlesPrefab);
        dunkParticles.transform.position = hoopPos;
        dunkParticles.transform.localScale = Vector3.one;

        progress++;
        score += touchedToHoop ? 1 : 2;
        UpdateScoreUI();

        addedScoreText = Instantiate(addedScorePrefab);
        addedScoreText.transform.position = hoopPos;
        addedScoreText.transform.localScale = Vector3.one;
        addedScoreText.Score = touchedToHoop ? 1 : 2;


        if (progress == numberOfBalls)
        {
            progress = 0;
            numberOfBalls = 0;
            canDraw = false;
            StartCoroutine(NewRound());
        }
    }
    public void GameOver()
    {
        if (checkIfGameOver != null)
        {
            StopCoroutine(checkIfGameOver);
            checkIfGameOver = null;
        }

        if (!isRevived && score > 2 && AdmobAdManager.Instance.IsRewardedAdReady())
        {
            Time.timeScale = 0;
            canDraw = false;
            reviveScreen.Show((reviveClicked) =>
            {
                if (reviveClicked)
                {
                    AdmobAdManager.Instance.ShowRewardedAdIfLoaded((rewarded) =>
                    {
                        UnityMainThreadDispatcher.Instance.Enqueue(() =>
                        {
                            if (rewarded)
                                Revive();
                            else
                            {
                                Time.timeScale = 1;
                                isRevived = true;
                                GameOver();
                            }
                        });
                    });
                }
                else
                {
                    Time.timeScale = 1;
                    isRevived = true;
                    GameOver();
                }
            });
            return;
        }
        
        RemoveObjects();
        canDraw = false;
        CheckIfBestScoreChanged();
        gameOverScreen.Score = score;
        gameOverScreen.BestScore = _bestScore;
        gameOverScreen.Show();
    }
    public void Restart()
    {
        isRevived = true;
        GameOver();
    }
    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
    }
    public void Revive()
    {
        Time.timeScale = 1;
        isRevived = true;
        progress = 0;
        numberOfBalls = 0;
        canDraw = false;
        StartCoroutine(NewRound());
    }
    #endregion




    #region Private methods
    private IEnumerator CheckIfGameOver()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            gameOver = true;
            foreach (Ball ball in balls)
            {
                if (ball == null) continue;
                if (ball.Speed > 0.001f)
                {
                    gameOver = false;
                }
            }

            if (gameOver)
            {
                GameOver();
                break;
            }
        }
    }
    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
        scoreAnim.ScoreUpdated();
    }
    private void CheckIfBestScoreChanged()
    {
        if (score > _bestScore)
        {
            BestScore = score;
            _bestScore = score;
        }
    }
    private void AddGameOverTrigger()
    {
        GameObject o = new GameObject("GameOverTrigger");
        BoxCollider2D boxCollider = o.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(screenBounds.x * 4, 1);
        boxCollider.isTrigger = true;
        o.transform.position = new Vector2(0, -screenBounds.y - 2f);
        o.tag = "GameOver";
    }
    #endregion




    #region Object spawn
    private IEnumerator NewRound()
    {
        if (checkIfGameOver != null)
        {
            StopCoroutine(checkIfGameOver);
            checkIfGameOver = null;
        }

        CheckIfBestScoreChanged();
        RemoveObjects();

        yield return new WaitForSeconds(0.5f - 0.5f * Mathf.Clamp01(score * 0.01f));
        canDraw = true;

        // Spawn objects
        rand = Random.Range(0, Mathf.Clamp(Mathf.FloorToInt(1 + score * 0.5f), 0, 8));
        switch (rand)
        {
            case 0:
                drawingCounter.BuildIcons(3);
                yield return Spawn1();
                break;
            case 1:
                drawingCounter.BuildIcons(3);
                yield return Spawn2();
                break;
            case 2:
                drawingCounter.BuildIcons(3);
                yield return Spawn3();
                break;
            case 3:
                drawingCounter.BuildIcons(3);
                yield return Spawn4();
                break;
            case 4:
                drawingCounter.BuildIcons(3);
                yield return Spawn5(3);
                break;
            case 5:
                drawingCounter.BuildIcons(3);
                yield return Spawn6(3);
                break;
            case 6:
                drawingCounter.BuildIcons(4);
                yield return Spawn7();
                break;
            case 7:
                drawingCounter.BuildIcons(3);
                yield return Spawn8();
                break;
        }
    }
    private IEnumerator Spawn1()
    {
        numberOfBalls = 1;

        audioManager.Whoosh();
        ballSpawnPos.x = RandomBool ? Random.Range(-screenBounds.x + Ball.ballSize, -Ball.ballSize) : Random.Range(Ball.ballSize, screenBounds.x - Ball.ballSize);
        SpawnBall(ballSpawnPos, velocity);
        
        yield return new WaitForSeconds(0.1f);

        hoopSpawnPos.y = Random.Range(hoopYLowerLimit, hoopYUpperLimit);
        hoopSpawnPos.x = ballSpawnPos.x < 0 ? Random.Range(Hoop.hoopWidth * 0.5f, screenBounds.x - Hoop.hoopWidth) :
            Random.Range(-screenBounds.x + Hoop.hoopWidth, -Hoop.hoopWidth * 0.5f);
        SpawnHoop(hoopSpawnPos);

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = ballSpawnPos.x < 0 ? Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize) : 
                Random.Range(Bomb.bombSize, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }
    private IEnumerator Spawn2()
    {
        numberOfBalls = 1;

        audioManager.Whoosh();
        ballSpawnPos.x = Random.Range(-screenBounds.x + Ball.ballSize, 0);
        velocity2.x *= RandomBool ? 1 : -1;
        SpawnBall(ballSpawnPos, velocity2);

        yield return new WaitForSeconds(0.65f);

        hoopSpawnPos.y = Random.Range(hoopYLowerLimit, hoopYUpperLimit);
        hoopSpawnPos.x = velocity2.x < 0 ? 
            Random.Range(Hoop.hoopWidth * 0.5f, screenBounds.x - Hoop.hoopWidth) : Random.Range(-screenBounds.x + Hoop.hoopWidth, -Hoop.hoopWidth * 0.5f);
        SpawnHoop(hoopSpawnPos);

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = hoopSpawnPos.x > 0 ? Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize) :
                Random.Range(Bomb.bombSize, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }
    private IEnumerator Spawn3()
    {
        numberOfBalls = 2;

        audioManager.Whoosh();
        ballSpawnPos.x = RandomBool ? Random.Range(0, screenBounds.x - Ball.ballSize) : Random.Range(-screenBounds.x + Ball.ballSize, 0);
        SpawnBall(ballSpawnPos, velocity);

        hoopSpawnPos.y = Random.Range(0, hoopYUpperLimit);
        hoopSpawnPos.x = ballSpawnPos.x > 0 ? Random.Range(-screenBounds.x + Hoop.hoopWidth, -Hoop.hoopWidth) :
            Random.Range(Hoop.hoopWidth, screenBounds.x - Hoop.hoopWidth);
        SpawnHoop(hoopSpawnPos);

        yield return new WaitForSeconds(0.5f);

        audioManager.Whoosh();
        ballSpawnPos.x = ballSpawnPos.x > 0 ? Random.Range(0, screenBounds.x - Ball.ballSize) : Random.Range(-screenBounds.x + Ball.ballSize, 0);
        SpawnBall(ballSpawnPos, velocity);

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = hoopSpawnPos.x > 0 ? Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize) :
                Random.Range(Bomb.bombSize, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }
    private IEnumerator Spawn4()
    {
        numberOfBalls = 2;

        audioManager.Whoosh();
        ballSpawnPos.x = 0;
        velocity2.x = Mathf.Abs(velocity2.x);
        SpawnBall(ballSpawnPos, velocity2);

        audioManager.Whoosh();
        ballSpawnPos.x = 0;
        velocity2.x = -Mathf.Abs(velocity2.x);
        SpawnBall(ballSpawnPos, velocity2);

        yield return new WaitForSeconds(0.65f);

        hoopSpawnPos.y = Random.Range(hoopYLowerLimit, 0);
        hoopSpawnPos.x = Random.Range(-screenBounds.x * 0.5f, screenBounds.x * 0.5f);
        SpawnHoop(hoopSpawnPos);

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = hoopSpawnPos.x > 0 ? Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize * 2) :
                Random.Range(Bomb.bombSize * 2, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }
    private IEnumerator Spawn5(int ballsCount)
    {
        numberOfBalls = ballsCount;

        ballSpawnPos.x = Random.Range(-screenBounds.x + Ball.ballSize * ballsCount, screenBounds.x - Ball.ballSize * ballsCount);
        for (int i = 0; i < ballsCount; i++)
        {
            audioManager.Whoosh();
            ballSpawnPos.x = ballSpawnPos.x + Ball.ballSize * i * 0.5f;
            SpawnBall(ballSpawnPos, velocity);
            yield return new WaitForSeconds(0.1f);
        }

        hoopSpawnPos.y = Random.Range(hoopYLowerLimit, hoopYUpperLimit);
        hoopSpawnPos.x = (ballSpawnPos.x - Ball.ballSize * (ballsCount - 1) * 0.5f) < 0 ?
            Random.Range(Hoop.hoopWidth * 2, screenBounds.x - Hoop.hoopWidth) : Random.Range(-screenBounds.x + Hoop.hoopWidth, -Hoop.hoopWidth * 2);
        SpawnHoop(hoopSpawnPos);

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = hoopSpawnPos.x > 0 ? Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize) :
                Random.Range(Bomb.bombSize, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }
    private IEnumerator Spawn6(int ballsCount)
    {
        numberOfBalls = ballsCount;

        ballSpawnPos.x = Random.Range(0, screenBounds.x - Ball.ballSize * ballsCount);
        velocity2.x = -Mathf.Abs(velocity2.x);
        for (int i = 0; i < ballsCount; i++)
        {
            audioManager.Whoosh();
            ballSpawnPos.x = ballSpawnPos.x + Ball.ballSize * i * 0.5f;
            SpawnBall(ballSpawnPos, velocity2);
            yield return new WaitForSeconds(0.1f);
        }

        hoopSpawnPos.y = Random.Range(hoopYLowerLimit, hoopYUpperLimit);
        hoopSpawnPos.x = Random.Range(Hoop.hoopWidth, screenBounds.x - Hoop.hoopWidth * 0.5f);
        SpawnHoop(hoopSpawnPos);

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }
    private IEnumerator Spawn7()
    {
        numberOfBalls = 2;

        audioManager.Whoosh();
        ballSpawnPos.x = Random.Range(-screenBounds.x + Ball.ballSize, -(Ball.ballSize + Hoop.hoopWidth));
        SpawnBall(ballSpawnPos, velocity);

        audioManager.Whoosh();
        ballSpawnPos.x = Random.Range(Ball.ballSize + Hoop.hoopWidth, screenBounds.x - Ball.ballSize);
        SpawnBall(ballSpawnPos, velocity);

        hoopSpawnPos.y = Random.Range(hoopYLowerLimit, - Hoop.hoopWidth);
        hoopSpawnPos.x = -Hoop.hoopWidth * 0.5f;
        SpawnHoop(hoopSpawnPos, Quaternion.Euler(0, 0, 45));

        hoopSpawnPos.y = Random.Range(Hoop.hoopWidth, hoopYUpperLimit);
        hoopSpawnPos.x = Hoop.hoopWidth * 0.5f;
        SpawnHoop(hoopSpawnPos, Quaternion.Euler(0, 0, -45));

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = RandomBool ? Random.Range(-screenBounds.x + Bomb.bombSize, -Hoop.hoopWidth) :
                Random.Range(Hoop.hoopWidth, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }

        yield return null;
    }
    private IEnumerator Spawn8()
    {
        numberOfBalls = 2;

        audioManager.Whoosh();
        ballSpawnPos.x = 0;
        velocity2.x = Mathf.Abs(velocity2.x);
        SpawnBall(ballSpawnPos, velocity2);

        audioManager.Whoosh();
        ballSpawnPos.x = 0;
        velocity2.x = -Mathf.Abs(velocity2.x);
        SpawnBall(ballSpawnPos, velocity2);
        
        yield return new WaitForSeconds(0.5f);

        if (RandomBool)
        {
            hoopSpawnPos.y = Random.Range(hoopYLowerLimit, -Hoop.hoopWidth);
            hoopSpawnPos.x = -screenBounds.x + Hoop.hoopWidth * 0.5f;
            SpawnHoop(hoopSpawnPos, Quaternion.Euler(0, 0, -45));
        }else
        {
            hoopSpawnPos.y = Random.Range(hoopYLowerLimit, -Hoop.hoopWidth);
            hoopSpawnPos.x = screenBounds.x - Hoop.hoopWidth * 0.5f;
            SpawnHoop(hoopSpawnPos, Quaternion.Euler(0, 0, 45));
        }

        if (score > minScoreForBomb && RandomBool)
        {
            yield return new WaitForSeconds(0.5f);

            audioManager.Whoosh();
            bombSpawnPos.x = hoopSpawnPos.x > 0 ? Random.Range(-screenBounds.x + Bomb.bombSize, -Bomb.bombSize * 2) :
                Random.Range(Bomb.bombSize * 2, screenBounds.x - Bomb.bombSize);
            SpawnBomb(bombSpawnPos, velocity);
        }
    }

    private void SpawnBall(Vector2 pos, Vector2 _velocity)
    {
        Ball ball = Instantiate(ballPrefab);
        ball.transform.position = pos;
        ball.Shoot(_velocity);
        balls.Add(ball);
    }
    private void SpawnHoop(Vector2 pos)
    {
        Hoop hoop = Instantiate(hoopPrefab);
        hoop.spawnPosition = pos;
        hoops.Add(hoop);
    }
    private void SpawnHoop(Vector2 pos, Quaternion rotation)
    {
        Hoop hoop = Instantiate(hoopPrefab);
        hoop.spawnPosition = pos;
        hoop.transform.rotation = rotation;
        hoops.Add(hoop);
    }
    private void SpawnBomb(Vector2 pos, Vector2 _velocity)
    {
        Bomb bomb = Instantiate(bombPrefab);
        bomb.transform.position = pos;
        bomb.Shoot(_velocity);
        bombs.Add(bomb);
    }
    private void RemoveObjects()
    {
        foreach (Ball _ball in balls)
        {
            if (_ball != null)
                Destroy(_ball.gameObject);
        }
        foreach (Hoop _hoop in hoops)
        {
            if (_hoop != null)
                _hoop.DestoryHoop();
        }
        foreach (Bomb _bomb in bombs)
        {
            if (_bomb != null)
                Destroy(_bomb.gameObject);
        }
        balls.Clear();
        hoops.Clear();
        bombs.Clear();
        lineDrawer.DestroyAllLines();
    }
    #endregion

}
