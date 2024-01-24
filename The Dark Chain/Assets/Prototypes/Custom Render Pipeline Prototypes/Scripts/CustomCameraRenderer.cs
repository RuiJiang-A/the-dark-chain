using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype
{
    public partial class CustomCameraRenderer
    {
        [Header("Settings")] private ScriptableRenderContext m_context;
        private Camera m_camera;

        [Header("Command Buffer")] private CommandBuffer m_buffer = new();

        private CullingResults m_cullingResults;

        private static ShaderTagId unlitShaderTagId = new("SRPDefaultUnlit");

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            m_context = context;
            m_camera = camera;

            PrepareBuffer();
            EmitGeometryForSceneView();
            if (!Culled()) return;

            Begin();
            OnRender();
            DrawGizmos();
            Flush();
        }

        private void Begin()
        {
            // Start Profile
            m_buffer.BeginSample(m_buffer.name);
            // Set up camera
            m_context.SetupCameraProperties(m_camera);
            // Most of the clear flag will be set as CameraClearFlags.Skybox
            CameraClearFlags flags = m_camera.clearFlags;
            // Clear frame buffer objects
            bool shouldClearDepth = flags != CameraClearFlags.Nothing;
            bool shouldClearColor = flags <= CameraClearFlags.Color;
            Color clearColor = flags <= CameraClearFlags.Color ? m_camera.backgroundColor.linear : Color.clear;
            m_buffer.ClearRenderTarget(shouldClearDepth, shouldClearColor, clearColor);
            ExecuteAndClearBuffer();
        }

        private void Flush()
        {
            // End Profile
            m_buffer.EndSample(m_buffer.name);
            // Clear buffer
            ExecuteAndClearBuffer();
            // Submit context
            m_context.Submit();
        }

        private void ExecuteAndClearBuffer()
        {
            m_context.ExecuteCommandBuffer(m_buffer);
            m_buffer.Clear();
        }

        private bool Culled()
        {
            if (!m_camera.TryGetCullingParameters(out ScriptableCullingParameters parameters))
                return false;

            m_cullingResults = m_context.Cull(ref parameters);
            return true;
        }

        protected void OnRender()
        {
            DrawGeometry();
        }

        private void DrawGeometry()
        {
            var sortingSettings = new SortingSettings(m_camera);
            sortingSettings.criteria = SortingCriteria.CommonOpaque;

            var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);

            m_context.DrawSkybox(m_camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;

            m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
        }
    }
}