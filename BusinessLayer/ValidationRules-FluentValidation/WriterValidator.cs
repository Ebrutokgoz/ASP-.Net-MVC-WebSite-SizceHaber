﻿using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules_FluentValidation
{
    public class WriterValidator: AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.WriterName).NotEmpty().WithMessage("Ad boş bırakılamaz!");
            RuleFor(x => x.WriterSurname).NotEmpty().WithMessage("Soyadı boş bırakılamaz!");
            RuleFor(x => x.WriterPassword).NotEmpty().WithMessage("Şifre boş bırakılamaz!");
            RuleFor(x => x.WriterName).MinimumLength(2).WithMessage("Ad en az 2 karakterden oluşmalıdır!");
            RuleFor(x => x.WriterSurname).MaximumLength(50).WithMessage("Soyad en fazla 50 karakterden oluşmalıdır!");RuleFor(x => x.WriterName).MinimumLength(2).WithMessage("Yazar adı en az 2 karakterden oluşmalıdır!");
            RuleFor(x => x.WriterSurname).MaximumLength(50).WithMessage("Soyad en fazla 50 karakterden oluşmalıdır!");
            

        }
    }
}
