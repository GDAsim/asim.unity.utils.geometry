using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    public class OrientationPoint_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject LineP1;
        [SerializeField] GameObject LineP2;
        [SerializeField] GameObject PointP;

        public enum OrientationMethod
        {
            CrossProduct,
            DotProduct
        }
        [SerializeField] OrientationMethod oreintationMethod;

        void OnGUI()
        {
            //Convert World Pos to GUI Pos
            Vector3 GUIp1 = cam.WorldToScreenPoint(LineP1.transform.position);
            Vector3 GUIp2 = cam.WorldToScreenPoint(LineP2.transform.position);
            GUIp1.y = UnityOnGUIHelper.Height - GUIp1.y;
            GUIp2.y = UnityOnGUIHelper.Height - GUIp2.y;

            //Draw Line of p1->p2
            UnityOnGUIHelper.DrawLine(GUIp1, GUIp2, Color.red, 8);

            int orientation;
            if (oreintationMethod == OrientationMethod.CrossProduct)
            {
                orientation = GeometryUtils.Orientation(LineP1.transform.position, LineP2.transform.position, PointP.transform.position);
            }
            else
            {
                orientation = GeometryUtils.Orientation2(LineP1.transform.position, LineP2.transform.position, PointP.transform.position);
            }

            if (orientation == 1) Text.text = "Point Is Left Of Vector";
            else if (orientation == -1) Text.text = "Point Is Right Of Vector";
            else Text.text = "Point Parallel Of Vector";
        }
    }
}
