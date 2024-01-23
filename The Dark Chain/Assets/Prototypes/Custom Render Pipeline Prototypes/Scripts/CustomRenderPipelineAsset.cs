using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype
{
    [UsedImplicitly]
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline/The Dark Chain")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipeline();
        }
    }
}