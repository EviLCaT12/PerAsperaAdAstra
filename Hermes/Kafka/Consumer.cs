using System;
using System.Threading;
using Confluent.Kafka;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka;

public class Consumer<TKey, TValue>
{
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Consumer(ConsumerConfig config, IServiceScopeFactory serviceScopeFactory)
    {
        _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Consume(string topic, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    if (consumeResult.Message.Value is string queryString)
                    {
                        queryString = queryString.Replace("Nodes", "Nodes1");
                        context.Database.ExecuteSqlRaw(queryString);
                    }
                    else
                    {
                        Console.WriteLine("Consumed message is not a string, skipping SQL execution.");
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
    }
}
