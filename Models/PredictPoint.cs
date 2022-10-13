using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Models
{
    public class PredictPoint
    {
        public Vector2D Point { get; set; }

        public float Weight { get; set; }

        public PredictPoint()
        {

        }

        public override string ToString()
        {
            return $"预测点：{Point}，权重：{Weight}";
        }
    }
}
