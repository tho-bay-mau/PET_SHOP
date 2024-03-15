using System.ComponentModel.DataAnnotations;

namespace ThoBayMau_ASM.Validation
{
    public class CheckWhiteSpace : ValidationAttribute
    {
        public CheckWhiteSpace() => ErrorMessage = "{0} không chứa khoảng trắng";
        public override bool IsValid(object? value)
        {
            if (value == null) return false;
            return !((string)value).Contains(" ");
        }
    }
}
