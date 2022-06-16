using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : SingletonGameStateObserver<T> where T:Component{

	protected bool m_IsReady = false;
	public bool IsReady { get { return m_IsReady; } }

	protected abstract IEnumerator InitCoroutine();

	// Use this for initialization
	protected virtual IEnumerator Start () {
		m_IsReady = false;
		yield return StartCoroutine(InitCoroutine());
		m_IsReady = true;
		GameManager.Instance.standsOrigin = GameManager.Instance.Stands.transform.localPosition;
		GameManager.Instance.pillarsOrigin = GameManager.Instance.Pillars.transform.localPosition;
		GameManager.Instance.targetPillars = new Vector3(9, -0.5f, 0);
		GameManager.Instance.targetStands = new Vector3(23.16f, -4.74f, 2.133845f);
	}



}
