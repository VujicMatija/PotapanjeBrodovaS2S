using PotapanjeBrodovaS2S.Components;
using Microsoft.AspNetCore.ResponseCompression;
using PotapanjeBrodovaS2S.Hubs;
using PotapanjeBrodovaS2S.Servisi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


//Dodavanje SignalR biblioteke
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});


//dodavanje servisa
//Singleton - jedna instanca za celu aplikaciju i svi je dele, svi korisnici vidi jednu instancu servisa
//Scoped - jedna instanca po http zahtevu, moze se reci po tabu, jedna instanca za jednog korisnika 
//Transient - svaki put kad se pozove servis se pravi nova instanca servera, jedna instanca po zahtevu korisnika
builder.Services.AddScoped<Komunikacija>();
builder.Services.AddScoped<BazaServis>();
builder.Services.AddSingleton<SobaServis>(); 




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

//mapiranje huba
app.MapHub<GameHub>("/gamehub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
