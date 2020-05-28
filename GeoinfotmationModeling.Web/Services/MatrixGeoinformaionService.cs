using GeoinformationModeling.Web.Entities;
using GeoinformationModeling.Web.Models;
using GeoinformationModeling.Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Services
{
    public class MatrixGeoinformaionService : IMatrixGeoinformaionService
    {
        private double[] FF;
        private double[] CopiedFF;
        private double[] KvadrX;
        private double[] KvadrC;
        private double[] MatrC;
        private double[] Fun;
        private int matrixLenth;
        private int matrixWidth;
        private double time;
        private double timeIncrease;
        private const double beta = 0.5;
        private const double Gr_umF = 0; //Гранична умова на функцію F(x= 0)
        private const double Gr_umU = 0; //Гранична умова на функцію U(x= 0)
        private const double mnoz = 1E+40;
        private const double buf = 1.0E-16;


        public MatrixGeoinformaionService()
        {
            KvadrX = new double[4];
            KvadrC = new double[4];
            KvadrX[0] = -0.86113; KvadrX[1] = -0.33998;
            KvadrX[2] = 0.33998; KvadrX[3] = 0.86113;

            KvadrC[0] = 0.34785; KvadrC[1] = 0.65214;
            KvadrC[2] = 0.65214; KvadrC[3] = 0.34785;
            FF = new double[10000];
            CopiedFF = new double[10000];
            Fun = new double[10000];
            MatrC = new double[500000];
        }
        public double GetLengthScale(double width, double height, double maxLength)
        {
            return height * width / (maxLength / 2);
        }
        public MatrixesModel GetMatrixes(GeoinformationModelingViewModel geoinformationModel)
        {
            var allLength = geoinformationModel.PiverParams.LengthByX;
            var mapDetails = new List<MapDetail>();
            matrixLenth = 2 * (geoinformationModel.PiverParams.SplitsNumberByX + 1);
            matrixWidth = 4;
            int j = geoinformationModel.PiverParams.SplitsNumberByX;

            double[,] matrixF = new double[(int)(geoinformationModel.PiverParams.SplitsNumberByT / geoinformationModel.PiverParams.OutputMultiplicityByT),
                    (geoinformationModel.PiverParams.SplitsNumberByX / geoinformationModel.PiverParams.OutputMultiplicityByX) + 1];
            double[,] matrixU = new double[(int)(geoinformationModel.PiverParams.SplitsNumberByT / geoinformationModel.PiverParams.OutputMultiplicityByT),
                 (geoinformationModel.PiverParams.SplitsNumberByX / geoinformationModel.PiverParams.OutputMultiplicityByX) + 1];
            var isFirst = true;
            //geoinformationModel.Lengthes = new List<double> { 2 };
            foreach (var pieceLength in geoinformationModel.Lengthes)
            {
                geoinformationModel.PiverParams.LengthByX = GetLengthScale(geoinformationModel.TaskParams.FreeSurfaceWidth, pieceLength, allLength);



                time = 0;
                timeIncrease = geoinformationModel.PiverParams.LengthByT / geoinformationModel.PiverParams.SplitsNumberByT;
                if (isFirst == true)
                {
                    Format(geoinformationModel);
                }


                int numRow = 0;
                for (int i = 1; i <= geoinformationModel.PiverParams.SplitsNumberByT; i++)
                {
                    time = time + timeIncrease;
                    CopyFF();
                    FormatMatr(time - timeIncrease / 2, geoinformationModel.PiverParams, geoinformationModel.TaskParams);
                    Deco(matrixLenth, matrixWidth, MatrC);
                    Solve(matrixLenth, matrixWidth, MatrC, FF);
                    BuildFF();

                    if (i % geoinformationModel.PiverParams.OutputMultiplicityByT == 0)
                    {
                        getMatrixFF(geoinformationModel, matrixF, matrixU, numRow);
                        numRow++;
                    }

                }
                CopyFF();
                isFirst = false;
                mapDetails.Add(new MapDetail
                {
                    Area = matrixF[matrixF.GetLength(0) - 1, matrixF.GetLength(1) - 1],
                    Sppeed = matrixU[matrixU.GetLength(0) - 1, matrixU.GetLength(1) - 1],
                    MatrixH = (double[,])matrixF.Clone(),
                    MatrixU = (double[,])matrixU.Clone()
                });
            }


            return new MatrixesModel { MatrixH = matrixF, MatrixU = matrixU, MapDetails = mapDetails };
        }
        public void getMatrixFF(GeoinformationModelingViewModel geoinformationModel, double[,] matrixF, double[,] matrixU, int numRow)
        {
            int counter = 0;
            for (int i = 0; i <= geoinformationModel.PiverParams.SplitsNumberByX; i++)
            {
                if (i % geoinformationModel.PiverParams.OutputMultiplicityByX == 0)
                {
                    matrixF[numRow, counter] = FF[2 * i];
                    matrixU[numRow, counter] = FF[2 * i + 1];
                    counter++;
                }
            }
        }
        private void CopyFF()
        {
            for (int i = 0; i < FF.Length; i++)
            {
                CopiedFF[i] = FF[i];
            }
        }

        private void BuildFF()
        {
            for (int i = 0; i < FF.Length; i++)
            {
                FF[i] = CopiedFF[i] + timeIncrease * FF[i];
            }
        }

        private void Format(GeoinformationModelingViewModel geoinformationModel)
        {
            for (int i = 0; i <= geoinformationModel.PiverParams.SplitsNumberByX; i++)
            {
                FF[2 * i] = F0(i * geoinformationModel.PiverParams.LengthByX / (double)geoinformationModel.PiverParams.SplitsNumberByX, geoinformationModel);
                FF[2 * i + 1] = U0(i * geoinformationModel.PiverParams.LengthByX / (double)geoinformationModel.PiverParams.SplitsNumberByX, geoinformationModel);
            }
        }

        private void FormatMatr(double tim, RiverParams riverModel, TaskParams taskParams)
        {
            double length = riverModel.LengthByX / riverModel.SplitsNumberByX;
            double x1, x2, xkv, zn, iser;

            for (int i = 0; i < Fun.Length; i++)
            {
                Fun[i] = 0;
            }

            for (int i = 0; i < MatrC.Length; i++)
            {
                MatrC[i] = 0;
            }

            for (int i = 0; i <= riverModel.SplitsNumberByX - 1; i++)
            {
                x1 = i * riverModel.LengthByX / riverModel.SplitsNumberByX;
                x2 = (i + 1) * riverModel.LengthByX / riverModel.SplitsNumberByX;

                if (i < (riverModel.SplitsNumberByX - 1) / 2)
                {
                    iser = taskParams.AngleOfInclinationSine;
                }
                else
                {
                    iser = taskParams.AngleSine;
                }
                for (int it = 0; it <= 3; it++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        xkv = (x1 + x2) / 2 + KvadrX[it] * (x2 - x1) / 2;
                        for (int k = 0; k <= 1; k++)
                        {
                            zn = length * KvadrC[it] * (Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) +
                                beta * timeIncrease * (Uj(xkv, riverModel) * dFi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) +
                                dUj(xkv, riverModel) * Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel)));

                            MatrC[GetNum(2 * (i + k), 2 * (i + j))] = MatrC[GetNum(2 * (i + k), 2 * (i + j))] + zn;

                            zn = beta * timeIncrease * length * KvadrC[it] * (Fj(xkv, riverModel) * dFi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) +
                                dFj(xkv, riverModel) * Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel));

                            MatrC[GetNum(2 * (i + k), 2 * (i + j) + 1)] = MatrC[GetNum(2 * (i + k), 2 * (i + j) + 1)] + zn;

                            zn = length * KvadrC[it] * (beta * timeIncrease * dFi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) / taskParams.FreeSurfaceWidth + ((taskParams.Alpha - 1) * (Uj(xkv, riverModel) /
                                Fj(xkv, riverModel)) * Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel)) / taskParams.GravityAcceleration);

                            MatrC[GetNum(2 * (i + k) + 1, 2 * (i + j))] = MatrC[GetNum(2 * (i + k) + 1, 2 * (i + j))] + zn;

                            zn = length * KvadrC[it] * (Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) / taskParams.GravityAcceleration +
                                beta * timeIncrease * (taskParams.Alpha / taskParams.GravityAcceleration * dUj(xkv, riverModel) * Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) +
                                1 / taskParams.GravityAcceleration * Uj(xkv, riverModel) * dFi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel) +
                                (2 / (taskParams.ShaziCoefficient * taskParams.ShaziCoefficient * taskParams.ChannelHydraulicRadius)) * Uj(xkv, riverModel) * Fi(i + j, xkv, riverModel) * Fi(i + k, xkv, riverModel)));


                            MatrC[GetNum(2 * (i + k) + 1, 2 * (i + j) + 1)] = MatrC[GetNum(2 * (i + k) + 1, 2 * (i + j) + 1)] + zn;
                        }

                        zn = -length * KvadrC[it] * (Fj(xkv, riverModel) * dUj(xkv, riverModel) * Fi(i + j, xkv, riverModel) +
                            dFj(xkv, riverModel) * Uj(xkv, riverModel) * Fi(i + j, xkv, riverModel));
                        Fun[2 * (i + j)] = Fun[2 * (i + j)] + zn;
                        zn = -length * KvadrC[it] * (dFj(xkv, riverModel) * Fi(i + j, xkv, riverModel) / taskParams.FreeSurfaceWidth + taskParams.Alpha / taskParams.GravityAcceleration * Uj(xkv, riverModel) *
                            dUj(xkv, riverModel) * Fi(i + j, xkv, riverModel) - iser * Fi(i + j, xkv, riverModel) + Uj(xkv, riverModel) * Uj(xkv, riverModel) * Fi(i + j, xkv, riverModel) /
                            (taskParams.ShaziCoefficient * taskParams.ShaziCoefficient * taskParams.ChannelHydraulicRadius));

                        Fun[2 * (i + j) + 1] = Fun[2 * (i + j) + 1] + zn;

                    }

                }


            }
            for (int i = 0; i < Fun.Length; i++)
            {
                FF[i] = Fun[i];
            }

            MatrC[GetNum(0, 0)] = mnoz;
            MatrC[GetNum(1, 1)] = mnoz;
            FF[0] = Gr_umF * mnoz;
            FF[1] = Gr_umU * mnoz;
        }

        private void Deco(int nv, int ng, double[] stif)
        {
            int k, l1, m1, m2;
            double di;
            m1 = ng;
            k = ng - 1;

            for (int i = 2; i <= nv; i++)
            {
                if (i > (nv - ng + 3))
                {
                    k = k - 1;
                }
                m2 = m1;
                //if (Math.Abs(stif[m1 - 1]) <= buf)
                //{
                //    throw new ArgumentException();
                //}
                di = -1 / stif[m1 - 1];
                for (int j = 1; j <= k; j++)
                {
                    m2 = m2 + (2 * ng - 1) - 1;
                    stif[m2 - 1] = stif[m2 - 1] * di;
                    for (int l = 1; l <= ng - 1; l++)
                    {
                        l1 = l - 1;

                        stif[m2 + l1] = stif[m2 + l1] + stif[m1 + l1] * stif[m2 - 1];
                    }
                    m1 = m1 + (2 * ng - 1);
                }
            }
        }
        private void Solve(int nv, int ng, double[] stif, double[] f)
        {
            int k, l1, m1, m2, kk, ii, jj;
            double s, r;
            double di;
            m2 = 3 * ng - 2;
            kk = ng - 1;
            for (int i = 1; i <= nv - 1; i++)
            {
                ii = i - 1;
                if (i >= (nv - ng + 2))
                {
                    kk = kk - 1;
                }
                for (int j = 1; j <= kk; j++)
                {
                    jj = j - 1;
                    f[i + jj] = stif[m2 + (2 * ng - 2) * (j - 1) - 1] * f[ii] + f[i + jj];

                }
                m2 = m2 + (2 * ng - 1);
            }
            f[nv - 1] = f[nv - 1] / stif[nv * (2 * ng - 1) - ng];
            m1 = (nv - 1) * (2 * ng - 1) - ng + 2;
            m2 = nv - 1;
            kk = 0;
            for (int i = 1; i <= nv - 1; i++)
            {
                s = 0;
                if (i <= ng - 1)
                {
                    kk = kk + 1;
                }
                for (int j = 1; j <= kk; j++)
                {
                    m2 = m2 + 1;
                    s = s + stif[m1 - 1] * f[m2 - 1];
                    m1 = m1 + 1;
                }
                m2 = m2 - kk;
                m1 = m1 - kk;
                r = f[m2 - 1] - s;

                if (Math.Abs(stif[m1 - 2]) <= buf && (Math.Abs(r) <= buf))
                {
                    f[m2 - 1] = 0;
                }
                f[m2 - 1] = r / stif[m1 - 2];
                m2 = m2 - 1;
                m1 = m1 - (2 * ng - 1);
            }
        }
        private int GetNum(int i, int j)
        {
            return (2 * matrixWidth - 1) * i + (matrixWidth - i + j) - 1;
        }
        private double F0(double x, GeoinformationModelingViewModel geoinformationModel)
        {
            double res = 0;
            switch (geoinformationModel.TestExample)
            {
                case TestExample.First:
                case TestExample.Second:
                    res = x;
                    break;
                case TestExample.Third:
                    res = x * x;
                    break;
                default:
                    res = Math.Sqrt(x);
                    break;

            }
            return res;

        }

        private double Fi(int i, double x, RiverParams riverModel)
        {
            double x1, x2, x3;
            x1 = riverModel.LengthByX * (i - 1) / riverModel.SplitsNumberByX;
            x2 = riverModel.LengthByX * i / riverModel.SplitsNumberByX;
            x3 = riverModel.LengthByX * (i + 1) / riverModel.SplitsNumberByX;
            if ((x >= x1) && (x <= x2))
            {
                return (x - x1) * riverModel.SplitsNumberByX / riverModel.LengthByX;
            }
            else if ((x >= x2) && (x <= x3))
            {
                return (x3 - x) * riverModel.SplitsNumberByX / riverModel.LengthByX;
            }
            return 0;
        }

        private double Fj(double x, RiverParams riverModel)
        {
            double kr = riverModel.LengthByX / riverModel.SplitsNumberByX;
            int nom = (int)(x / kr);
            return FF[2 * nom] * Fi(nom, x, riverModel) + FF[2 * (nom + 1)] * Fi(nom + 1, x, riverModel);
        }

        private double dFj(double x, RiverParams riverModel)
        {
            double kr = riverModel.LengthByX / riverModel.SplitsNumberByX;
            int nom = (int)(x / kr);
            return FF[2 * nom] * dFi(nom, x, riverModel) + FF[2 * (nom + 1)] * dFi(nom + 1, x, riverModel);
        }
        private double dFi(int i, double x, RiverParams riverModel)
        {
            double x1, x2, x3;
            x1 = riverModel.LengthByX * (i - 1) / riverModel.SplitsNumberByX;
            x2 = riverModel.LengthByX * i / riverModel.SplitsNumberByX;
            x3 = riverModel.LengthByX * (i + 1) / riverModel.SplitsNumberByX;
            if ((x >= x1) && (x <= x2))
            {
                return riverModel.SplitsNumberByX / riverModel.LengthByX;
            }
            else if ((x >= x2) && (x <= x3))
            {
                return (-1) * riverModel.SplitsNumberByX / riverModel.LengthByX;
            }
            return 0;
        }
        private double U0(double x, GeoinformationModelingViewModel geoinformationModel)
        {
            double res = 0;
            switch (geoinformationModel.TestExample)
            {
                case TestExample.First:
                    res = x;
                    break;
                case TestExample.Second:
                    res = -x;
                    break;
                case TestExample.Third:
                    res = x * x;
                    break;
                default:
                    res = Math.Sqrt(x);
                    break;

            }
            return res;
        }

        private double Uj(double x, RiverParams riverModel)
        {
            double kr = riverModel.LengthByX / riverModel.SplitsNumberByX;
            int nom = (int)(x / kr);
            nom = nom >= riverModel.SplitsNumberByX ? riverModel.SplitsNumberByX - 1 : nom;
            return FF[2 * nom + 1] * Fi(nom, x, riverModel) + FF[2 * (nom + 1) + 1] * Fi(nom + 1, x, riverModel);
        }

        private double dUj(double x, RiverParams riverModel)
        {
            double kr = riverModel.LengthByX / riverModel.SplitsNumberByX;
            int nom = (int)(x / kr);
            nom = nom >= riverModel.SplitsNumberByX ? riverModel.SplitsNumberByX - 1 : nom;
            return FF[2 * nom + 1] * dFi(nom, x, riverModel) + FF[2 * (nom + 1) + 1] * dFi(nom + 1, x, riverModel);
        }
    }
}
