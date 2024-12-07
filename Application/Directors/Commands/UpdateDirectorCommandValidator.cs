using FluentValidation;

namespace Application.Directors.Commands;

public class UpdateDirectorCommandValidator : AbstractValidator<UpdateDirectorCommand>
{
    public UpdateDirectorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.DirectorId).NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);  
    }
}