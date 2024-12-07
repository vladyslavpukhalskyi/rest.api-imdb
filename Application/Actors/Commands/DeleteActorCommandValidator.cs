using FluentValidation;

namespace Application.Actors.Commands;

public class DeleteActorCommandValidator : AbstractValidator<DeleteActorCommand>
{
    public DeleteActorCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("ActorId is required.");
    }
}