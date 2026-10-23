using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Practica2.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    var mensajes = options.ModelBindingMessageProvider;
    mensajes.SetValueMustBeANumberAccessor(campo => $"El campo {campo} debe ser un número.");
    mensajes.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"El valor '{valor}' no es válido para {campo}.");
    mensajes.SetUnknownValueIsInvalidAccessor(campo => $"El valor indicado no es válido para {campo}.");
    mensajes.SetValueMustNotBeNullAccessor(campo => $"El campo {campo} es obligatorio.");
});

var apiBaseUrl = new Uri(builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException("Falta 'Api:BaseUrl' en appsettings.json."));

builder.Services.AddHttpClient<IClientesService, ClientesService>(cliente => cliente.BaseAddress = apiBaseUrl);
builder.Services.AddHttpClient<IMascotasService, MascotasService>(cliente => cliente.BaseAddress = apiBaseUrl);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Cultura fija Invariant: el input number envía "12.5" y con es-CR (coma decimal) el binding del Peso fallaría.
var culturaInvariante = CultureInfo.InvariantCulture;
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culturaInvariante),
    SupportedCultures = [culturaInvariante],
    SupportedUICultures = [culturaInvariante],
    RequestCultureProviders = []
});

app.UseHttpsRedirection();
app.UseRouting();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
