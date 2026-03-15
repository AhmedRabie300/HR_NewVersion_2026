using Application.UARbac.Users.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Users.Validators
{
    internal class UpdateUserValidator :AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator() 
        
        {
            RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage("Id is required.");

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                .WithMessage("English Caption max length is 100.");

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                .WithMessage("Arabic Caption max length is 100.");

            // Optional: if your business wants at least one change
            RuleFor(x => x)
                .Must(x =>
                    x.EngName != null ||
                    x.ArbName != null  
                    )
                .WithMessage("At least one field must be provided to update.");
        }


    }    
}

