using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WrinkleMapsDriver))]
public class WrinkleMapsDriverEd : Editor {
	new WrinkleMapsDriver target { get { return base.target as WrinkleMapsDriver; } }

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		if(target.IsValidForPreview) {
			EditorGUILayout.Space();
			EditorGUILayout.Foldout(true, "Blendshape Preview:");

			var smr = target.SkinnedMeshRenderer;
			var smesh = smr.sharedMesh;
			var max = target.MaxShapes;

			for(int i = 0; i < max; ++i) { 
				var name = smesh.GetBlendShapeName(i);
				var val = Mathf.Clamp(smr.GetBlendShapeWeight(i), 0f, 100f);
				smr.SetBlendShapeWeight(i, EditorGUILayout.Slider(name, val, 0f, 100f));
			}

			EditorGUILayout.Space();

			if(GUILayout.Button("Reset Preview"))
				for(int i = 0; i < max; ++i)
					smr.SetBlendShapeWeight(i, 0f);
		} else {
			EditorGUILayout.Space();
			
			EditorGUILayout.HelpBox(
					"Preview mode currently not available for this object!\n\n" +
					"Driver component needs to:\n" +
					"- be configured with a valid 'SkinnedMeshRenderer' target.\n" +
					"- contain at least one blend shape and associated wrinkle map configuration.\n" +
					"- contain no empty texture slot in any wrinkle map configuration."
					, MessageType.Warning);
		}

		if(GUI.changed)
			SceneView.RepaintAll();
	}
}
