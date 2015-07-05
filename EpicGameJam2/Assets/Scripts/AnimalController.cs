using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DT.FiniteStateMachine.TweakableVariableExtensions;
using DT.TweakableVariables;

public class AnimalController : MonoBehaviour {
	protected const float DEFAULT_LEANING_MIN_TIME = 2.0f;
	protected const float DEFAULT_LEANING_MAX_TIME = 6.0f;
	
	protected const float DEFAULT_PRELOOK_MIN_TIME = 0.3f;
	protected const float DEFAULT_PRELOOK_MAX_TIME = 1.0f;
	
	protected const float DEFAULT_LOOKING_MIN_TIME = 2.0f;
	protected const float DEFAULT_LOOKING_MAX_TIME = 6.0f;
	
	protected const string LEANING_STATE_ID = "animal::leaning";
	protected const string PRELOOK_STATE_ID = "animal::prelook";
	protected const string LOOKING_STATE_ID = "animal::looking";
	
	protected Dictionary<string, string>stateIdToTriggerMap = new Dictionary<string, string> {
		{LEANING_STATE_ID, "Lean"},
		{PRELOOK_STATE_ID, "PreLook"},
		{LOOKING_STATE_ID, "Look"}
	};
	
	protected TweakableFiniteStateMachine _stateMachine;
	protected Animator _animator;
	protected PlayerMovementController _player;
	
	protected IndicatorView _indicatorView;
	protected float _suspicionLevel = 0.0f;
	protected TweakableFloat _suspicionMultiplier;
	protected float SuspicionMultiplier {
		get { return _suspicionMultiplier.Value; }
	}
	
	protected void Awake() { 
		_suspicionMultiplier = new TweakableFloat("AnimalSuspicionMultiplier", 0.0f, 1.0f, 0.5f);
		_animator = GetComponent<Animator>();
		
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players.Length != 1) {
			Debug.LogError("Weird number of players found: " + players.Length);
		}
		_player = players[0].GetComponent<PlayerMovementController>();
		
		IndicatorView[] indicatorViews = transform.GetComponentsInChildren<IndicatorView>();
		if (indicatorViews.Length != 1) {
			Debug.LogError("Weird number of indicator views found: " + indicatorViews.Length);
		}
		_indicatorView = indicatorViews[0];
		(transform.Find("Indicator").gameObject as GameObject).SetActive(false);
		
		SetUpStateMachine();
	}
		
	protected void SetUpStateMachine() {
		_stateMachine = gameObject.AddComponent<TweakableFiniteStateMachine>() as TweakableFiniteStateMachine;
		
		_stateMachine.AddState(LEANING_STATE_ID, DEFAULT_LEANING_MIN_TIME, DEFAULT_LEANING_MAX_TIME);
		_stateMachine.AddState(PRELOOK_STATE_ID, DEFAULT_PRELOOK_MIN_TIME, DEFAULT_PRELOOK_MAX_TIME);
		_stateMachine.AddState(LOOKING_STATE_ID, DEFAULT_LOOKING_MIN_TIME, DEFAULT_LOOKING_MAX_TIME);
		
		_stateMachine.AddTransition(LEANING_STATE_ID, PRELOOK_STATE_ID, 1.0f);
		_stateMachine.AddTransition(PRELOOK_STATE_ID, LOOKING_STATE_ID, 0.6f);
		_stateMachine.AddTransition(PRELOOK_STATE_ID, LEANING_STATE_ID, 0.4f);
		_stateMachine.AddTransition(LOOKING_STATE_ID, LEANING_STATE_ID, 1.0f);
		
		_stateMachine.AddStateChangeAction(HandleStateChange);
		
		_stateMachine.SetStartState(LEANING_STATE_ID);
	}
	
	protected void Update() {
		if (_stateMachine.CurrentStateId().Equals(LOOKING_STATE_ID)) {
			float suspicionDelta = Time.deltaTime * _player.CurrentRelativeSpeed() * SuspicionMultiplier;
			_suspicionLevel += suspicionDelta;
			
			_indicatorView.UpdateFill(_suspicionLevel);
			if (_suspicionLevel >= 1.0f) {
				Debug.Log("Player was caught!");
			}
		}
	}
	
	protected virtual void HandleStateChange(string previousStateId, string nextStateId) {
		_animator.SetTrigger(stateIdToTriggerMap[nextStateId]);
	}
}
