using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kafka;

namespace Hermes.Services
{
    public class KafkaConsumerHostedService<TKey, TValue> : IHostedService
    {
        private readonly Consumer<TKey, TValue> _kafkaConsumer;
        private readonly ILogger<KafkaConsumerHostedService<TKey, TValue>> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _consumerTask;

        public KafkaConsumerHostedService(Consumer<TKey, TValue> kafkaConsumer, ILogger<KafkaConsumerHostedService<TKey, TValue>> logger)
        {
            _kafkaConsumer = kafkaConsumer;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka Consumer Hosted Service is starting.");
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _consumerTask = Task.Run(() => _kafkaConsumer.Consume("my_topic", _cancellationTokenSource.Token), cancellationToken);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka Consumer Hosted Service is stopping.");
            if (_consumerTask != null)
            {
                _cancellationTokenSource.Cancel();
                await Task.WhenAny(_consumerTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
    }
}
