using FluentValidation;
using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.PersonService.Commands.CreatePersonRelation;

public class CreatePersonRelationValidator : AbstractValidator<CreatePersonRelationCommand>
{
    public CreatePersonRelationValidator()
    {
        RuleFor(x => x.PersonId)
            .NotNull()
            .WithMessage("Person Id Required");

        RuleFor(x => x.RelatedPersonId)
            .NotNull()
            .WithMessage("Related Person Id Required");

        RuleFor(x => x.RelationType)
            .NotNull()
            .Must(x => Enum.TryParse<RelationType>(x.ToString(), out _))
            .WithMessage("Invalid Relation Type");
    }
}