using Microsoft.EntityFrameworkCore;
using MinimalBookListApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add db context to appl.
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("BookListApi"));



//after build we can use services that were registered through the DI
var app=builder.Build();


app.MapGet("/booksList", async (ApiDbContext db) => 
          await db.Books.ToListAsync());


app.MapPost("/addBook", async (Book book, ApiDbContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/booksList/{book.Id}",book);
});

app.MapGet("/getBookById/{id}", async (int id, ApiDbContext db) =>
{
    var book = await db.Books.FindAsync(id);
    
    return book!=null? Results.Ok(): Results.NotFound();
});

app.MapDelete("/deleteBookById/{id}", async (int id, ApiDbContext db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book != null)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    
    return Results.NotFound();
});

app.MapPut("/updateBookById/{id}", async (int id,Book book, ApiDbContext db) =>
{
    var bookInDb=await db.Books.FindAsync(id);

    if (bookInDb != null)
    {
        //db.Update(book);
        bookInDb.Name = book.Name;
        bookInDb.IsPurchased = book.IsPurchased;

        await db.SaveChangesAsync();
        return Results.Ok(bookInDb);
    }
    return Results.NotFound();

});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();