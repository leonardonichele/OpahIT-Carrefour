using Domain.Commands.Requests;
using FluentValidation;

public class CreateLaunchValidator : AbstractValidator<CreateLaunchRequest>
{
    public CreateLaunchValidator()
    {
        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

        RuleFor(x => x.Dt)
            .NotEmpty().WithMessage("A data do lançamento é obrigatória.");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo inválido.");
    }
}