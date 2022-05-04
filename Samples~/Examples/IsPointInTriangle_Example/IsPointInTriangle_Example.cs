using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    public class IsPointInTriangle_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject TriP1;
        [SerializeField] GameObject TriP2;
        [SerializeField] GameObject TriP3;

        [SerializeField] GameObject PointP;

        public enum PointTriangleMethod
        {
            Barycentric,
            Orientation,
            Area
        }
        [SerializeField] PointTriangleMethod oreintationMethod;

        void OnGUI()
        {
            //Convert World Pos to GUI Pos
            Vector3 GUIp1 = cam.WorldToScreenPoint(TriP1.transform.position);
            Vector3 GUIp2 = cam.WorldToScreenPoint(TriP2.transform.position);
            Vector3 GUIp3 = cam.WorldToScreenPoint(TriP3.transform.position);

            GUIp1.y = UnityOnGUIHelper.Height - GUIp1.y;
            GUIp2.y = UnityOnGUIHelper.Height - GUIp2.y;
            GUIp3.y = UnityOnGUIHelper.Height - GUIp3.y;

            //Draw Triangle
            UnityOnGUIHelper.DrawLine(GUIp1, GUIp2, Color.red, 8);
            UnityOnGUIHelper.DrawLine(GUIp2, GUIp3, Color.red, 8);
            UnityOnGUIHelper.DrawLine(GUIp3, GUIp1, Color.red, 8);


            int IsPointInTriangle;
            if (oreintationMethod == PointTriangleMethod.Barycentric)
            {
                IsPointInTriangle = GeometryUtils.IsPointInTriangle(TriP1.transform.position, TriP2.transform.position, TriP3.transform.position, PointP.transform.position);
            }
            else if (oreintationMethod == PointTriangleMethod.Orientation)
            {
                IsPointInTriangle = GeometryUtils.IsPointInTriangleOrientation(TriP1.transform.position, TriP2.transform.position, TriP3.transform.position, PointP.transform.position);
            }
            else if (oreintationMethod == PointTriangleMethod.Area)
            {
                IsPointInTriangle = GeometryUtils.IsPointInTriangleArea(TriP1.transform.position, TriP2.transform.position, TriP3.transform.position, PointP.transform.position);
            }
            else
            {
                IsPointInTriangle = -1;
            }

            if (IsPointInTriangle == 1) Text.text = "Point Is Inside Of Triangle";
            else if (IsPointInTriangle == -1) Text.text = "Point Is Outside Of Triangle";
            else Text.text = "Point Is On Triangle";
        }
    }
}
