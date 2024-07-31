using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Kafka;

public class Producer<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _producer;

    public Producer(ProducerConfig config)
    {
        _producer = new ProducerBuilder<TKey, TValue>(config).Build();
    }

    public async Task ProduceAsync(string topic, TValue message)
    {
        try
        {
            var deliveryReport = await _producer.ProduceAsync(topic, new Message<TKey, TValue> { Value = message });
            Console.WriteLine($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'");
        }
        catch (ProduceException<TKey, TValue> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}

