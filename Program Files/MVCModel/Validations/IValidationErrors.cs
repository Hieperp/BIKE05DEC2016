using System.Collections.Generic;

namespace MVCModel.Validations
{
    public interface IValidationErrors
    {
        List<IBaseError> Errors { get; set; }
    }
}
