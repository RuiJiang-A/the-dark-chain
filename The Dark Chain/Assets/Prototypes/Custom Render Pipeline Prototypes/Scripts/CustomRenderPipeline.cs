using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private bool m_useDynamicBatching;
        private bool m_useGPUInstancing;

        public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
        {
            m_useDynamicBatching = useDynamicBatching;
            m_useGPUInstancing = useGPUInstancing;
            GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
        }

        private CustomCameraRenderer m_renderer = new();

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (Camera camera in cameras)
                m_renderer.Render(context, camera, m_useDynamicBatching, m_useGPUInstancing);
        }
    }
}