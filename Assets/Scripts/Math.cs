using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clusternoid
{
    /// <summary>
    /// 클러스터노이드 개발중 자주 쓰일 수학적 함수를 담아놓은 곳
    /// </summary>
    public class Math : MonoBehaviour
    {

        /// <summary>
        /// from 과 to 사이의 각도를 Quaternion으로 반환해 준다.
        /// </summary>
        /// <returns>from과 to 사이의 각도 Quaternion</returns>
        public static Quaternion RotationAngle(Vector2 from, Vector2 to)
        {
            Vector2 from2to = to - from;
            from2to.Normalize();
            float rot_z = Mathf.Atan2(from2to.y, from2to.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, 0f, rot_z - 90);
        }

    }

}
