using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype
{
    public class CustomCameraRenderer
    {
        [Header("Settings")] private ScriptableRenderContext m_context;
        private Camera m_camera;

        [Header("Command Buffer")] private const string BUFFER_NAME = "Custom Camera Renderer";

        private CommandBuffer m_buffer = new()
        {
            name = BUFFER_NAME
        };

        CullingResults m_objectsInFrustum;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            m_context = context;
            m_camera = camera;

            if (!Culled()) return;

            Begin();
            OnRender(context, camera);
            Flush();
        }

        protected void OnRender(ScriptableRenderContext context, Camera camera)
        {
            context.DrawSkybox(camera);
        }

        private void Begin()
        {
            // Start Profile
            m_buffer.BeginSample(BUFFER_NAME);
            // Set up camera
            m_context.SetupCameraProperties(m_camera);
            // Clear frame buffer objects
            m_buffer.ClearRenderTarget(true, true, Color.clear);
            ExecuteBuffer();
        }

        private void Flush()
        {
            // Submit context
            m_context.Submit();
            // Clear buffer
            m_buffer.Clear();

            // End Profile
            m_buffer.EndSample(BUFFER_NAME);
        }

        private void ExecuteBuffer()
        {
            m_context.ExecuteCommandBuffer(m_buffer);
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

            m_objectsInFrustum = m_context.Cull(ref parameters);
            return true;
        }
    }
}