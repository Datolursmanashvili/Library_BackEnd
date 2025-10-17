using Application.Shared;
using Domain.Entities.ProductEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.ProductsCommands;


[Validator(typeof(AddProductCommandValidator))]
public class AddProductCommand : Command<ProductCommandResult>
{
    public string Name { get; set; }
    public string Annotation { get; set; }
    public string ProductType { get; set; }
    public string ISBN { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int PublisherId { get; set; }
    public int PageCount { get; set; }
    public string Address { get; set; }
    public override async Task<CommandExecutionResultGeneric<ProductCommandResult>> ExecuteCommandLogicAsync()
    {   

        var product = new Product
        {
            Name = Name,
            Annotation = Annotation,
            ProductType = ProductType,
            ISBN = ISBN,
            Address = Address,
            PageCount = PageCount,
            ReleaseDate = ReleaseDate,
            PublisherId = PublisherId,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        if (applicationDbContext.Publishers.FirstOrDefault(x=> x.Id == PublisherId && x.IsDeleted == false).IsNull())
        {
            return await Fail<ProductCommandResult>("ასეთი გამომცემლობა არ არსებობს");
        }

        var result = await _productRepository.CreateAsync(product);
        if (!result.Success)
            return await Fail<ProductCommandResult>(result.ErrorMessage);

        return await Ok(new ProductCommandResult
        {
            Id = Convert.ToInt32(result.ResultId),
            Name = product.Name,
            Annotation = product.Annotation,
            ProductType = product.ProductType,
            ISBN = product.ISBN,
            ReleaseDate = product.ReleaseDate,
            PublisherId = product.PublisherId,
            Address = product.Address,
            PageCount = product.PageCount,  
        });
    }

    // Validator
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
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

public class ProductCommandResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Annotation { get; set; }
    public string ProductType { get; set; }
    public string ISBN { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int PublisherId { get; set; }
    public int PageCount { get; set; }
    public string Address { get; set; }
}