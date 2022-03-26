using UnityEngine;
using System.Collections;

    public static class TransformHelper
    {

        //找到FindChild 放在这里 递归！
        public static Transform FindChildByName(this Transform trans, string goName)
        {
            Transform child = trans.Find(goName);
            if (child != null)
                return child;

            Transform go;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindChildByName(child, goName);
                if (go != null)
                    return go;
            }
            return null;
        }
        /// <summary>
        /// 转向
        /// </summary>
        public static void LookAtTarget(Vector3 target,
            Transform transform, float rotationSpeed)
        {
            if (target != Vector3.zero)
            {
                Quaternion dir = Quaternion.LookRotation(target);
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    dir, rotationSpeed);
            }
        }
    }
