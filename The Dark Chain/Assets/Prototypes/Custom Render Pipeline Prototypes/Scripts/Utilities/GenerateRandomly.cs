using JetBrains.Annotations;
using UnityEngine;

namespace Prototype.SRP
{
    public class GenerateRandomly : MonoBehaviour
    {
        private const int SIZE = 1024;
        private static int m_baseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Mesh m_mesh = default;
        [SerializeField] private Material m_material = default;

        private MaterialPropertyBlock m_block;
        private Matrix4x4[] m_matrices = new Matrix4x4[SIZE];
        private Vector4[] m_baseColors = new Vector4[SIZE];

        [UsedImplicitly]
        private void Awake()
        {
            const float radius = 5.0f;

            for (int i = 0; i < m_matrices.Length; i++)
            {
                m_matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * radius,
                    Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f),
                    Vector3.one * Random.Range(0.5f, 1.5f));
                m_baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if (m_block == null)
            {
                m_block = new MaterialPropertyBlock();
                m_block.SetVectorArray(m_baseColorId, m_baseColors);
            }

            for (int i = 0; i < m_matrices.Length; i++)
            {
                float delta = 1.0f * Time.deltaTime;
                Matrix4x4 translate = Matrix4x4.Translate(new Vector3(0, delta, 0));
                m_matrices[i] *= translate;
            }

            Graphics.DrawMeshInstanced(m_mesh, 0, m_material, m_matrices, SIZE, m_block);
        }
    }
}