using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Posts.Commands.Add;

public class AddPostCommandValidator : AbstractValidator<AddPostCommand>
{
    private readonly IApplicationDbContext _context;

    public AddPostCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Choose category")
            .MustAsync(CategoryExists).WithMessage("Selected category does not exist.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Enter title")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Enter description")
            .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters");

        RuleFor(x => x.TitleEn)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.TitleEn))
            .WithMessage("English title cannot exceed 255 characters");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(4000)
            .When(x => !string.IsNullOrEmpty(x.DescriptionEn))
            .WithMessage("English description cannot exceed 4000 characters");

        RuleFor(x => x.TitleRu)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.TitleRu))
            .WithMessage("Russian title cannot exceed 255 characters");

        RuleFor(x => x.DescriptionRu)
            .MaximumLength(4000)
            .When(x => !string.IsNullOrEmpty(x.DescriptionRu))
            .WithMessage("Russian description cannot exceed 4000 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Enter name")
            .MaximumLength(72).WithMessage("Name cannot exceed 72 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Enter phone number");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("Choose city")
            .MustAsync(CityExists).WithMessage("Selected city does not exist.");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Enter price")
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");

        RuleFor(x => x.PostType)
            .IsInEnum().WithMessage("Invalid post type");

        RuleFor(x => x.CurrencyType)
            .IsInEnum().WithMessage("Invalid currency type");

        RuleFor(x => x.AutoRenewalOnceIn)
            .Must((x, v) => x.AutoRenewal ? v != null : v == null)
            .WithMessage("Auto renewal interval must be set only when auto renewal is enabled.");

        RuleFor(x => x.AutoRenewalAtTime)
            .Must((x, v) => x.AutoRenewal ? v != null : v == null)
            .WithMessage("Auto renewal time must be set only when auto renewal is enabled.");

        RuleFor(x => x.AutoRenewalOnceIn)
            .GreaterThan(0)
            .When(x => x.AutoRenewalOnceIn.HasValue);

        RuleFor(x => x.PromoDays)
            .Must((x, v) => x.PromoType != null ? v != null : v == null)
            .WithMessage("Promo days must be set only when promo type is selected.");

        RuleFor(x => x.PromoDays)
            .GreaterThan(0)
            .When(x => x.PromoDays.HasValue);

        RuleFor(x => x.ColorDays)
            .Must((x, v) => x.IsColored ? v != null : v == null)
            .WithMessage("Color days must be set only when coloring is enabled.");

        RuleFor(x => x.ColorDays)
            .GreaterThan(0)
            .When(x => x.ColorDays.HasValue);
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == categoryId, cancellationToken);
    }

    private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }

    private async Task<bool> CityExists(int cityId, CancellationToken cancellationToken)
    {
        return await _context.Cities
            .AnyAsync(c => c.Id == cityId, cancellationToken);
    }
}