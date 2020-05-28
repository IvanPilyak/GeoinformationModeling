using GeoinformationModeling.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Services.Abstractions
{
    public interface IMatrixGeoinformaionService
    {
        MatrixesModel GetMatrixes(GeoinformationModelingViewModel geoinformationModel);
    }
}
