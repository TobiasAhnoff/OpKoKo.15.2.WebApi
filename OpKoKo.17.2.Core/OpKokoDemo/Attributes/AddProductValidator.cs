using System;
using System.ComponentModel.DataAnnotations;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Attributes
{
    public class AddProductValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var request = (AddProductRequest)validationContext.ObjectInstance;
            if (request.Price < 0 || request.Price > 10000)
            {
                ErrorMessage = $"{nameof(request.Price)} must be between 1 to 9999";
                return new ValidationResult(ErrorMessage);
            }

            if (!Enum.TryParse(typeof(Language), request.Language, out object validLanguange))
            {
                ErrorMessage = $"{nameof(request.Language)} is not a valid language.";
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
