using GYM.BLL.Services.Classes;
using GYM.BLL.Services.Interfaces;
using GYM.DAL.Data.DataSeeder;
using GYM.DAL.Data.GymDbContext;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Classes;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GYMDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<GYMDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


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
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));


var app = builder.Build();

// Seed the database with plans from JSON file
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GYMDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");
    var seedFilesPath = Path.Combine(app.Environment.WebRootPath, "Files");
    await DataSeeder.SeedAsync(context, seedFilesPath, logger);
    
    // Seed Identity
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DataSeederIdentity.SeedRolesAndAdminAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
