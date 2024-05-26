import * as signalR from "@microsoft/signalr";

const hubUrl = "http://localhost:5192/notificationHub";
const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MGNkZGVhYS0yNzNjLTRkMDUtYTI1Ny02MTZjYWVkNDk3NjgiLCJ1bmlxdWVfbmFtZSI6Imt1QGdtYWlsLmNvbSIsImp0aSI6ImMyZDVjMzA2LTBhZjYtNGEzYy05MWU3LTAwNzk0OGU3YWNkYSIsIm5hbWVpZCI6IjkwY2RkZWFhLTI3M2MtNGQwNS1hMjU3LTYxNmNhZWQ0OTc2OCIsIm5iZiI6MTcxNjcyNDkzMywiZXhwIjoxNzE2ODExMzMzLCJpYXQiOjE3MTY3MjQ5MzN9.crjkzp8yaf-ftPg6GREdaayUWo7AjRQPCERS6kWaXQs";

class Notificator {
    private connection: signalR.HubConnection | null = null;

    private initializeConnection(): void {
        if (!this.connection) {
            this.connection = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Debug)
                .withUrl(hubUrl,{
                    skipNegotiation: true,
                    accessTokenFactory: () => token,
                    withCredentials: false,
                    transport: signalR.HttpTransportType.WebSockets
                })
                .configureLogging(signalR.LogLevel.Information)
                .withAutomaticReconnect()
                .build();


            this.connection.onreconnected((connectionId) => {
                console.log('SignalR Reconnected: ', connectionId);
            });

            this.connection.onclose(async () => {
                console.log("SignalR connection closed.");
            });
        }
    }

    public async startConnection(): Promise<void> {
        this.initializeConnection();

        if (this.connection && this.connection.state === signalR.HubConnectionState.Disconnected) {
            try {
                await this.connection.start();
                console.log("SignalR connected.");
                await this.notifyConnectedUsers();
            } catch (err) {
                console.error("Error while establishing SignalR connection:", err);
                setTimeout(this.startConnection.bind(this), 10000);
            }
        }
    }

    public async stopConnection(): Promise<void> {
        if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
            try {
                await this.connection.stop();
                console.log("SignalR disconnected.");
            } catch (err) {
                console.error("Error while stopping SignalR connection:", err);
            }
        }
    }

    public async subscribeNotification(callback: (message: any) => void): Promise<void> {
        if (this.connection) {
            this.connection.on("Retrieve", callback);
            console.log("Subscribed to notification");
        } else {
            console.error("Connection is not established. Call startConnection first.");
        }
    }


    public async notifyConnectedUsers(): Promise<void> {
        if (this.connection) {
            console.log("I am tryying to notify connected users");
            await this.connection.invoke("NotifyConnectedUsers").catch((err) => console.error(err));
        }
    }
}

export const notificator = new Notificator();
