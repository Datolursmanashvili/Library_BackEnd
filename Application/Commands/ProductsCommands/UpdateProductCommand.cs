using Application.Shared;
using FluentValidation;
using Shared;

namespace Application.Commands.ProductsCommands;

public class UpdateProductCommand : Command<ProductCommandResult>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Annotation { get; set; }
    public string ProductType { get; set; }
    public string ISBN { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int PublisherId { get; set; }

    public override async Task<CommandExecutionResultGeneric<ProductCommandResult>> ExecuteAsync()
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(this);

        if (!validationResult.IsValid)
            return await Fail<ProductCommandResult>(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
            );

        var product = await _productRepository.GetByIdAsync(Id);
        if (product == null)
            return await Fail<ProductCommandResult>("პროდუქტი ვერ მოიძებნა");

        product.Name = Name;
        product.Annotation = Annotation;
        product.ProductType = ProductType;
        product.ISBN = ISBN;
        product.ReleaseDate = ReleaseDate;
        product.PublisherId = PublisherId;

        var result = await _productRepository.UpdateAsync(product);
        if (!result.Success)
            return await Fail<ProductCommandResult>(result.ErrorMessage);

        return await Ok(new ProductCommandResult
        {
            Id = product.Id,
            Name = product.Name,
            Annotation = product.Annotation,
            ProductType = product.ProductType,
            ISBN = product.ISBN,
            ReleaseDate = product.ReleaseDate,
            PublisherId = product.PublisherId
        });
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("პროდუქტის Id სავალდებულოა");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("სახელი სავალდებულოა")
                .MinimumLength(2).WithMessage("სახელი უნდა იყოს მინიმუმ 2 სიმბოლო")
                .MaximumLength(250).WithMessage("სახელი მაქსიმუმ 250 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.Annotation)
                .NotEmpty().WithMessage("ანოტაცია სავალდებულოა")
                .MinimumLength(100).WithMessage("ანოტაცია უნდა იყოს მინიმუმ 100 სიმბოლო")
                .MaximumLength(500).WithMessage("ანოტაცია მაქსიმუმ 500 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.ProductType)
                .NotEmpty().WithMessage("პროდუქტის ტიპი სავალდებულოა")
                .Must(pt => pt == "წიგნი" || pt == "სტატია" || pt == "ელექტრონული რესურსი")
                .WithMessage("პროდუქტის ტიპი უნდა იყოს 'წიგნი', 'სტატია' ან 'ელექტრონული რესურსი'");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN სავალდებულოა")
                .Matches(@"^\d{13}$").WithMessage("ISBN უნდა შეიცავდეს ზუსტად 13 ციფრს");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("გამოშვების თარიღი სავალდებულოა");

            RuleFor(x => x.PublisherId)
                .GreaterThan(0).WithMessage("გამომცემლობა სავალდებულოა");
        }
    }
}
