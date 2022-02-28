using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TrafficSystem
{

    public class SpeedSystemManager : MonoBehaviour
    {
        public static SpeedSystemManager Instance;

        [SerializeField] [Range(.01f, 1f)] float ratioUnitPer10km;

        private void Awake()
        {

            if (!Instance)
            {
                Instance = this;
            }
            else Destroy(this);
        }

        public float KmToUnitSpeed(float kmSpeed)
        {
            float value = kmSpeed * ratioUnitPer10km;
            return value;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(SpeedSystemManager))]
    public class SpeedSystemManagerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SpeedSystemManager manager = (SpeedSystemManager)target;

            EditorGUILayout.LabelField($"20km/h   -->   {manager.KmToUnitSpeed(20).ToString("F2")}", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField($"40km/h   -->   {manager.KmToUnitSpeed(40).ToString("F2")}", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField($"60km/h   -->   {manager.KmToUnitSpeed(60).ToString("F2")}", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField($"100km/h  -->   {manager.KmToUnitSpeed(100).ToString("F2")}", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField($"120km/h  -->   {manager.KmToUnitSpeed(120).ToString("F2")}", EditorStyles.centeredGreyMiniLabel);

        }
    }
#endif

}