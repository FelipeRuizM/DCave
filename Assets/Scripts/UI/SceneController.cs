using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private BossStats bossStats;
    [SerializeField] private UIController ui;
    
    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    private void OnPlayerDead()
    {
        ui.ShowGameOverPopup();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDifficultyChanged(int newDifficulty)
    {
        Debug.Log("Scene.OnDifficultyChanged(" + newDifficulty + ")");
        bossStats.maxHealth = bossStats.maxHealth + newDifficulty * 10;
    }

    public int GetDifficulty()
    {
        return PlayerPrefs.GetInt("difficulty", 1);
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
