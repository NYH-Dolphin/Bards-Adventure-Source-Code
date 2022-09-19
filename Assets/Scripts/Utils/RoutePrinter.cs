using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    /// <summary>
    /// 路线可视化绘制
    /// </summary>
    public class RoutePrinter : MonoBehaviour
    {

        // 注意，List 中的每个 GameObject 都是一组路径的父物体
        public List<GameObject> objRoutes; // 需要绘制的路径List

        private List<Color> colorList = new List<Color>();
        

        private void OnDrawGizmos()
        {
            if (colorList.Count == 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Color color = Random.ColorHSV();
                    colorList.Add(color);
                }
            }

            for (int i = 0; i < objRoutes.Count; i++)
            {
                Transform[] r = objRoutes[i].transform.GetComponentsInChildren<Transform>();
                r[0] = r[1];
                iTween.DrawPath(r, colorList[i]);
            }
        }
    }
}