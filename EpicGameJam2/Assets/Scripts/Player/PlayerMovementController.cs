using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {
	public const string HORIZONTAL_AXIS_KEY = "Horizontal";
	
	protected Rigidbody2D _rigidbody;
	protected Animator _animator;
	protected DT.TweakableFloat _playerSpeed;
	protected float PlayerSpeed {
		get { return _playerSpeed.Value; }
	}
	protected float currentXAxis;
	
	public void Awake() {
		_playerSpeed = new DT.TweakableFloat("PlayerSpeed", 0.0f, 10.0f, 2.0f);
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}
	
	public void Update() {
		currentXAxis = Input.GetAxis(HORIZONTAL_AXIS_KEY);
		
		_animator.speed = currentXAxis;
	}
	
	public void FixedUpdate() {
		_rigidbody.velocity = new Vector2(currentXAxis * PlayerSpeed, _rigidbody.velocity.y);
	}
}
