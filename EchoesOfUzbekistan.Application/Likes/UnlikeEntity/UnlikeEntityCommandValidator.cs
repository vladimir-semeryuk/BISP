using EchoesOfUzbekistan.Domain.Abstractions;
using FluentValidation;

namespace EchoesOfUzbekistan.Application.Likes.UnlikeEntity;
public class UnlikeEntityCommandValidator : AbstractValidator<UnlikeEntityCommand>
{
    public UnlikeEntityCommandValidator()
    {
        RuleFor(x => x.EntityType)
            .NotEmpty().WithMessage("Entity type must be provided.")
            .Must(BeAValidEntityType).WithMessage("Entity type must be AudioGuide.");
    }

    private bool BeAValidEntityType(string entityType)
    {
        var validEntityTypes = new[] { EntityTypeNames.AudioGuide };
        return validEntityTypes.Contains(entityType);
    }
}