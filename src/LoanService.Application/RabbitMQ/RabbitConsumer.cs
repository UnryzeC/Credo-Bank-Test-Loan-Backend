using System.Text;
using System.Text.Json;
using System.Threading.Channels;

using LoanService.Application.Loan.Command.ChangeLoanRequestStatus;
using LoanService.Application.Loan.Command.CreateLoanRequest;
using LoanService.Application.Loan.Command.UpdateLoanRequest;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LoanService.Application.RabbitMQ;
public sealed class RabbitConsumer : BackgroundService
{
    private readonly IServiceProvider _service;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitConsumer( IServiceProvider serviceProvider )
    {
        _service = serviceProvider;

        var connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true,
        };

        _connection = connectionFactory.CreateConnection( "loan-request-consumer" );
        _channel = _connection.CreateModel( );
    }

    protected override Task ExecuteAsync( CancellationToken cancellationToken )
    {
        try
        {
            _channel.QueueDeclare( queue: "loan-request-service", durable: true, exclusive: false, autoDelete: false, arguments: null );

            var consumer = new AsyncEventingBasicConsumer( _channel );

            consumer.Received += async ( model, ea ) =>
            {
                try
                {
                    var properties = ea.BasicProperties;
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString( body );

                    if ( !properties.Headers.TryGetValue( "RequestType", out var requestTypeBytes ) )
                    { 
                        throw new Exception( string.Format( "[ERROR] RabbitConsumer: Failed to parse ServiceType." ) );
                    }

                    var requestType = Encoding.UTF8.GetString( (byte[])requestTypeBytes );
                    Enum.TryParse( requestType, out RabbitProducer.REQUEST_TYPE type );

                    using var scope = _service.CreateScope( );
                    var _mediator = scope.ServiceProvider.GetRequiredService<ISender>( );

                    switch( type )
                    {
                        case RabbitProducer.REQUEST_TYPE.CREATE:
                        {
                            var request = JsonSerializer.Deserialize<LoanCreateRequestCommand>( message ); if ( request == null ) { break; }
                            await _mediator.Send( request, cancellationToken );

                            break;
                        }
                        case RabbitProducer.REQUEST_TYPE.UPDATE:
                        {
                            var request = JsonSerializer.Deserialize<LoanRequestUpdateCommand>( message ); if ( request == null ) { break; }
                            await _mediator.Send( request, cancellationToken );

                            break;
                        }
                        case RabbitProducer.REQUEST_TYPE.CHANGE_STATUS:
                        {
                            var request = JsonSerializer.Deserialize<LoanChangeRequestStatusCommand>( message ); if ( request == null ) { break; }
                            await _mediator.Send( request, cancellationToken );

                            break;
                        }
                        default: break;
                    }

                    _channel.BasicAck( ea.DeliveryTag, false );
                }
                catch ( Exception ex )
                {
                    _channel.BasicNack( ea.DeliveryTag, false, false );
                    Console.WriteLine( ex.ToString() );
                }
            };

            _channel.BasicConsume( queue: "loan-request-service", autoAck: false, consumer: consumer );
            //_channel.BasicConsume( queue: "loan-request-service", autoAck: false, consumerTag: "", noLocal: false, exclusive: false, arguments: null, consumer: consumer );
        }
        catch ( Exception ex )
        {
            throw;
        }

        return Task.CompletedTask;
    }
}
