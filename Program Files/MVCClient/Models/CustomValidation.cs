using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCClient.Models
{
    public class CustomValidation
    {
    }

    public enum GenericCompareOperator
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    //public class TestDate : RequiredAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        if (value != null)
    //        {                
    //            return base.IsValid(true) && ((DateTime)value) > DateTime.Now;
    //        }
    //        else
    //            return base.IsValid(false);
    //    }
        
    //}

    public class GenericCompareAttribute : ValidationAttribute, IClientValidatable
    {
        private GenericCompareOperator operatorname = GenericCompareOperator.GreaterThanOrEqual;

        public string CompareToPropertyName { get; set; }
        public GenericCompareOperator OperatorName { get { return operatorname; } set { operatorname = value; } }
        //public IComparable CompareDataType { get; set; }

        public GenericCompareAttribute() : base() { }

        //Override IsValid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string operstring = (OperatorName == GenericCompareOperator.GreaterThan ?
            "greater than " : (OperatorName == GenericCompareOperator.GreaterThanOrEqual ?
            "greater than or equal to " :
            (OperatorName == GenericCompareOperator.LessThan ? "less than " :
            (OperatorName == GenericCompareOperator.LessThanOrEqual ? "less than or equal to " : ""))));
            var basePropertyInfo = validationContext.ObjectType.GetProperty(CompareToPropertyName);

            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valThis = (IComparable)value;

            if ((operatorname == GenericCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
                (operatorname == GenericCompareOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0) ||
                (operatorname == GenericCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0) ||
                (operatorname == GenericCompareOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0))
                return new ValidationResult(base.ErrorMessage);
            return null;
        }
        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule>
        GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            ModelClientValidationRule compareRule = new ModelClientValidationRule();
            compareRule.ErrorMessage = errorMessage;
            compareRule.ValidationType = "genericcompare";
            compareRule.ValidationParameters.Add("comparetopropertyname", CompareToPropertyName);
            compareRule.ValidationParameters.Add("operatorname", OperatorName.ToString());
            yield return compareRule;
        }

        #endregion
    }

    //public class BirthDateCheckAttribute : ValidationAttribute
    //{
    //    //variable which will hold the number of years for the age check
    //    private int Numyears;

    //    public BirthDateCheckAttribute(int numyears)
    //    {
    //        //catch the pass-in variable from the function call
    //        this.Numyears = numyears;
    //    }

    //    //override the IsValid method of the Validation Attribute

    //    public override bool IsValid(object value)
    //    {
    //        //if the passed-in value to be checked is null, exit and return false
    //        if (value != null)
    //        {
    //            //convert the passed-in value to a DateTime object
    //            DateTime birthdate = Convert.ToDateTime(value);

    //            //subtract the passed-in number of years from the current DateTime
    //            DateTime targetbirthdate = DateTime.Now.AddYears(-Numyears);

    //            //compare the two dates.  If the birthdate is greater that the target date, the function returns 1 else it returns 0
    //            int comp = DateTime.Compare(birthdate, targetbirthdate);

    //            //now we simply evaluate the result of the compare function.  If the value is greater than 0, return false, otherwise return true
    //            if (comp > 0)
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                return true;
    //            }
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}
}