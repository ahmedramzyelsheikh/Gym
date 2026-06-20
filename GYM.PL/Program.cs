using GYM.BLL.Services.Classes;
using GYM.BLL.Services.Interfaces;
using GYM.DAL.Data.GymDbContext;
using GYM.DAL.Repositories.Classes;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GYMDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddScoped<IPlanRepository, planRepository>();
//builder.Services.AddTransient<IPlanRepository, planRepository> as  new object  foreach  operations.
//builder.Services.AddSingleton object  share all  requests . 
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericeRepository<>));
//builder.Services.AddScoped<IPlanRepository<Plan>, PlanRepository<Plan>>();
//builder.Services.AddScoped<IMemberRepository<Member>, MemberRepository<Member>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IPlanServices, PlanService>();
//builder.Services.AddScoped<ITrainerRepository<Trainer>, TrainerRepository<Trainer>>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
