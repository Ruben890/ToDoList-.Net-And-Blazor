using Frontend;
using Frontend.Services;
using Frontend.Services.Auth;
using Frontend.Services.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthServices>();
//builder.Services.AddScoped<ToDoService>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7179/") });

await builder.Build().RunAsync();
