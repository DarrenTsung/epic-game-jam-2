using UnityEngine;
using System.Collections;
using DT.TweakableVariables;

public class PlayerMovementController : MonoBehaviour {
	public const string HORIZONTAL_AXIS_KEY = "Horizontal";
	public const float PIXEL_SIZE = 0.1f;
	
	protected Rigidbody2D _rigidbody;
	protected Animator _animator;
	protected Transform _spriteTransforms;
	protected TweakableFloat _playerSpeed;
	protected float PlayerSpeed {
		get { return _playerSpeed.Value; }
	}
	protected float currentXAxis;
	
	public float CurrentRelativeSpeed() {
		return currentXAxis;
	}
	
	public void Awake() {
		_playerSpeed = new TweakableFloat("pSpeed", 0.0f, 10.0f, 2.0f);
		_spriteTransforms = transform.Find("Sprites");
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}
	
	public void Update() {
		if (GameManager.Instance.CurrentState != GameState.GAME) {
			currentXAxis = 0.0f;
		} else {
			currentXAxis = Input.GetAxis(HORIZONTAL_AXIS_KEY);
		}
		
		_animator.SetFloat("SneakingSpeed", currentXAxis);
		_animator.SetBool("BeingInconspicuous", currentXAxis <= 0.001);
		
		AnimatorStateInfo inconspicuousLayerState = _animator.GetCurrentAnimatorStateInfo(1);
		if (inconspicuousLayerState.IsTag("NoOverride")) {
			_animator.SetLayerWeight(1, 0);
		} else {
			_animator.SetLayerWeight(1, 1);
		}
	}
	
	public void LateUpdate() {
		// offset the sprite from the player so that they are pixel perfect
		float offsetX = -(transform.position.x % PIXEL_SIZE);
		float offsetY = -(transform.position.y % PIXEL_SIZE);
		_spriteTransforms.localPosition = new Vector3(offsetX, offsetY);
	}
	
	public void FixedUpdate() {
		_rigidbody.velocity = new Vector2(currentXAxis * PlayerSpeed, _rigidbody.velocity.y);
	}
}
