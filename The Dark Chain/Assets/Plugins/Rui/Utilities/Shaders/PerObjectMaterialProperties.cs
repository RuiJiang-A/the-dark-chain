using JetBrains.Annotations;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int m_baseColorId = Shader.PropertyToID("_BaseColor");
    private static int m_cutoffId = Shader.PropertyToID("_Cutoff");

    [SerializeField] private Color m_baseColor = Color.white;
    [SerializeField, Range(0f, 1f)] private float m_cutoff = 0.5f;
    private static MaterialPropertyBlock m_block;

    [UsedImplicitly]
    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_block ??= new MaterialPropertyBlock();

        m_block.SetColor(m_baseColorId, m_baseColor);
        m_block.SetFloat(m_cutoffId, m_cutoff);
        GetComponent<Renderer>().SetPropertyBlock(m_block);
    }
}