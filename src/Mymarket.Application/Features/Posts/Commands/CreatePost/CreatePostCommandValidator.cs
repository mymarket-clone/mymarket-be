using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.features.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandValidator(IApplicationDbContext context)
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
            .WithMessage("English title cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.TitleEn));

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(4000).WithMessage("English description cannot exceed 4000 characters")
            .When(x => !string.IsNullOrEmpty(x.DescriptionEn));

        RuleFor(x => x.TitleRu)
            .MaximumLength(255).WithMessage("Russian title cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.TitleRu));

        RuleFor(x => x.DescriptionRu)
            .MaximumLength(4000).WithMessage("Russian description cannot exceed 4000 characters")
            .When(x => !string.IsNullOrEmpty(x.DescriptionRu));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Enter name")
            .MaximumLength(72).WithMessage("Name cannot exceed 72 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Enter phone number");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required")
            .MustAsync(UserExists).WithMessage("User does not exist.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Enter price")
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");

        RuleFor(x => x.PostType)
            .NotEmpty().WithMessage("Enter post type")
            .IsInEnum().WithMessage("Invalid post type");

        RuleFor(x => x.CurrencyType)
            .NotEmpty().WithMessage("Enter currency")
            .IsInEnum().WithMessage("Invalid currency type");

        RuleFor(x => x.CanOfferPrice).NotNull();

        RuleFor(x => x.IsNegotiable).NotNull();

        RuleFor(x => x.ForDisabledPerson).NotNull();

        RuleFor(x => x.IsColored).NotNull();

        RuleFor(x => x.AutoRenewal).NotNull();
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
    }

    private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    }
}
