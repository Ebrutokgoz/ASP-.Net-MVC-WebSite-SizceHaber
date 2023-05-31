using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules_FluentValidation
{
    public class HeadingValidator: AbstractValidator<Heading>
    {
        public HeadingValidator()
        {
            RuleFor(x => x.HeadingName).NotEmpty().WithMessage("Başlık boş bırakılamaz!");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Kategori boş bırakılamaz!");
            RuleFor(x => x.WriterID).NotEmpty().WithMessage("Yazar boş bırakılamaz!");
            RuleFor(x => x.HeadingName).MinimumLength(3).WithMessage("Başlık en az 3 karakterden oluşmalıdır!");
            RuleFor(x => x.HeadingName).MaximumLength(250).WithMessage("Başlık en fazla 20 karakterden oluşmalıdır!");

        }
    }
}
