using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WrinkleMapsTargetProxy : MonoBehaviour {
	[HideInInspector] public WrinkleMapsDriver owner;

	void OnWillRenderObject() {
		owner.ProxyWillRenderObject();
	}

	void OnRenderObject() {
		owner.ProxyRenderObject();
	}
}
