using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype
{
    [UsedImplicitly]
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline/The Dark Chain")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField] private bool m_useDynamicBatching = true;
        [SerializeField] private bool m_useGPUInstancing = true;
        [SerializeField] private bool m_useSRPBatcher = true;

        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipeline(m_useDynamicBatching, m_useGPUInstancing, m_useSRPBatcher);
        }
    }
}