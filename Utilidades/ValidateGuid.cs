using System.ComponentModel.DataAnnotations;

namespace APIControlNet.Utilidades
{
    public class ValidateGuid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is Guid guidValue)
                {
                    if (guidValue != Guid.Empty)
                    {
                        return ValidationResult.Success;
                    }
                }
            }

            return new ValidationResult("El valor proporcionado no es un GUID válido.");
        }
    }
}
