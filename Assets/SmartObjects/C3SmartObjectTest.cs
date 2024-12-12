using UnityEngine;

namespace OrbitGames
{
    public class C3SmartObjectTest
    {
        public static void CallFuncts()
        {
            C3SmartObject a = 0.0;
            C3SmartObject b = "0";

            C3SmartObject c = "0" + a + b;

            var e = "0";
            var d = a == b;
            var g = e == b;
        }

        private static void CallFuncts2(double a, string b)
        {
            
        }
    }
}