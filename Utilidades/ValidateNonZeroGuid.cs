using System.ComponentModel.DataAnnotations;

namespace APIControlNet.Utilidades
{
    public class ValidateNonZeroGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Guid.TryParse(value.ToString(), out Guid result))
            {
                if (result == Guid.Empty)
                {
                    return new ValidationResult("El ID proporcionado no puede ser un GUID de ceros.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("El ID proporcionado no es un GUID válido.");
            }
        }
    }
}
