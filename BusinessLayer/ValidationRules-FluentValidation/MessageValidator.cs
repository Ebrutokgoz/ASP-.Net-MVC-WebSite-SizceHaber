using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules_FluentValidation
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.ReceiverMail).NotEmpty().WithMessage("Alıcı mail adresi boş bırakılamaz!");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Konu boş bırakılamaz!");
            RuleFor(x => x.MessageContent).NotEmpty().WithMessage("Mesaj boş bırakılamaz!");
            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Konu en az 3 karakterden oluşmalıdır!");
            RuleFor(x => x.Subject).MaximumLength(100).WithMessage("Konu en fazla 100 karakterden oluşmalıdır!");
            RuleFor(x => x.MessageContent).MaximumLength(1000).WithMessage("Mesaj en fazla 1000 karakterden oluşmalıdır!");
        }
    }
}
