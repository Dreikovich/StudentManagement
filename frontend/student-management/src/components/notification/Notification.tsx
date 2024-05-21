import React, { useEffect, useState } from "react";
import  {notificator}  from "../../services/SignalRService";

const Notifications: React.FC = () => {


    const [messages, setMessages] = useState<string[]>([]);

    useEffect(() => {
        const initializeConnection = async () => {
            await notificator.startConnection()
                .then(() => console.log("SignalR connection established."))
                .catch((error) => console.error("Error while establishing SignalR connection:", error));

            return () => {
                notificator.stopConnection()
                    .then(() => console.log("SignalR connection stopped."))
                    .catch((error) => console.error("Error while stopping SignalR connection:", error));
            }
        };

        initializeConnection();
    }, []);

    useEffect(() => {
        notificator.subscribeNotification((content) => {
            setMessages((prevMessages) => [...prevMessages, content]);
        });
    }, []);

    console.log(messages);
    return (
        <div>
            <h2>Notifications</h2>
            <ul>
                {messages.map((message, index) => (
                    <li key={index}>{`${message}`}</li>
                ))}
            </ul>
        </div>
    );
};

export default Notifications;
