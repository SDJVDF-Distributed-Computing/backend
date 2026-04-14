using Backend.Console;

var certPath          = Environment.GetEnvironmentVariable("SMP_CERT_PATH") ?? "certs/smpserver.cer";
var session           = Session.Create();
var sessionService    = new SessionService(certPath);
var messageRepository = new InMemoryMessageRepository();

var cli = new SecureMessageProtocolCLI([
    new ConnectCLICommand   (new ConnectCommandHandler    (session, sessionService)),
    new LoginCLICommand     (new AuthenticateCommandHandler(session, sessionService)),
    new UploadCLICommand    (new UploadCommandHandler     (session, sessionService)),
    new DownloadCLICommand  (new DownloadCommandHandler   (session, messageRepository, sessionService)),
    new ShowMessagesCLICommand(new GetCachedMessagesQueryHandler(messageRepository)),
    new ShowStatusCLICommand(new GetSessionStatusQueryHandler(session)),
    new QuitCLICommand      (new DisconnectCommandHandler (session, sessionService)),
]);

await cli.RunAsync();
