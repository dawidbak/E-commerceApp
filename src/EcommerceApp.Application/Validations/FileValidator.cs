using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.Validations
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(200000); //.WithMessage("File size is larger than allowed,max 200KB");
            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"));
            //.WithMessage("File type is different than allowed, must be:.jpeg, .jpg or .png");
        }
    }
}
