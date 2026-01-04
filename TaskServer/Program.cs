using Microsoft.EntityFrameworkCore;
using TaskServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
app.UseCors();
app.MapGet("/tasks", (AppDbContext context) => 
{
    Console.WriteLine("GET recieved");
    var tasks = context.Tasks.ToList();
    return Results.Ok(tasks);
});
app.MapPost("/tasks", (AppDbContext context, TaskItem task) =>
{
    Console.WriteLine($"new task:{task.Title}");
    task.Id = context.Tasks.Any() ? context.Tasks.Max(t => t.Id) + 1 : 1;
    context.Tasks.Add(task);
    context.SaveChanges();
    return Results.Ok(task);
});
app.MapDelete("/tasks/{id}", (AppDbContext context, int id) =>
{
    var task = context.Tasks.FirstOrDefault(t => t.Id == id);
    if (task == null) return Results.NotFound();
    Console.WriteLine($"deleted task:{task}");
    context.Tasks.Remove(task);
    context.SaveChanges();
    return Results.Ok();
});
app.MapPatch("/tasks/{id}", (AppDbContext context, int id, CompleteDto completed) => 
{
    Console.WriteLine($"completion changed: id:{id}, completed:{completed.Completed}");
    var task = context.Tasks.FirstOrDefault(t => t.Id == id);
    if (task == null) return Results.NotFound();
    task.Completed = completed.Completed;
    context.SaveChanges();
    return Results.Ok();
});
app.MapPatch("/tasks/update/{id}", (AppDbContext context, int id, TaskUpdateDto update) =>
{
    var task = context.Tasks.FirstOrDefault(t => t.Id == id);
    if (task == null) return Results.NotFound();
    if(update.Title != null) task.Title = update.Title;
    if (update.Description != null) task.Description = update.Description;
     context.SaveChanges();
    return Results.Ok(task);
});
app.Run();