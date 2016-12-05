using System;
using System.Collections.Generic;

namespace MVCModel.Validations
{
    public class ValidationErrors : Exception, IValidationErrors
    {
        public List<IBaseError> Errors { get; set; }
        public ValidationErrors()
        {
            Errors = new List<IBaseError>();
        }

        public ValidationErrors(IBaseError error): this()
        {
            Errors.Add(error);
        }
    }
}
