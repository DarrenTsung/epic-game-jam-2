using UnityEngine;
using System.Collections;
using DT;
	
public enum GameState { TITLE_SCREEN, TUTORIAL, GAME, WIN, LOSE };

public class GameManager : Singleton<GameManager> {
	protected float BUFFER_LENGTH = 3.0f;
	protected float winLoseInputBuffer;
	
	public void WinGame() {
		winLoseInputBuffer = BUFFER_LENGTH;
		CurrentState = GameState.WIN;
	}
	
	public void LoseGame() {
		winLoseInputBuffer = BUFFER_LENGTH;
		CurrentState = GameState.LOSE;
	}
	
	public void ResetGame() {
		CurrentState = GameState.GAME;
	}
	
	protected GameManager() {}
	
	protected GameState _currentState;
	public GameState CurrentState {
		get { return _currentState; }
		set {
			WillTransitionToState(value);
			_currentState = value;
		}
	}
	
	protected void Start() {
		CurrentState = GameState.TITLE_SCREEN;
	}
	
	protected void Update() {
		winLoseInputBuffer -= Time.deltaTime;
		
		if (Input.GetKeyDown(KeyCode.D)) {
			if (CurrentState == GameState.TITLE_SCREEN) {
				GameObject[] titleScreenObjects = GameObject.FindGameObjectsWithTag("TitleScreen");
				foreach (GameObject titleScreenObject in titleScreenObjects) {
					Animator titleScreenAnimator = titleScreenObject.GetComponent<Animator>();
					
					if (titleScreenAnimator) {
						titleScreenAnimator.SetTrigger("Disappear");
					} else {
						Debug.LogError("Title screen object - no animator");
					}
				}
				StartGame();
			} else if ((CurrentState == GameState.WIN || CurrentState == GameState.LOSE) && winLoseInputBuffer <= 0.0f) {
				ResetGame();
			}
		} 
	}
		
	protected void StartGame() {
		int firstTime = PlayerPrefs.GetInt("FirstTime", 0);
		if (firstTime == 0) {
			// first time flow
			PlayerPrefs.SetInt("FirstTime", 1);
			CurrentState = GameState.TUTORIAL;
		} else {
			CurrentState = GameState.GAME;
		}
	}
	
	protected void WillTransitionToState(GameState newState) {
		switch (newState) {
			case GameState.TUTORIAL:
				GameObject[] tutorialObjects = GameObject.FindGameObjectsWithTag("Tutorial");
				foreach (GameObject tutorialObject in tutorialObjects) {
					tutorialObject.SetActive(true);
				}
				goto case GameState.GAME;
			case GameState.GAME:
				// reset game objects
				IGameStateInterface[] interfaces = transform.GetComponentsInChildren<IGameStateInterface>();
				foreach (IGameStateInterface component in interfaces) {
					component.Reset();
				}
				break;
			case GameState.WIN:
				GameObject[] winObjects = GameObject.FindGameObjectsWithTag("Win");
				foreach (GameObject obj in winObjects) {
					Animator animator = obj.GetComponent<Animator>();
					animator.SetTrigger("Reset");
				}
				break;
			case GameState.LOSE:
				GameObject[] loseObjects = GameObject.FindGameObjectsWithTag("Lose");
				foreach (GameObject obj in loseObjects) {
					Animator animator = obj.GetComponent<Animator>();
					animator.SetTrigger("Reset");
				}
				break;
		}
	}
}
