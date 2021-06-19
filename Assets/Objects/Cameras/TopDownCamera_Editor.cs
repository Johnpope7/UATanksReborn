//credits to Indie Pixel for the tutorial on this script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CameraSpace
{
    [CustomEditor(typeof(TopDown_Camera))]
    public class TopDownCamera_Editor : Editor
    {
        #region Variable
        private TopDown_Camera targetCamera; //stores the camera
        #endregion

        #region Custom Methods
        void OnEnable()
        {
            targetCamera = (TopDown_Camera)target;
        }

        public override void OnInspectorGUI() //just in case i want to edit the GUI later to include parts of this tool
        {
            base.OnInspectorGUI();
        }

        void OnScreenGUI()
        {
            if (!targetCamera.m_Target)  //checks for camera on target
            {
                return;
            }

            //this stores the target reference
            Transform camTarget = targetCamera.m_Target;

            //this creates the distance disc
            Handles.color = new Color(1f, 0f, 0f, 0.15f);
            Handles.DrawSolidDisc(targetCamera.m_Target.position, Vector3.up, targetCamera.m_Distance);

            Handles.color = new Color(1f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(targetCamera.m_Target.position, Vector3.up, targetCamera.m_Distance);

            //creates slider handles to adjust the cameras properties
            Handles.color = new Color(1f, 0f, 0f, 0.5f);
            targetCamera.m_Distance = Handles.ScaleSlider(targetCamera.m_Distance, camTarget.position, -camTarget.forward, Quaternion.identity, targetCamera.m_Distance, 1f);
            targetCamera.m_Distance = Mathf.Clamp(targetCamera.m_Height, 10f, float.MaxValue);

            Handles.color = new Color(0f, 0f, 1f, 0.5f);
            targetCamera.m_Height = Handles.ScaleSlider(targetCamera.m_Height, camTarget.position, -camTarget.forward, Quaternion.identity, targetCamera.m_Height, 1f);
            targetCamera.m_Height = Mathf.Clamp(targetCamera.m_Height, 10f, float.MaxValue);

            //This creates all the labels
            GUIStyle labelStyle = new GUIStyle(); //my GUI stored style
            labelStyle.fontSize = 15;  //the size of the font
            labelStyle.normal.textColor = Color.white; //the color of the texts
            labelStyle.alignment = TextAnchor.UpperCenter; //the alignment of the texts in the box
            Handles.Label(camTarget.position + (-camTarget.forward * targetCamera.m_Distance), "Distance", labelStyle); //creates label for Distance
            Handles.Label(camTarget.position + (Vector3.up * targetCamera.m_Height), "Height"); //creates label for Height
        }
        #endregion
    }
}
