namespace LendingApp.API;

using FluentValidation;
using LendingApp.Application.Behavior;
using LendingApp.Application.Command.CreateLoanRequest;
using LendingApp.Application.Command.CreateQuoteCommand;
using LendingApp.Application.Command.LoanConfirmation;
using LendingApp.Application.Command.UpdateLoanRequest;
using LendingApp.Application.Query.GetAllProducts;
using LendingApp.Application.Query.GetLoanRequestById;
using LendingApp.Application.Query.GetQuoteById;
using LendingApp.Domain.Interfaces;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Services;
using LendingApp.Infrastructure.Repositories;
using MediatR;
using System.Reflection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        //CORS
        services.AddCors(options =>
         {
             options.AddPolicy("AllowAngularApp", policy =>
             {
                 policy.WithOrigins("http://localhost:4200") // Your Angular app URL
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials(); // If you need to send cookies/authentication
             });
         });

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateLoanRequestCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateLoanRequestCommandHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(UpdateLoanRequestCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(UpdateLoanRequestCommandHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetAllProductsQuery).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetAllProductsQueryHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetLoanRequestByIdQuery).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetLoanRequestByIdQueryHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateQuoteCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateQuoteCommandHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetQuoteByIdQuery).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetQuoteByIdQueryHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoanConfirmationCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoanConfirmationCommandHandler).Assembly));

        //validators
        services.AddValidatorsFromAssemblyContaining<CreateLoanRequestCommandValidator>();
        services.AddValidatorsFromAssembly(typeof(CreateLoanRequestCommandValidator).Assembly);
        services.AddValidatorsFromAssemblyContaining<LoanConfirmationCommandValidator>();
        services.AddValidatorsFromAssembly(typeof(LoanConfirmationCommandValidator).Assembly);

        // MediatR pipeline behavior to run FluentValidation validators for requests
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        //services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILoanRequestService, LoanRequestService>();
        services.AddScoped<IBlacklistService, BlacklistService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IQuoteService, QuoteService>();
        services.AddScoped<ILoanCalculationService, LoanCalculationService>();
        services.AddScoped<ILoanConfirmationService, LoanConfirmationService>();

        //repositories
        services.AddScoped<IBorrowerRepository, BorrowerRepository>();
        services.AddScoped<ILoanDetailsRepository, LoanDetailsRepository>();
        services.AddScoped<IBlacklistRepository, BlacklistRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

        return services;
    }
}