using Drawing;
using UnityEngine;

namespace Boopoo.MotionMatching
{
    public class MotionMatchingVisualizer : MonoBehaviourGizmos
    {
        [SerializeField] private MotionMatchingManager _motionMatchingManager;

        public override void DrawGizmos()
        {

            DrawFeatures();
            DrawDesiredTrajectory();
            DrawSimulatedTrajectory();

            // using (var draw = DrawingManager.GetBuilder(true))
            // {
            //     using (draw.WithColor(Color.white))
            //     {
            //         using (draw.InLocalSpace(transform))
            //         {
            //             draw.WireSphere(Vector3.zero, 0.2f);
            //         }
            //     }
            // }
        }

        
        private void DrawFeatures()
        {
        }

        private void DrawDesiredTrajectory()
        {
        }

        private void DrawSimulatedTrajectory()
        {
        }
    }
}