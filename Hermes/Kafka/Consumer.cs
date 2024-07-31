using System;
using System.Threading;
using Confluent.Kafka;

namespace Kafka;

public class Consumer<TKey, TValue>
{
    private readonly IConsumer<TKey, TValue> _consumer;

    public Consumer(ConsumerConfig config)
    {
        _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
    }

    public void Consume(string topic, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                Console.WriteLine($"Consumed message '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
    }
}

