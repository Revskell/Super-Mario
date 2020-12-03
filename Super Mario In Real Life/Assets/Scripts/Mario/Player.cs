using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private int Health;
    private int Coins;
    private int Lives;
    private bool dead;
    private bool paused;
    private Animator UIAnimator = null;
    private float HealthCounter;
    public bool currentCursor;

    [Header("UI")]
    [SerializeField] private Image HealthInterface = null;
    [SerializeField] private List<Sprite> HealthStates = null;
    [SerializeField] private Image CoinLeftDigit = null;
    [SerializeField] private Image CoinRightDigit = null;
    [SerializeField] private Image LifeLeftDigit = null;
    [SerializeField] private Image LifeRightDigit = null;
    [SerializeField] private List<Sprite> Digits = null;
    [SerializeField] private GameObject GameOverScreen = null;
    [SerializeField] private GameObject PauseScreen = null;
    [SerializeField] private List<GameObject> Cursors = null;

    [Header("Sounds")]
    [SerializeField] private AudioClip OneUpSound = null;
    [SerializeField] private AudioClip PickUpCoin = null;
    [SerializeField] private AudioClip HealSound = null;
    [SerializeField] private AudioClip DieSound = null;
    [SerializeField] private AudioClip HurtSound = null;
    [SerializeField] private AudioClip GameOverSound = null;
    [SerializeField] private AudioClip RespawnSound = null;
    [SerializeField] private AudioClip PauseSound = null;
    [SerializeField] private AudioClip SelectSound = null;

    public static Player instance = null;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance.gameObject);
        instance = this;
    }

    void Start()
    {
        Health = 8;
        HealthInterface.sprite = HealthStates[8];
        Coins = 0;
        Lives = 3;
        dead = false;
        UIAnimator = GameOverScreen.GetComponent<Animator>();
        HealthCounter = 0f;
        paused = false;
        currentCursor = false;
    }
    
    void Update()
    {
        if (!paused)
        {
            if (!dead)
            {
                if (HealthCounter > 0f) HealthCounter -= Time.deltaTime;
                else HealthInterface.enabled = false;
                // provisional
                if (Input.GetKeyDown(KeyCode.DownArrow)) TakeDamage(1);
                else if (Input.GetKeyDown(KeyCode.UpArrow)) AddHealth(1);
                else if (Input.GetKeyDown(KeyCode.Plus)) AddCoins(1);
                else if (Input.GetKeyDown(KeyCode.Minus)) AddCoins(10);
                else if (Input.GetKeyDown(KeyCode.Escape)) PauseGame(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.E))
            {
                if (!currentCursor) PauseGame(false);
                else Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) UpdateCursor();
        }
    }

    public void Die()
    {
        if(!dead)
        {
            dead = true;
            AudioSource.PlayClipAtPoint(GameOverSound, transform.position);
            GameOverScreen.gameObject.SetActive(true);
            UIAnimator.SetTrigger("Game Over");
            Invoke("EndGameOver", 2.3f);
        }
    }

    private void Respawn(Vector3 newPos)
    {
        transform.GetChild(0).position = newPos;
        transform.parent = null;
        dead = false;
        AddHealth(8);
    }

    public void AddHealth(int amount)
    {
        if (Health + amount <= 8) Health += amount;
        else Health = 8;
        AudioSource.PlayClipAtPoint(HealSound, transform.position);
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        HealthInterface.enabled = true;
        HealthInterface.sprite = HealthStates[Health];
        HealthCounter = 2f;
    }

    public void TakeDamage(int amount)
    {
        if (Health - amount >= 0) Health -= amount;
        else Health = 0;
        AudioSource.PlayClipAtPoint(HurtSound, transform.position);
        UpdateHealth();
        if (Health <= 0) Die();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        if (Coins > 99)
        {
            Coins = Coins%99;
            AddOneUp();
        }
        UpdateCoins();
        AudioSource.PlayClipAtPoint(PickUpCoin, transform.position);
    }

    public void AddOneUp()
    {
        AudioSource.PlayClipAtPoint(OneUpSound, transform.position);
        Lives++;
        if (Lives > 99) Lives = 99;
        UpdateLives();
    }

    private void UpdateCoins()
    {
        CoinLeftDigit.sprite = Digits[Coins / 10];
        CoinRightDigit.sprite = Digits[Coins%10];
    }

    private void UpdateLives()
    {
        LifeLeftDigit.sprite = Digits[Lives / 10];
        LifeRightDigit.sprite = Digits[Lives % 10];
    }

    public void EndGameOver()
    {
        GameOverScreen.SetActive(false);
        Respawn(Checkpoints.GetLastCheckpoint());
    }

    private void PauseGame(bool pause)
    {
        AudioSource.PlayClipAtPoint(PauseSound, transform.position);
        paused = pause;
        if(pause)
        {
            Time.timeScale = 0f;
            // desactivar character controller
            PauseScreen.SetActive(true);
            Cursors[1].SetActive(false);
            HealthInterface.enabled = true;
            Debug.Log("Paused");
        }
        else
        {
            Time.timeScale = 1f;
            // reactivar
            PauseScreen.SetActive(false);
            HealthInterface.enabled = false;
            Debug.Log("Unpaused");
        }
    }

    private void UpdateCursor()
    {
        AudioSource.PlayClipAtPoint(SelectSound, transform.position);
        currentCursor = !currentCursor;
        if(!currentCursor)
        {
            Cursors[0].SetActive(true);
            Cursors[1].SetActive(false);
        }
        else
        {
            Cursors[0].SetActive(false);
            Cursors[1].SetActive(true);
        }
    }
}
