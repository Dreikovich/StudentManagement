import * as signalR from "@microsoft/signalr";

const hubUrl = "http://localhost:5192/notificationHub";

class Notificator {
    private connection: signalR.HubConnection | null = null;

    private initializeConnection(): void {
        if (!this.connection) {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl(hubUrl)
                .configureLogging(signalR.LogLevel.Information)
                .build();

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

    public async subscribeNotification(callback: (content: string) => void): Promise<void> {
        if (this.connection) {
            this.connection.on("Retrieve", callback);
            await this.stopConnection();
        } else {
            console.error("Connection is not established. Call startConnection first.");
        }
    }
}

export const notificator = new Notificator();
