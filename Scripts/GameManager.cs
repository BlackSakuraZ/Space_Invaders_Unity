using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public sealed class GameManager : MonoBehaviour {
    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    public GameObject gameOverUI;
    public Text scoreText;
    public Text livesText;
    public Text menuText;
    public Button restartButton;

    public int score { get; private set; }
    public int lives { get; private set; }

    private void Awake() {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();
    }

    private void Start() {
        player.killed += OnPlayerKilled;
        mysteryShip.killed += OnMysteryShipKilled;
        invaders.killed += OnInvaderKilled;
        NewGame();
    }

    private void Update() {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            NewGame();
        }
    }

    private void NewGame() {
        gameOverUI.SetActive(false);
        menuText.color = Color.black;
        restartButton.GetComponent<Graphic>().color = Color.black;
        restartButton.GetComponent<Button>().enabled = false;
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound() {   
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++) {
            bunkers[i].ResetBunker();
        }
        Respawn();
    }

    private void Respawn() {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver() {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
        menuText.text = "Game Over\n\nYour Score:\n\n" + score.ToString();
        menuText.color = Color.cyan;
	    restartButton.GetComponent<Graphic>().color = Color.cyan;
        restartButton.GetComponent<Button>().enabled = true;
        foreach (var bunker in bunkers) {
            bunker.gameObject.SetActive(false);
        }
    }

    private void SetScore(int score) {
        this.score = score;
        scoreText.text = "SCORE: " + score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives) {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = "LIFE: " + lives.ToString();
    }

    private void OnPlayerKilled() {
        SetLives(lives - 1);
        player.gameObject.SetActive(false);
        if (lives > 0) {
            Respawn();
        } else {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader) {
        SetScore(score + invader.score);
        if (invaders.CounterKilled == invaders.TotalAmount) {
            NewRound();
        }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip) {
        SetScore(score + mysteryShip.score);
    }
}
