using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.Services
{
    public class GeoinformationModelingService : IGeoinformationModelingService
    {
        private const int maxMatrixElements = 500000;
        private const int maxWidthMatrix = 10000;
        private const int segmentLength = 1;
        private const int breakSegmentsNumber = 40;
        private double[] rightPart;

        public GeoinformationModelingService()
        {
            rightPart = new double[maxWidthMatrix];
        }
        public double BusinessU(double x)
        {
            return 1;
        }

        public double Fi(double i, double x)
        {
            double x1, x2, x3;
            if (x < 0 || x == segmentLength)
            {
                return 0;
            }
            else
            {
                x1 = segmentLength * (i - 1) / (double)breakSegmentsNumber;
                x2 = segmentLength * i / (double)breakSegmentsNumber;
                x3 = segmentLength * (i + 1) / (double)breakSegmentsNumber;
                if ((x >= x1) && (x <= x2))
                {
                    return (x - x1) * breakSegmentsNumber / segmentLength;
                }
                else if ((x >= x2) && (x <= x3))
                {
                    return (x3 - x) * breakSegmentsNumber / segmentLength;
                }
            }
            return 0;
        }

        public double Differential_fi(int i, double x)//dfi
        {
            double x1, x2, x3;
            if (x < 0 || x == segmentLength)
            {
                return 0;
            }
            else
            {
                x1 = segmentLength * (i - 1) / (double)breakSegmentsNumber;
                x2 = segmentLength * i / (double)breakSegmentsNumber;
                x3 = segmentLength * (i + 1) / (double)breakSegmentsNumber;
                if ((x >= x1) && (x <= x2))
                {
                    return breakSegmentsNumber / segmentLength;
                }
                else if ((x >= x2) && (x <= x3))
                {
                    return -breakSegmentsNumber / segmentLength;
                }
            }
            return 0;
        }

        public double Fj (double x)//Fj
        {
            int num;
            double kr;
            if(x < 0 || x > segmentLength)
            {
                throw new ArgumentException();
            }
            else
            {
                kr = segmentLength / breakSegmentsNumber;
                num = (int)(x / kr);
                if(num >= breakSegmentsNumber)
                {
                    num = breakSegmentsNumber - 1;
                }
            }
            return rightPart[2 * num] * Fi(num, x) + rightPart[2 * (num + 1)] * Fi(num + 1, x);
        }

        public double Uj(double x)//Uj
        {
            int num;
            double kr;
            if (x < 0 || x > segmentLength)
            {
                throw new ArgumentException();
            }
            else
            {
                kr = segmentLength / breakSegmentsNumber;
                num = (int)(x / kr);
                if (num >= breakSegmentsNumber)
                {
                    num = breakSegmentsNumber - 1;
                }
            }
            return rightPart[2 * num + 1] * Fi(num, x) + rightPart[2 * (num + 1) + 1] * Fi(num + 1, x);
        }

        public double Differential_Fj(int i, double x)//dFj
        {
            int num;
            double kr;
            if (x < 0 || x > segmentLength)
            {
                throw new ArgumentException();
            }
            else
            {
                kr = segmentLength / breakSegmentsNumber;
                num = (int)(x / kr);
                if (num >= breakSegmentsNumber)
                {
                    num = breakSegmentsNumber - 1;
                }
            }
            return rightPart[2 * num] * Differential_fi(num, x) + rightPart[2 * (num + 1)] * Differential_fi(num + 1, x);
        }

        public double Differential_Uj(int i, double x)//dUj
        {
            int num;
            double kr;
            if (x < 0 || x > segmentLength)
            {
                throw new ArgumentException();
            }
            else
            {
                kr = segmentLength / breakSegmentsNumber;
                num = (int)(x / kr);
                if (num >= breakSegmentsNumber)
                {
                    num = breakSegmentsNumber - 1;
                }
            }
            return rightPart[2 * num + 1] * Differential_fi(num, x) + rightPart[2 * (num + 1) + 1] * Differential_fi(num + 1, x);
        }
    }
}
