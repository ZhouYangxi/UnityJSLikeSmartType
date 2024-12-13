using System;
using UnityEngine;

namespace OrbitGames
{
    public class C3SmartObjectTest : MonoBehaviour
    {
        private static C3SmartObject global_val_voiceDbVolume = "30";
        
        private void Start()
        {
            Test1(1);
            Test2();
            Test3();
        }

        public static void Test1(double param_extraDBVolume)
        {
            var volume = global_val_voiceDbVolume + param_extraDBVolume;
            Debug.Log($"NumberValue = {(double)volume}, ToString = {volume}");
        }
        
        public static void Test2()
        {
            double a = 3.0 + 5.0;
            double dt = 2;

            var pow = a ^ (C3SmartObject)dt;
            
            Debug.Log($"NumberValue = {(double)pow}, ToString = {pow}");
        }

        public static void Test3()
        {
            var yes = global_val_voiceDbVolume ? 1 : 0;
            Debug.Log($"Result = {yes}");
        }
    }
}