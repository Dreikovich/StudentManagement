import * as signalR from "@microsoft/signalr";

const hubUrl = "http://localhost:5192/notificationHub";

const connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl, {
        withCredentials: true,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

export async function startConnection() {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
        try {
            await connection.start();
            console.log("SignalR connected.");
        } catch (err) {
            console.error("Error while establishing SignalR connection:", err);
            setTimeout(startConnection, 10000);
        }
    } else {
        console.log("SignalR connection is already in state: ", connection.state);
    }
}

connection.onreconnecting((error) => {
    console.log(`SignalR connection lost due to error "${error}". Reconnecting.`);
});

connection.onreconnected((connectionId) => {
    console.log(`SignalR connection reestablished. Connected with connectionId "${connectionId}".`);
});

connection.onclose((error) => {
    console.log(`SignalR connection closed due to error "${error}". Starting connection.`);
    startConnection();
});

export { connection };
