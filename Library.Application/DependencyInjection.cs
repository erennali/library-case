using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Library.Application.Abstractions.Services;
using Library.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMemberService, MemberService>();

        // New service registrations
        //services.AddScoped<ITransactionService, TransactionService>();
        //services.AddScoped<IFineService, FineService>();
        //services.AddScoped<IReservationService, ReservationService>();
        //services.AddScoped<IReviewService, ReviewService>();
        //services.AddScoped<INotificationService, NotificationService>();
        //services.AddScoped<IReportService, ReportService>();
        //services.AddScoped<IAlertService, AlertService>();
        //services.AddScoped<ILibrarianService, LibrarianService>();
        //services.AddScoped<ISettingsService, SettingsService>();
        //services.AddScoped<IDashboardService, DashboardService>();
        //services.AddScoped<ISearchService, SearchService>();
        //services.AddScoped<IStatisticsService, StatisticsService>();
        //services.AddScoped<IAuditService, AuditService>();
        //services.AddScoped<IImportExportService, ImportExportService>();

        // Auth services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}


