using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject menuPause, menuWin, menuLose, menuActive;
    public bool isPaused;
    float timeScaleOrigin;
    int gameGoalCount;

    [SerializeField] TMP_Text goalCountText;
    public GameObject player;
    public PlayerController playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrigin = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(menuActive == null)
            {
                StatePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                StateUnpause();
            }
        }
    }
    public void StatePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void StateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrigin;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void UpdateGameGoal(int amount)
    {
        gameGoalCount += amount;
        goalCountText.text = gameGoalCount.ToString("F0");
        if (gameGoalCount <= 0)
        {
            StatePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }
    public void YouLose()
    {
        StatePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}
