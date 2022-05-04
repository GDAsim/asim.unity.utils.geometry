using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    public class IsPointInRect_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject TargetCenter;
        public Vector2 Size = Vector2.one;

        [SerializeField] GameObject Point;

        float rotation = 0;

        void OnGUI()
        {
            //Convert World Pos to GUI Pos
            Vector3 GUIcenterpos = cam.WorldToScreenPoint(TargetCenter.transform.position);
            Vector3 GUIpointpos = cam.WorldToScreenPoint(Point.transform.position);
            GUIcenterpos.y = UnityOnGUIHelper.Height - GUIcenterpos.y;
            GUIpointpos.y = UnityOnGUIHelper.Height - GUIpointpos.y;

            Vector3 sizeX = cam.WorldToScreenPoint(TargetCenter.transform.position + new Vector3(Size.x, 0, 0));
            Vector3 sizeY = cam.WorldToScreenPoint(TargetCenter.transform.position + new Vector3(0, Size.y, 0));
            sizeX.y = UnityOnGUIHelper.Height - sizeX.y;
            sizeY.y = UnityOnGUIHelper.Height - sizeY.y;
            var GUISize = new Vector2(Vector3.Distance(GUIcenterpos, sizeX), Vector3.Distance(GUIcenterpos, sizeY));

            rotation += 0.003f;

            int IsPointInRect = GeometryUtils.IsPointInRectOrientation(GUIcenterpos, GUISize, rotation, GUIpointpos);
            if (IsPointInRect == 1) Text.text = "Point Inside Rect";
            else if (IsPointInRect == -1) Text.text = "Point Outside Rect";
            else Text.text = "Point On Rect";

            UnityOnGUIHelper.DrawRect(GUIcenterpos, GUISize, rotation, new Color32(0,0,0,0), Color.black,true, 1);
        }
    }
}
