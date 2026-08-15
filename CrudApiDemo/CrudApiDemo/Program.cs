using CrudApiDemo.Interfaces;
using CrudApiDemo.Models;
using CrudApiDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICrudService<Client>, ClientService>();
builder.Services.AddSingleton<ICrudService<Product>, ProductService>();
builder.Services.AddSingleton<ICrudService<Order>, OrderService>();
builder.Services.AddSingleton<ICrudService<OrderItem>, OrderItemService>();
builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IOrderItemService, OrderItemService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Client Endpoints
app.MapGet("/clients", (ICrudService<Client> repo) =>
{
    return Results.Ok(repo.GetAll());
});
app.MapGet("/client/{id}", (int id, ICrudService<Client> repo) =>
{
    var client = repo.GetById(id);
    return client is null ? Results.NotFound(new
    {
        message = $"Client with Id {id} not found."
    }) : Results.Ok(client);
});
app.MapPost("/addClient", (Client newClient, ICrudService<Client> repo) =>
{
    var success = repo.Add(newClient);
    return success ? Results.Created($"/clients/{newClient.Id}", newClient) : Results.BadRequest("Email or Id already exists.");
});
app.MapPatch("/updateClientName/{id}", (int id, string newName, IClientService repo) =>
{
    var success = repo.UpdateName(id, newName);
    return success ? Results.Ok() : Results.NotFound();
});
app.MapPatch("/updateClientEmail/{id}", (int id, string newEmail, IClientService repo) =>
{
    var success = repo.UpdateEmail(id, newEmail);
    return success ? Results.Ok() : Results.NotFound();
});
app.MapDelete("/deleteClient/{id}", (int id, ICrudService<Client> repo) =>
{
    var success = repo.Delete(id);
    return success ? Results.Ok() : Results.NotFound();
});

//Product Endpoints
app.MapGet("/products", (ICrudService<Product> repo) =>
{
    return Results.Ok(repo.GetAll());
});
app.MapGet("/product/{id}", (int id, ICrudService<Product> repo) =>
{
    var product = repo.GetById(id);
    return product == null
        ? Results.NotFound(new { message = $"Product with id {id} not found." })
        : Results.Ok(product);
});
app.MapPost("/addProduct", (Product newProduct, ICrudService<Product> repo) =>
{
    return repo.Add(newProduct) ? 
    Results.Created($"/products/{newProduct.Id}", new {message = "Product added successfully." }) 
    : Results.BadRequest(new { message = "Product with same ID already exists." });
});
app.MapDelete("/deleteProduct/{id}", (int id, ICrudService<Product> repo) =>
{
    return repo.Delete(id) ? 
    Results.Ok(new { message = "Product deleted successfully." }) 
    : Results.NotFound(new { message = $"Product with id {id} not found." });
});
app.MapPatch("/updateProductName/{id}", (int id, string newName, IProductService repo) =>
{
    return repo.UpdateName(id, newName)
        ? Results.Ok(new { message = "Product name updated." })
        : Results.NotFound(new { message = $"Product with id {id} not found." });
});
app.MapPatch("/updateProductPrice/{id}", (int id, decimal newPrice, IProductService repo) =>
{
    return repo.UpdatePrice(id, newPrice)
        ? Results.Ok(new { message = "Product price updated." })
        : Results.NotFound(new { message = $"Product with id {id} not found." });
});

//Orders Endpoints
app.MapGet("/orders", (ICrudService<Order> repo) =>
{
    return Results.Ok(repo.GetAll());
});
app.MapGet("/order/{id}", (int id, ICrudService<Order> repo) =>
{
    var order = repo.GetById(id);
    return order is null
        ? Results.NotFound(new { message = $"Order with id {id} not found." })
        : Results.Ok(order);
});
app.MapPost("/newOrder", (Order newOrder, ICrudService<Order> repo) =>
{
    var success = repo.Add(newOrder);
    return success
        ? Results.Created($"/orders/{newOrder.Id}", new { message = "Order created successfully.", order = newOrder })
        : Results.BadRequest(new { message = "An order with that id already exists or the user does not exist." });
});
app.MapDelete("/orders/{id}", (int id, ICrudService<Order> repo) =>
{
    return repo.Delete(id)
        ? Results.Ok(new { message = "Order deleted successfully." })
        : Results.NotFound(new { message = $"Order with id {id} not found." });
});
app.MapDelete("/orders/{orderId}/items/{itemId}", (int orderId, int itemId, IOrderService repo) =>
{
    var success = repo.RemoveItemFromOrder(orderId, itemId);
    return success
        ? Results.Ok(new { message = "Item removed from order." })
        : Results.NotFound(new { message = "Order or item not found." });
});
app.MapPost("/orders/addItemToOrder", (OrderItem item, IOrderService repo) =>
{
    var success = repo.AddItemToOrder(item.OrderId, item);
    return success
        ? Results.Ok(new { message = "Item added to order." })
        : Results.NotFound(new { message = $"Order with id {item.OrderId} not found." });
});

//OrderItem Endpoints
app.MapGet("/orderitems", (ICrudService<OrderItem> repo) =>
{
    return Results.Ok(repo.GetAll());
});

app.MapGet("/orderitems/{id}", (int id, ICrudService<OrderItem> repo) =>
{
    var item = repo.GetById(id);
    return item is null
        ? Results.NotFound(new { message = $"OrderItem with id {id} not found." })
        : Results.Ok(item);
});
app.MapPatch("/orderitems/{id}/quantity", (int id, int newQuantity, IOrderItemService repo) =>
{
    var success = repo.UpdateQuantity(id, newQuantity);
    return success
        ? Results.Ok(new { message = "Quantity updated." })
        : Results.NotFound(new { message = $"OrderItem with id {id} not found." });
});

app.Run();
