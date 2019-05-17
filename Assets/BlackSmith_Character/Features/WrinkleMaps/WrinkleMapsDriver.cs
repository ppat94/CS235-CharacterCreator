using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class WrinkleMapsDriver : MonoBehaviour {
	[HideInInspector] public Shader occlusionShader;

	[System.Serializable] public class WrinkleMap {
		public Texture2D	normalMap;
		public Texture2D	occlusionMap;
		public float 		bumpScale;
		public float 		occlusionStrength;
		public Vector4 		maskWeights;
	}

//	public enum DebugMode { None, Normals, Occlusion, Lighting, Mask, Influences }

	// TODO: This isn't an optimization (yet), it just disables
	//       the influence of the visual effect.
	//
	//       We probably won't support toggling these individually for perf, but rather
	//       we'll turn off wrinkles altogether instead.
	//
	public bool					useWrinkleNormals = true;
	public bool					useWrinkleOcclusion = true;

	public GameObject			target;
	public Transform			targetBone;

	public LayerMask			cullingMask = ~0;
	public float				maxDistance = 25f;

	public Texture2D			wrinkleMask;
	public WrinkleMap[] 		wrinkleMaps;

//	public DebugMode			debugMode = DebugMode.None;

	public bool					IsValidForPreview	{ get; private set; }
	public SkinnedMeshRenderer	SkinnedMeshRenderer	{ get { return m_renderer; } }
	public int					MaxShapes 			{ get { return m_maxShapes; } }
	
	int							m_wrinkleMaskSID;
	int[]						m_wrinkleNormalMapSIDS;
	int[]						m_wrinkleOcclusionMapSIDS;
	int[]						m_wrinkleInfluenceSIDS;
	int							m_wrinkleBumpScalesSID;
	int							m_wrinkleOcclusionStrengthsSID;
	int							m_normalAndOcclusionSID;

	SkinnedMeshRenderer			m_renderer;
	Material[]					m_materials;
	int							m_maxShapes;
	int							m_maxConcurrentShapes;
	uint[]						m_sortingKeysValues;

	Camera						m_occlusionCamera;
	Dictionary<int, Camera>		m_occlusionCameras;

	bool						m_hasAwoken;
	bool						m_isFullyConfigured;

	void Awake() {
		m_wrinkleMaskSID = Shader.PropertyToID("_WrinkleMask");
		m_wrinkleBumpScalesSID = Shader.PropertyToID("_WrinkleMapBumpScales");
		m_wrinkleOcclusionStrengthsSID = Shader.PropertyToID("_WrinkleOcclusionStrengths");
		m_normalAndOcclusionSID = Shader.PropertyToID("_NormalAndOcclusion");
		m_wrinkleNormalMapSIDS = new [] {
			Shader.PropertyToID("_WrinkleNormalMap0"),
			Shader.PropertyToID("_WrinkleNormalMap1"),
			Shader.PropertyToID("_WrinkleNormalMap2"),
			Shader.PropertyToID("_WrinkleNormalMap3")
		};
		m_wrinkleOcclusionMapSIDS = new [] {
			Shader.PropertyToID("_WrinkleOcclusionMap0"),
			Shader.PropertyToID("_WrinkleOcclusionMap1"),
			Shader.PropertyToID("_WrinkleOcclusionMap2"),
			Shader.PropertyToID("_WrinkleOcclusionMap3")
		};
		m_wrinkleInfluenceSIDS = new [] {
			Shader.PropertyToID("_WrinkleInfluences0"),
			Shader.PropertyToID("_WrinkleInfluences1"),
			Shader.PropertyToID("_WrinkleInfluences2"),
			Shader.PropertyToID("_WrinkleInfluences3")
		};

		m_hasAwoken = true;
	}

	void OnEnable() {
		Reconfigure();

		m_occlusionCamera = CreateOcclusionCamera(null);

		if(Application.isEditor)
			m_occlusionCameras = new Dictionary<int, Camera>();
	}

	void OnDisable() {
		if(m_materials != null)
			ClearShaderKeywords();
	}

	void OnValidate() {
		if(m_hasAwoken)
			Reconfigure();
	}

	void OnDestroy() {
		if(target && target != gameObject) {
			var proxy = target.GetComponent<WrinkleMapsTargetProxy>();
			if(proxy && proxy.owner == this)
				Object.DestroyImmediate(proxy);
		}

		if(m_occlusionCamera) {
			RenderTexture.ReleaseTemporary(m_occlusionCamera.targetTexture);
			Object.DestroyImmediate(m_occlusionCamera.gameObject);
		}

		if(m_occlusionCameras != null) {
			foreach(var cam in m_occlusionCameras.Values) {
				RenderTexture.ReleaseTemporary(cam.targetTexture);
				Object.DestroyImmediate(cam.gameObject);
			}
		}
	}

	Camera CreateOcclusionCamera(Camera source) {
		var camGO = new GameObject("#> _Wrinkles Occlusion Camera < " + this.name + " < " + (source ? source.name : "(game main)"));
		camGO.hideFlags = HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideInHierarchy;

		var cam = camGO.AddComponent<Camera>();
		cam.enabled = false;

		return cam;
	}

	RenderTexture UpdateOcclusionCamera(Camera source, Camera target, Shader replacement) {
		if(target.targetTexture) {
			Debug.LogError("Didn't expect existing render texture: " + target.name);
			RenderTexture.ReleaseTemporary(target.targetTexture);
			target.targetTexture = null;
		}

		// Start by copying everything from src, but then override quite a few things.
		target.CopyFrom(source);

		var pr = source.pixelRect;
		var rt = target.targetTexture = RenderTexture.GetTemporary(
			Mathf.RoundToInt(pr.width),
			Mathf.RoundToInt(pr.height),
			Application.isPlaying ? 24 : 16, // this avoids a bug that corrupts scene view depth
			RenderTextureFormat.ARGB2101010
		);

		target.renderingPath = RenderingPath.Forward;
		target.depthTextureMode = DepthTextureMode.None;
		target.clearFlags = CameraClearFlags.SolidColor;
		target.backgroundColor = Color.white;
		target.useOcclusionCulling = false;
		target.cullingMask = cullingMask;
		target.farClipPlane = maxDistance + 5f;

		target.RenderWithShader(replacement, "Special");

		return rt;
	}

	void Reconfigure() {
		m_isFullyConfigured = false;
		IsValidForPreview = false;

		m_renderer = (target ?? gameObject).GetComponent<SkinnedMeshRenderer>();
		if(m_renderer == null)
			return;

		if(target != gameObject) {
			var proxy = target.GetComponent<WrinkleMapsTargetProxy>() ?? target.AddComponent<WrinkleMapsTargetProxy>();
			if(proxy.owner != this)
				proxy.owner = this;
		}

		m_materials = m_renderer.sharedMaterials;

		bool hasValidWrinkles = true;
		foreach(var w in wrinkleMaps)
			hasValidWrinkles = hasValidWrinkles && w.normalMap != null && w.occlusionMap != null;

		m_maxShapes = Mathf.Min(wrinkleMaps.Length,  m_renderer.sharedMesh.blendShapeCount);
		m_maxConcurrentShapes = Mathf.Min(m_maxShapes, 4);

		m_sortingKeysValues = new uint[m_maxShapes];

		foreach(var m in m_materials)
			for(int j = 0; j < m_maxConcurrentShapes; ++j)
				m.SetVector(m_wrinkleInfluenceSIDS[j], Vector4.zero);

		SetShaderKeywords();

		m_isFullyConfigured = true;
		IsValidForPreview = hasValidWrinkles && m_maxShapes > 0;
	}

	void ClearShaderKeywords() {
		foreach(var m in m_materials) {
			m.DisableKeyword("WRINKLE_MAPS");
//			m.DisableKeyword("DBG_NONE");
//			m.DisableKeyword("DBG_NORMALS");
//			m.DisableKeyword("DBG_OCCLUSION");
//			m.DisableKeyword("DBG_LIGHTING");
//			m.DisableKeyword("DBG_MASK");
//			m.DisableKeyword("DBG_DETAIL_INFLUENCES");
		}
	}

	void SetShaderKeywords() {
		ClearShaderKeywords();

		foreach(var m in m_materials) {
			m.EnableKeyword("WRINKLE_MAPS");

//			if(debugMode == DebugMode.Normals)
//				m.EnableKeyword("DBG_NORMALS");
//			else if(debugMode == DebugMode.Occlusion)
//				m.EnableKeyword("DBG_OCCLUSION");
//			else if(debugMode == DebugMode.Lighting)
//				m.EnableKeyword("DBG_LIGHTING");
//			else if(debugMode == DebugMode.Mask)
//				m.EnableKeyword("DBG_MASK");
//			else if(debugMode == DebugMode.Influences)
//				m.EnableKeyword("DBG_DETAIL_INFLUENCES");
//			else
//				m.EnableKeyword("DBG_NONE");
		}
	}

	void ToggleWrinkleMaps(bool enable) {
		if(m_materials.Length > 0 && m_materials[0].IsKeywordEnabled("WRINKLE_MAPS") == enable)
			return;

		if(enable) {
			for(int i = 0, n = m_materials.Length; i < n; ++i)
				m_materials[i].EnableKeyword("WRINKLE_MAPS");
		} else {
			for(int i = 0, n = m_materials.Length; i < n; ++i)
				m_materials[i].DisableKeyword("WRINKLE_MAPS");
		}
	}

	void UpdateWrinkles(RenderTexture normalAndOcclusion) {
		// This function does "double setup" by setting shader uniforms
		// used both by the replacement shader, and the actual render shader.

		for(int i = 0; i < m_maxShapes; ++i) {
			var w = m_renderer.GetBlendShapeWeight(i);
			m_sortingKeysValues[i] = (uint)(Mathf.CeilToInt(w * 10000f) << 8) | (uint)(i & 0xFF);
		}

		InsertionSortInPlace(m_sortingKeysValues);

		for(int i = 0, n = m_materials.Length; i < n; ++i) {
			var m = m_materials[i];

			var bumpScales = Vector4.zero;
			var occlusionStrengths = Vector4.zero;
			for(int j = 0; j < m_maxConcurrentShapes; ++j) {
				var bwIdx = (int)(m_sortingKeysValues[m_maxShapes - j - 1] & 0xFF);

				var w = wrinkleMaps[bwIdx];
				if(w.normalMap == null || w.occlusionMap == null)
					continue;

				bumpScales[j] = w.bumpScale;
				occlusionStrengths[j] = w.occlusionStrength;

				var influence = Mathf.Clamp01(m_renderer.GetBlendShapeWeight(bwIdx) / 100f);
				m.SetVector(m_wrinkleInfluenceSIDS[j], w.maskWeights * influence);
				m.SetTexture(m_wrinkleNormalMapSIDS[j], w.normalMap);
				m.SetTexture(m_wrinkleOcclusionMapSIDS[j], w.occlusionMap);
			}

			if(!useWrinkleNormals)
				bumpScales = Vector4.zero;

			if(!useWrinkleOcclusion)
				occlusionStrengths = Vector4.zero;

			m.SetVector(m_wrinkleBumpScalesSID, bumpScales);
			m.SetVector(m_wrinkleOcclusionStrengthsSID, occlusionStrengths);

			if(wrinkleMask)
				m.SetTexture(m_wrinkleMaskSID, wrinkleMask);

			if(normalAndOcclusion)
				m.SetTexture(m_normalAndOcclusionSID, normalAndOcclusion);
		}
	}

	// No-alloc sort
	static public void InsertionSortInPlace(uint[] a) {
		for(int i = 1, n = a.Length; i < n; ++i) {
			var t = a[i];
			int j = i;
			for(; j > 0 && a[j-1] > t; --j)
				a[j] = a[j-1];
			a[j] = t;
		}
	}
	
	public void ProxyWillRenderObject() {
		if(!m_isFullyConfigured)
			return;

		var currentCam = Camera.current;

		if(Application.isEditor && currentCam != Camera.main)
			RenderSceneView(currentCam);
		else
			RenderGameView(currentCam);
	}

	public void ProxyRenderObject() {
		if(!m_isFullyConfigured)
			return;

		RenderTexture rt = null;
		var currentCam = Camera.current;
		
		if(currentCam == Camera.main) {
			rt = m_occlusionCamera.targetTexture;
			m_occlusionCamera.targetTexture	= null;
		} else if(m_occlusionCameras != null) {
			Camera target;
			if(m_occlusionCameras.TryGetValue(currentCam.GetHashCode(), out target)) {
				rt = target.targetTexture;
				target.targetTexture = null;
			}
		}
	
		RenderTexture.ReleaseTemporary(rt);
	}

	void RenderSceneView(Camera cam) {
		var shouldRender = cam.name == "SceneCamera" && !m_occlusionCameras.ContainsValue(cam);
		if(shouldRender) {
			Camera target;
			if(!m_occlusionCameras.TryGetValue(cam.GetHashCode(), out target))
				m_occlusionCameras[cam.GetHashCode()] = target = CreateOcclusionCamera(cam);

			if(target.targetTexture != null) {
				//Debug.Log("Aborting rendering since we already have data " + target.name);
				return;
			}

			bool distCull = Vector3.SqrMagnitude(cam.transform.position - targetBone.transform.position) >= (maxDistance * maxDistance);
			ToggleWrinkleMaps(!distCull);

			var rt = UpdateOcclusionCamera(cam, target, occlusionShader);
			UpdateWrinkles(rt);
		}
	}

	void RenderGameView(Camera cam) {
		if(cam == Camera.main) {
			if(m_occlusionCamera.targetTexture != null) {
				//Debug.Log("Aborting rendering since we already have data " + m_occlusionCamera.name);
				return;
			}
			
			bool distCull = Vector3.SqrMagnitude(cam.transform.position - targetBone.transform.position) >= (maxDistance * maxDistance);
			ToggleWrinkleMaps(!distCull);

			var rt = UpdateOcclusionCamera(cam, m_occlusionCamera, occlusionShader);
			UpdateWrinkles(rt);
		}
	}
}
