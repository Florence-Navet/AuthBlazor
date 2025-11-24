using BlazorServerAuth.Components;
using BlazorServerAuth.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BlazorServerAuth;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = IdentityConstants.ApplicationScheme;
});

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/account/logout", async (
    SignInManager<IdentityUser> signIn) =>
{
    await signIn.SignOutAsync();
    return Results.Redirect("/");
});

app.Run();
