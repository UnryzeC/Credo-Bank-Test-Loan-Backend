using System.Text.Json;
using LoanService.Application.Loan.Command.ChangeLoanRequestStatus;
using LoanService.Application.Loan.Command.CreateLoanRequest;
using LoanService.Application.Loan.Command.UpdateLoanRequest;

using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;


public class RabbitProducer : ControllerBase
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    //private const string Exchange = "loans-service";

    public enum REQUEST_TYPE
    {
        CREATE = 1,
        UPDATE = 2,
        CHANGE_STATUS = 3,
    }

    public RabbitProducer( )
    {
        var connectionFactory = new ConnectionFactory( )
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };

        _connection = connectionFactory.CreateConnection( "loan-request-publisher" );
        _channel = _connection.CreateModel( );
    }

    public IBasicProperties CreateBasicProperty( IModel channel, REQUEST_TYPE type )
    {
        var basicProperties = channel.CreateBasicProperties( );

        if ( !Enum.IsDefined( typeof( REQUEST_TYPE ), type ) ) { return basicProperties; }

        basicProperties.Headers = new Dictionary<string, object>
        {
            { "ServiceType", "LOAN_REQUEST" },
            { "RequestType", type.ToString() }
        };

        basicProperties.ContentType = "application/json";
        basicProperties.MessageId = type.ToString( );

        return basicProperties;
    }

    [HttpPost]
    public IActionResult SendRequest( in object request, REQUEST_TYPE type, CancellationToken cancellationToken )
    {
        try
        {
            var basicProperties = CreateBasicProperty( _channel, type );
            
            if ( !basicProperties.Headers.TryGetValue( "ServiceType", out var serviceType ) )
            { 
                throw new Exception( string.Format( "[ERROR] SendRequest: Unknown ServiceType." ) );
            }

            if ( serviceType.ToString( ) != "LOAN_REQUEST" )
            {
                throw new Exception( string.Format( "[ERROR] SendRequest: IBasicProperties creation failed, with type {0}.", type ) );
            }

            cancellationToken.ThrowIfCancellationRequested( );

            //channel.QueueDeclare( queue: RabbitService.QUEUE_NAME, durable: true, exclusive: false, autoDelete: false, null );
            _channel.BasicPublish( exchange: "", routingKey: "loan-request-service", mandatory: true, basicProperties: basicProperties, body: JsonSerializer.SerializeToUtf8Bytes( request ) );
        }
        catch ( Exception ex )
        {
            throw new Exception( ex.Message );
        }

        return Ok( );
    }

    public IActionResult CreateRequest( in LoanCreateRequestCommand request, CancellationToken cancellationToken )
    {
        return SendRequest( request, REQUEST_TYPE.CREATE, cancellationToken );
    }

    public IActionResult UpdateRequest( in LoanRequestUpdateCommand request, CancellationToken cancellationToken )
    {
        return SendRequest( request, REQUEST_TYPE.UPDATE, cancellationToken );
    }

    public IActionResult ChangeStatusRequest( in LoanChangeRequestStatusCommand request, CancellationToken cancellationToken )
    {
        return SendRequest( request, REQUEST_TYPE.CHANGE_STATUS, cancellationToken );
    }
}