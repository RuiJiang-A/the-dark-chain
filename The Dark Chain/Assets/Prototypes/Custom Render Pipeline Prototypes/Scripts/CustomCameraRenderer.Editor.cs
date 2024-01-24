using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace Prototype
{
    partial class CustomCameraRenderer
    {
        partial void DrawGizmos();
        partial void EmitGeometryForSceneView();
        partial void PrepareBuffer();

#if UNITY_EDITOR
        partial void DrawGizmos()
        {
            if (!Handles.ShouldRenderGizmos()) return;
            m_context.DrawGizmos(m_camera, GizmoSubset.PreImageEffects);
            m_context.DrawGizmos(m_camera, GizmoSubset.PostImageEffects);
        }

        partial void EmitGeometryForSceneView()
        {
            if (m_camera.cameraType != CameraType.SceneView) return;
            ScriptableRenderContext.EmitWorldGeometryForSceneView(m_camera);
        }

        partial void PrepareBuffer()
        {
            Profiler.BeginSample("Editor Only");
            m_buffer.name = m_camera.name;
            Profiler.EndSample();
        }
#endif
    }
}