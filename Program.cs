using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static ConcurrentQueue<Order> orderQueue = new ConcurrentQueue<Order>();
    private static Random random = new Random();

    static void Main()
    {
        Task[] waiters = new Task[3];
        for (int i = 0; i < waiters.Length; i++)
        {
            int waiterId = i + 1;
            waiters[i] = Task.Run(() => Waiter(waiterId));
        }

        Task[] cooks = new Task[3];
        for (int i = 0; i < cooks.Length; i++)
        {
            int cookId = i + 1;
            cooks[i] = Task.Run(() => Cook(cookId));
        }

        Task.WaitAll(waiters);
        Task.WaitAll(cooks);
    }

    static void Waiter(int waiterId)
    {
        for (int i = 0; i < 5; i++) // Каждый официант принимает 5 заказов
        {
            Thread.Sleep(random.Next(1000, 3000)); // Симуляция времени принятия заказа
            var order = new Order { OrderId = i + 1, WaiterId = waiterId, Dish = "Блюдо " + (i + 1) };
            orderQueue.Enqueue(order);
            Console.WriteLine($"Официант {waiterId} принял заказ: {order.Dish}");
        }
    }

    static void Cook(int cookId)
    {
        while (true)
        {
            if (orderQueue.TryDequeue(out Order? order))
            {
                Thread.Sleep(random.Next(1000, 3000)); // Симуляция времени приготовления блюда
                Console.WriteLine($"Повар {cookId} приготовил заказ: {order.Dish}");
            }
            else
            {
                break;
            }
        }
    }
}

class Order
{
    public int OrderId { get; set; }
    public int WaiterId { get; set; }
    public string? Dish { get; set; }
}
