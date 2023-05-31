using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules_FluentValidation
{
    public class ContentValidator : AbstractValidator<Content>
    {
        public ContentValidator()
        {
            RuleFor(x => x.ContentValue).NotEmpty().WithMessage("Bir yazı yazmadınız!");
            RuleFor(x => x.ContentValue).MaximumLength(3000).WithMessage("En fazla 3000 karakterden oluşan bir yazı ekleyebilirsiniz.");

        }
    }
}
