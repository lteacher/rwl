using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace RobotInitial.Validation {
	class NumericOnlyRule : ValidationRule {
		
		public string ErrorMessage { get; set; }
		
		public override ValidationResult Validate(object value,CultureInfo cultureInfo) {
			ValidationResult result = new ValidationResult(true, null);
			int i=0;

			bool isInt = Int32.TryParse((string)value,out i);
			return new ValidationResult(isInt,this.ErrorMessage);
			//string inputString = (value ?? string.Empty).ToString();
			//if (inputString.Length < this.MinimumLength ||
			//       (this.MaximumLength > 0 &&
			//        inputString.Length > this.MaximumLength)) {
			//    result = new ValidationResult(false, this.ErrorMessage);
			//}
			//return result;
		}
	}
}
