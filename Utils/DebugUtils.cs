using System;
using UnityEngine.UI;

namespace Utils
{
    class DebugUtils
    {
        static public void LogButtonSubscribers(Button myButton)
        {
            int scriptCount = myButton.onClick.GetPersistentEventCount();
            for (int i = 0; i < scriptCount; i++)
            {
                UnityEngine.Object target = myButton.onClick.GetPersistentTarget(i);
                string methodName = myButton.onClick.GetPersistentMethodName(i);

                if (target != null)
                {
                    Type type = target.GetType();
                    Console.WriteLine("Script #" + i + ": " + type.FullName + " - " + methodName);
                }
                else
                {
                    Console.WriteLine("Script #" + i + ": No target object found - " + methodName);
                }
            }
        }
    }
}