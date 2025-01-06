using Poliak_UI_WT.Blazor.Components;
using Poliak_UI_WT.Blazor.Services;
using Poliak_UI_WT.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IProductService<Phone>, ApiProductService>
    (client => client.BaseAddress = new Uri("https://localhost:7002/api/Phones/"));
builder.Services.AddScoped<IProductService<Phone>, ApiProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
