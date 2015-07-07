using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IGameStateInterface {
	// PRAGMA MARK - IGameStateInterface
	void IGameStateInterface.Reset() {
		transform.position = _storedStartPosition;
	}
	
	protected Vector3 _storedStartPosition;
	
	protected void Start () {
		_storedStartPosition = transform.position;
	}
}
