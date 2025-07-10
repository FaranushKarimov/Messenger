using Microsoft.AspNetCore.SignalR.Client;
using Messenger.Application.DTOs;  // подключаем DTO

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/chatHub")
    .Build();

// получение входящих сообщений
connection.On<string, string>("ReceiveMessage", (fromUserId, message) =>
{
    Console.WriteLine($"Получено сообщение от {fromUserId}: {message}");
});

await connection.StartAsync();
Console.WriteLine("Клиент подключен к серверу SignalR");

while (true)
{
    Console.WriteLine("Введите команду (send/edit/delete/exit):");
    var command = Console.ReadLine();

    if (command == "send")
    {
        Console.Write("FromUserId: ");
        var from = Console.ReadLine();
        Console.Write("ToUserId: ");
        var to = Console.ReadLine();
        Console.Write("Сообщение: ");
        var messageText = Console.ReadLine();

        var request = new SendMessageRequestDto(
            from!,
            to!,
            messageText!
        );

        await connection.InvokeAsync("SendMessage", request);
    }
    else if (command == "edit")
    {
        Console.Write("Id сообщения: ");
        var id = Guid.Parse(Console.ReadLine()!);
        Console.Write("Новый текст: ");
        var newText = Console.ReadLine();
        await connection.InvokeAsync("EditMessage", id, newText);
    }
    else if (command == "delete")
    {
        Console.Write("Id сообщения: ");
        var id = Guid.Parse(Console.ReadLine()!);
        await connection.InvokeAsync("DeleteMessage", id);
    }
    else if (command == "exit")
    {
        break;
    }
}

await connection.StopAsync();
Console.WriteLine("Соединение закрыто");