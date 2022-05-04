using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    public class IsPointInEllipse_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject TargetCenter;
        public Vector2 EllipseSize;

        [SerializeField] GameObject Point;

        float rotation = 0;
        void OnGUI()
        {
            //Convert World Pos to GUI Pos
            Vector3 GUIcenterpos = cam.WorldToScreenPoint(TargetCenter.transform.position);
            Vector3 GUIpointpos = cam.WorldToScreenPoint(Point.transform.position);
            GUIcenterpos.y = UnityOnGUIHelper.Height - GUIcenterpos.y;
            GUIpointpos.y = UnityOnGUIHelper.Height - GUIpointpos.y;

            Vector3 radiusX = cam.WorldToScreenPoint(TargetCenter.transform.position + new Vector3(EllipseSize.x, 0, 0));
            Vector3 radiusY = cam.WorldToScreenPoint(TargetCenter.transform.position + new Vector3(0, EllipseSize.y, 0));
            radiusX.y = UnityOnGUIHelper.Height - radiusX.y;
            radiusY.y = UnityOnGUIHelper.Height - radiusY.y;
            var GUIEllipseSize = new Vector2(Vector3.Distance(GUIcenterpos, radiusX), Vector3.Distance(GUIcenterpos, radiusY));

            rotation += 0.003f;

            int IsPointInEllipse = GeometryUtils.IsPointInEllipse(GUIcenterpos, GUIEllipseSize, -rotation, GUIpointpos);
            if (IsPointInEllipse == 1) Text.text = "Point Inside Ellipse";
            else if (IsPointInEllipse == -1) Text.text = "Point Outside Ellipse";
            else Text.text = "Point On Ellipse";

            UnityOnGUIHelper.DrawEllipse(GUIcenterpos, GUIEllipseSize, rotation, new Color32(0, 255, 0,255), Color.red, 1);
        }
    }
}
