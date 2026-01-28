using Collection10Api.src.Application.Dtos.Vinil;
using FluentValidation;

namespace Collection10Api.src.Application.Validators.Vinil;

public class VinilCreateValidator : AbstractValidator<VinilCreateDto>
{
    public VinilCreateValidator()
    {
        RuleFor(x => x.Artist)
          .NotEmpty().WithMessage("Artist is required")
          .MinimumLength(3).WithMessage("Artist must be at least 3 characters")
          .MaximumLength(50).WithMessage("Artist must not exceed 50 characters");

        RuleFor(x => x.Album)
            .NotEmpty().WithMessage("Album is required")
            .MinimumLength(3).WithMessage("Album must be at least 3 characters")
            .MaximumLength(50).WithMessage("Album must not exceed 50 characters");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Year is required")
            .InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"Year must be between 1900 and {DateTime.Now.Year}");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Photo)
            .NotEmpty().WithMessage("Photo is required")
            .MinimumLength(10).WithMessage("Photo URL must be at least 10 characters")
            .MaximumLength(255).WithMessage("Photo URL must not exceed 255 characters");
    }
}
