import * as signalR from "@microsoft/signalr";

const hubUrl = "http://localhost:5012/notificationHub";
const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MGNkZGVhYS0yNzNjLTRkMDUtYTI1Ny02MTZjYWVkNDk3NjgiLCJ1bmlxdWVfbmFtZSI6Imt1QGdtYWlsLmNvbSIsImp0aSI6IjdiY2FiMjMxLWE3NjQtNDUwMy1hNmQ1LTAyMzU1MzkwMTMwOSIsIm5hbWVpZCI6IjkwY2RkZWFhLTI3M2MtNGQwNS1hMjU3LTYxNmNhZWQ0OTc2OCIsIm5iZiI6MTcxNjc3MzI2OCwiZXhwIjoxNzE2ODU5NjY4LCJpYXQiOjE3MTY3NzMyNjh9.WGyvSob6HDgIDMDAVNRupQ7e5rkH5Al12b5fN-M6roc";

class Notificator {

    connection: signalR.HubConnection | null = null;

    constructor() {

    }

    private async initializeConnection(): Promise<void> {
        if (!this.connection) {
            this.connection = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Debug)
                .withKeepAliveInterval(1000 * 60 * 5)
                .withServerTimeout(1000 * 60 * 10)
                .withUrl(hubUrl,{
                    skipNegotiation: true,
                    accessTokenFactory: () => token,
                    withCredentials: false,
                    transport: signalR.HttpTransportType.WebSockets,
                    logMessageContent: true,

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
                try {
                    if (this.connection) {
                        await this.connection.invoke("NotifyConnectedUsers")
                            .then(() => console.log("Notified connected users"))
                            .catch((error) => console.error("Error while notifying connected users:", error));

                        // Subscribe to notifications right after the connection is started
                        this.connection.on("retrieve", message => {
                            console.log("Received message: ", message);
                        });
                    }
                } catch (error) {
                    console.error("Error while subscribing to notifications:", error);
                }
            } catch (err) {
                console.error("Error while starting SignalR connection:", err);
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
            this.connection.on("retrieve", callback);
            console.log("Subscribed to notification");
        } else {
            console.error("Connection is not established. Call startConnection first.");
        }
    }
}

export const notificator = new Notificator();
