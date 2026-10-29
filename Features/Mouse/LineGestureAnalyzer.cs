// ============================================================================
// LineGestureAnalyzer.cs
// ----------------------------------------------------------------------------
// 画线手势分析：判断轨迹方向（上/下/左/右）与是否闭环圆圈。
// 输入：ScreenLineDrawer 采集的轨迹点（屏幕坐标，List<Point>）。
// ============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;

namespace keyupMusic4
{
    /// <summary>画线手势分析器（纯静态，无状态）。</summary>
    public static class LineGestureAnalyzer
    {
        /// <summary>手势方向。</summary>
        public enum Direction { None, Up, Down, Left, Right }

        // 判定阈值
        public const int MinGestureDistance = 30;       // 首尾位移最小像素（小于视为无效）
        public const double MinPathLength = 60;         // 总路径最小长度（排除原地抖动）
        public const double CircleMinPathLength = 120;  // 闭环最少路径长度
        public const double CloseRatio = 0.2;           // 首尾距离占路径长度的最大比例（视为闭合）
        public const int CircleMinSize = 40;            // 闭环包围盒最小宽/高（像素）
        public const double CircleMinAngle = 270;       // 轨迹相对质心的角度覆盖最小值（度）

        /// <summary>判断轨迹整体方向（上/下/左/右）。按首尾位移主轴判断；首尾太近视为无效。</summary>
        public static Direction GetDirection(List<Point> points)
        {
            if (points == null || points.Count < 2) return Direction.None;

            // 总路径长度（排除原地抖动）
            if (TotalLength(points) < MinPathLength) return Direction.None;

            var first = points[0];
            var last = points[points.Count - 1];
            double dx = last.X - first.X;
            double dy = last.Y - first.Y;

            // 首尾位移太小：可能画了又画回去（如圆圈闭合），视为无效方向
            if (Math.Abs(dx) < MinGestureDistance && Math.Abs(dy) < MinGestureDistance)
                return Direction.None;

            if (Math.Abs(dx) >= Math.Abs(dy))
                return dx > 0 ? Direction.Right : Direction.Left;
            return dy > 0 ? Direction.Down : Direction.Up;
        }

        /// <summary>判断轨迹是否为闭环圆圈：路径够长 + 首尾接近 + 包围盒够大 + 角度覆盖 ≥ 270°。</summary>
        public static bool IsClosedCircle(List<Point> points)
        {
            if (points == null || points.Count < 8) return false;

            double pathLength = TotalLength(points);
            if (pathLength < CircleMinPathLength) return false;

            // 首尾必须接近（闭合）
            double endDist = Distance(points[0], points[points.Count - 1]);
            if (endDist > pathLength * CloseRatio) return false;

            // 包围盒不能太扁（宽度/高度都要够）
            var rect = BoundingBox(points);
            if (rect.Width < CircleMinSize || rect.Height < CircleMinSize) return false;

            // 轨迹相对质心的角度覆盖 ≥ 270°（真绕了一圈）
            return AngleCoverage(points) >= CircleMinAngle;
        }

        /// <summary>总路径长度（逐段累加）。</summary>
        public static double TotalLength(List<Point> points)
        {
            double len = 0;
            for (int i = 1; i < points.Count; i++)
                len += Distance(points[i - 1], points[i]);
            return len;
        }

        /// <summary>轨迹包围盒。</summary>
        public static Rectangle BoundingBox(List<Point> points)
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            foreach (var p in points)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }

        /// <summary>轨迹点相对质心的极角覆盖范围（0~360°，度量轨迹是否绕了一圈）。</summary>
        public static double AngleCoverage(List<Point> points)
        {
            if (points == null || points.Count == 0) return 0;

            double cx = 0, cy = 0;
            foreach (var p in points) { cx += p.X; cy += p.Y; }
            cx /= points.Count;
            cy /= points.Count;

            var angles = new List<double>(points.Count);
            foreach (var p in points)
            {
                double a = Math.Atan2(p.Y - cy, p.X - cx) * 180 / Math.PI; // -180~180
                if (a < 0) a += 360;
                angles.Add(a);
            }
            angles.Sort();

            // 最大间隙 = 360 - 覆盖范围
            double maxGap = 0;
            for (int i = 1; i < angles.Count; i++)
            {
                double gap = angles[i] - angles[i - 1];
                if (gap > maxGap) maxGap = gap;
            }
            double wrapGap = angles[0] + 360 - angles[angles.Count - 1];
            if (wrapGap > maxGap) maxGap = wrapGap;

            return 360 - maxGap;
        }

        /// <summary>方向的中文显示。</summary>
        public static string DirectionText(Direction d)
        {
            if (d == Direction.Up) return "上";
            if (d == Direction.Down) return "下";
            if (d == Direction.Left) return "左";
            if (d == Direction.Right) return "右";
            return "";
        }

        private static double Distance(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
