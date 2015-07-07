using UnityEngine;
using System.Collections;

public class ConditionController : MonoBehaviour, IGameStateInterface {
	// PRAGMA MARK - IGameStateInterface
	void IGameStateInterface.Reset() {
		gameObject.GetComponent<Animator>().SetTrigger("Disappear");
	}
}
