using UnityEngine;
using System.Collections;

public class IndicatorView : MonoBehaviour {
	protected Animator _animator;
	protected tk2dClippedSprite _fillSprite;
	protected float _cachedPercentage;
	protected bool _updatedPercentage;
	
	public void UpdateFill(float percentage) {
		if (percentage == _cachedPercentage) {
			return;
		}
		
		_cachedPercentage = percentage;
		
		_updatedPercentage = true;
		_fillSprite.ClipRect = new Rect(0, 0, 1.0f, Mathf.Min(percentage, 1.0f));
	}
	
	protected void LateUpdate() {
		_animator.SetBool("Pulsing", _updatedPercentage);
		if (_updatedPercentage) {
			_updatedPercentage = false;
		}
	}
	
	protected void Awake() {
		_animator = GetComponent<Animator>();
		_fillSprite = transform.Find("Sprites/Fill").GetComponent<tk2dClippedSprite>();
	}
}
