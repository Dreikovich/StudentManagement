import React, { useEffect, useState } from "react";
import { notificator } from "../../services/SignalRService";

const Notifications: React.FC = () => {
    type Message = {
        messageId: string;
        userId: string;
        content: any;
    };

    const [messages, setMessages] = useState<Message[]>([]);

    useEffect(() => {
        const initializeConnection = async () => {
            await notificator.startConnection()
                .then(() =>
                {
                    console.log("SignalR connection established.")
                })
                .catch((error) => console.error("Error while establishing SignalR connection:", error));

            return () => {
                notificator.stopConnection()
                    .then(() => console.log("SignalR connection stopped."))
                    .catch((error) => console.error("Error while stopping SignalR connection:", error));
            };
        };

        initializeConnection().then(() => {
            console.log("I amd here to subscribe to notifications.")
            notificator.connection?.on("retrieve", (message:any) => {
                console.log("received message: ", message);
                setMessages((prevMessages) => [...prevMessages, message]);
            });
        });
    }, []);




    console.log(messages)

    return (
        <div>
            <h2>Notifications</h2>
            <ul>
                {messages.map((message, index) => (
                    <li key={index}>
                        {typeof message.content === 'string' ? message.content : JSON.stringify(message.content)}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Notifications;
