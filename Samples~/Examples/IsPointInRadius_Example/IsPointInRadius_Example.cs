using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    public class IsPointInRadius_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject TargetCenter;
        public float CircleRadius;

        [SerializeField] GameObject Point;

        void Update()
        {
            var newpos = new Vector3(Mathf.PingPong(Time.unscaledTime * 8, CircleRadius * 2.4f) - CircleRadius*1.2f,0,0);
            Point.transform.position = newpos;
        }
        void OnGUI()
        {
            //Convert World Pos to GUI Pos
            Vector3 GUIcenterpos = cam.WorldToScreenPoint(TargetCenter.transform.position);
            Vector3 GUIpointpos = cam.WorldToScreenPoint(Point.transform.position);
            GUIcenterpos.y = UnityOnGUIHelper.Height - GUIcenterpos.y;
            GUIpointpos.y = UnityOnGUIHelper.Height - GUIpointpos.y;
            Vector3 radius = cam.WorldToScreenPoint(TargetCenter.transform.position + new Vector3(CircleRadius, 0, 0));
            radius.y = UnityOnGUIHelper.Height - radius.y;

            var GUICircleRadius = Vector3.Distance(GUIcenterpos, radius);

            int IsPointInRadius = GeometryUtils.IsPointInRadius(GUIcenterpos, GUICircleRadius, GUIpointpos);
            if (IsPointInRadius == 1) Text.text = "Point Inside Center Radius";
            else if (IsPointInRadius == -1) Text.text = "Point Outside Center Radius";
            else Text.text = "Point On Center Radius";

            UnityOnGUIHelper.DrawEllipse(GUIcenterpos, new Vector2(GUICircleRadius, GUICircleRadius), 0, new Color32(0, 255, 0,255), Color.red, 1);
        }
    }
}
