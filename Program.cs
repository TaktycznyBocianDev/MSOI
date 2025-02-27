using MSOI.Components;
using DataLibrary;
using MSOI.Services; //adding using statement

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Adding DataAccess service
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddScoped<WorkerService>();
builder.Services.AddScoped<ItemTypeService>();
builder.Services.AddScoped<ItemReleaseService>();
builder.Services.AddScoped<WorkerItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
