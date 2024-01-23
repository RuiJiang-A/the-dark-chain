using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private CustomCameraRenderer m_renderer = new();

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (Camera camera in cameras)
                m_renderer.Render(context, camera);
        }
    }
}