using BlazorWalletTrackerFE;
using BlazorWalletTrackerFE.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using BlazorWalletTrackerFE.Store;
using BlazorWalletTrackerFE.Store.TransactionStore;
using BlazorWalletTrackerFE.Store.UserStore;
using BlazorWalletTrackerFE.Shared.Services;
using BlazorWalletTrackerFE.Store.CategoryStore;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7220/") });
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddFluxor(options =>
{
    options
        .ScanAssemblies(typeof(Program).Assembly)
        .UseReduxDevTools(
        opt =>
        {
            opt.Name = "BlazorWalletTrackerFE";
            opt.EnableStackTrace();
        });
});
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<TransactionState>();
builder.Services.AddScoped<CategoryState>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    );
});
await builder.Build().RunAsync();
