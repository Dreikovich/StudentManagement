import React, { useEffect, useState } from "react";
import { connection, startConnection } from "../../services/SignalRService";

const Notifications: React.FC = () => {


    const [messages, setMessages] = useState<string[]>([]);

    useEffect(() => {
        const initializeConnection = async () => {
            await startConnection();

            connection.on("Retrieve", (content: string) => {
                setMessages((prevMessages) => [...prevMessages,  content ]);
            });
        };

        initializeConnection();

        return () => {
            connection.off("Retrieve");
        };
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
