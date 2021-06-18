using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CameraSpace
{
    public class Camera_Menu : MonoBehaviour
    {
        [MenuItem("UATanks/Cameras/Top Down Camera")]
        public static void CreateTopDownCamera()
        {
            GameObject[] selectedGO = Selection.gameObjects;

            if (selectedGO.Length > 0 && selectedGO[0].GetComponent<TopDown_Camera>())
            {
                if (selectedGO.Length < 2)
                {
                    AttachTopDownScript(selectedGO[0].gameObject, null);
                }
                else if (selectedGO.Length == 2)
                {
                    AttachTopDownScript(selectedGO[0].gameObject, selectedGO[1].transform);
                }
                else if (selectedGO.Length == 3)
                {
                    EditorUtility.DisplayDialog("Camera Tools", "You can only select two GameObjects in the scene " +
                        "for this to work, the first selection needs to be a camera!", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Camera Tools", "You need to select a GameObject in the scene " +
                    "that has a Camera component assigned to it!", "OK");
            }
        }

        static void AttachTopDownScript(GameObject aCamera, Transform aTarget)
        {
            TopDown_Camera cameraScript = null;
            if (aCamera)
            {
                cameraScript = aCamera.AddComponent<TopDown_Camera>();


                if (cameraScript && aTarget)
                {
                    cameraScript.m_Target = aTarget;
                }
                Selection.activeObject = aCamera;
            }
        }
    }
}
