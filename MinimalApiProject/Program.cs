using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Data;
using MinimalApiProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


async Task<List<UsuariosModel>>GetUsuarios(AppDbContext context)
{
    return await context.Usuarios.ToListAsync();
}

app.MapGet("/Ususario", async (AppDbContext context) =>
{
    return await GetUsuarios(context);
});


app.MapGet("/usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuario = await context.Usuarios.FindAsync(id);

    if(usuario == null)
    {
        return Results.NotFound("Usuário não localizado!");
    }

    return Results.Ok(usuario);
});


app.MapPost("/Usuario", async (AppDbContext context, UsuariosModel usuario) =>
{
    context.Usuarios.Add(usuario);
    await context.SaveChangesAsync();

    return await GetUsuarios(context);
});

app.MapPut("/Usuario", async (AppDbContext context, UsuariosModel usuario) =>
{
    var usuarioDb = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(usuarioDb => usuarioDb.Id == usuario.Id);

    if (usuarioDb == null) return Results.NotFound("Usuário não localizado!");
    
    usuarioDb.Username = usuario.Username;
    usuarioDb.Nome = usuario.Nome;
    usuarioDb.Email = usuario.Email;

    context.Update(usuario);
    await context.SaveChangesAsync();

    return Results.Ok(await GetUsuarios(context)
        );
});

app.MapDelete("/usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuarioDb = await context.Usuarios.FindAsync(id);
    if (usuarioDb == null) return Results.NotFound("Usuário não localizado!");

    context.Usuarios.Remove(usuarioDb);
    await context.SaveChangesAsync();

    return Results.Ok(await GetUsuarios(context));
});

app.Run();


