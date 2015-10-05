using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLF
{
    public static class Mathf
    {
        public static double clamp(double value, double min, double max)
        {
            return value > max ? max : (value < min ? min : value);
        }

        public static double Distance(double value1, double value2)
        {
            return Math.Abs(value1 - value2);
        }

        public static double hermite(double value1, double tangent1, double value2, double tangent2, double amount)
        {
            // All transformed to double not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            double sCubed = s * s * s;
            double sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2.0f * v1 - 2.0f * v2 + t2 + t1) * sCubed +
                    (3.0f * v2 - 3.0f * v1 - 2.0f * t1 - t2) * sSquared +
                    t1 * s +
                    v1;
            return (double)result;
        }


        public static double lerp(double value1, double value2, double amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static double max(double value1, double value2)
        {
            return Math.Max(value1, value2);
        }

        public static double min(double value1, double value2)
        {
            return Math.Min(value1, value2);
        }

        public static double smoothStep(double value1, double value2, double amount)
        {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            double result = Mathf.clamp(amount, 0f, 1f);
            result = Mathf.hermite(value1, 0f, value2, 0f, result);
            return result;
        }

        public static double toDegrees(double radians)
        {
            // This method uses double precission internally,
            // though it returns single double
            // Factor = 180 / pi
            return (double)(radians * 57.295779513082320876798154814105);
        }

        public static double toRadians(double degrees)
        {
            // This method uses double precission internally,
            // though it returns single double
            // Factor = pi / 180
            return (double)(degrees * 0.017453292519943295769236907684886);
        }

        public static double wrapAngle(double angle)
        {
            angle = (double)Math.IEEERemainder((double)angle, 6.2831854820251465);
            if (angle <= -3.141593f)
            {
                angle += 6.283185f;
                return angle;
            }
            if (angle > 3.141593f)
            {
                angle -= 6.283185f;
            }
            return angle;
        }

        public static bool intersect(Rect p1, Rect p2)
        {
            if (p1.x > p2.x && p1.x < p2.width && p1.y > p2.y && p1.y < p2.height)
                return true;
            else
                return false;
        }

        public static bool intersect(double x1, double y1, double w1, double h1, double x2, double y2, double w2, double h2)
        {
            if (x1 >= x2 && x1 <= w2 && w1 <= w2 && y1 >= y2 && y1 <= h2 && h1 <= h2)
                return true;
            else
                return false;
        }

        public static double roundTo(double value, double step)
        {
            return Math.Floor((value + step / 2) / step) * step;
        }

        public static double lookAt(Vector from, Vector to)
        {
            return -Math.Atan2(to.x - from.x, to.y - from.y) * 180 / Math.PI;
        }

        public static Vector rotateAround(Vector p, Vector o, float a)
        {
            double X = o.x + (p.x - o.x) * Math.Cos(a) - (p.y - o.y) * Math.Sin(a);
            double Y = o.y + (p.y - o.y) * Math.Cos(a) + (p.x - o.x) * Math.Sin(a);

            return new Vector((float)X, (float)Y);
        }

        public static Vector rotate(Vector p, float a)
        {
            double X = p.x * Math.Cos(a) - p.y * Math.Sin(a);
            double Y = p.y * Math.Cos(a) + p.x * Math.Sin(a);

            return new Vector((float)X, (float)Y);
        }

        public static float roundDegrees(float angle)
        {
            float _angle = angle;

            if (_angle >= 360)
                _angle -= 360;
            if (_angle <= -360)
                _angle += 360;

            return _angle;
        }
    }

    public static class Random
    {
        static System.Random r = new System.Random();

        public static double range(double min, double max)
        {
            return r.NextDouble() * (max - min) + min;
        }
    }
}