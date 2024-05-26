 import React from 'react';
import 'tailwindcss/tailwind.css';

interface Event {
    id: number;
    name: string;
    day: string;
    startTime: string; // in HH:MM format
    endTime: string; // in HH:MM format
    color?: string;
}

const events: Event[] = [
    { id: 1, name: 'Meeting with Team', day: 'Monday', startTime: '09:00', endTime: '10:00',  color: 'bg-blue-500'},
    { id: 2, name: 'Lunch Break', day: 'Monday', startTime: '12:00', endTime: '13:00', color: 'bg-green-500' },
    { id: 3, name: 'Project Presentation', day: 'Wednesday', startTime: '11:00', endTime: '12:30', color: 'bg-purple-500' },
    { id: 4, name: 'PMP', day: 'Monday', startTime: '14:00', endTime: '16:30', color: 'bg-purple-500' },
    { id: 5, name: 'Lecture on Quantum Physics', day: 'Tuesday', startTime: '10:00', endTime: '12:00', color: 'bg-yellow-500' },
    { id: 6, name: 'Study Group Meeting', day: 'Tuesday', startTime: '13:00', endTime: '15:00', color: 'bg-red-500' },
    { id: 7, name: 'Lab Work', day: 'Wednesday', startTime: '14:00', endTime: '16:00', color: 'bg-green-500' },
    { id: 8, name: 'Office Hours with Professor', day: 'Thursday', startTime: '11:00', endTime: '12:00', color: 'bg-blue-500' },
    { id: 9, name: 'Library Study Time', day: 'Thursday', startTime: '13:00', endTime: '15:00', color: 'bg-indigo-500' },
    { id: 10, name: 'Group Project Work', day: 'Friday', startTime: '10:00', endTime: '12:00', color: 'bg-pink-500' },
    { id: 11, name: 'Seminar on AI', day: 'Friday', startTime: '13:00', endTime: '15:00', color: 'bg-gray-500' },
];

// Days of the week for header
const daysOfWeek: string[] = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

const Calendar: React.FC = () => {
    const hourHeight = 4; // 4rem for each hour


    // Helper function to convert time string to the number of hours
    const timeToHours = (time: string): number => {
        const [hours, minutes] = time.split(':').map(Number);
        return hours + minutes / 60;
    };

    // Helper function to calculate top and height for event styling
    const getEventStyle = (startTime: string, endTime: string) => {
        const startHours = timeToHours(startTime) - 6;
        const endHours = timeToHours(endTime) - 6;
        const durationHours = endHours - startHours;

        // Calculate the top position in 'rem' based on the start time
        const topPositionRem = (startHours * 4) + 1.75;

        // Calculate the height of the event in 'rem'
        const eventHeightRem = (durationHours * 4) ;
        return {
            top: `${topPositionRem}rem`,
            height: `${eventHeightRem}rem`,
        };
    };

    return (
        <div className="flex flex-row min-h-screen overflow-x-auto">
            {/* Time Column */}
            <div className="flex flex-col w-16 bg-gray-100 text-center mt-[4rem] max-h-[60rem] min-w-[4rem]">

                {Array.from({ length: 15 }, (_, index) => ( // 6 AM to 8 PM
                    <div key={index} className="h-16 border-b text-xs flex items-center justify-center min-h-[4rem] ">
                        {`${String(index + 6).padStart(2, '0')}:00`}
                    </div>
                ))}

            </div>

            {/* Days Columns */}
            <div className="flex flex-1 text-sm">
                {daysOfWeek.map(day => (
                    <div key={day} className="flex-1 border-l border-t border-r border-gray-300 max-h-[64.1rem] min-w-[5rem]" style={{
                        "backgroundImage": "linear-gradient(to bottom, transparent 99%, gray 99%)",
                        "backgroundSize" : "100% 4rem"
                    }}>
                        <div className="text-center font-bold py-2 bg-gray-200">{day}</div>
                        <div className="relative " style={{ maxHeight: `${14 * hourHeight}rem` }}>
                            {/* Render events for this day */}
                            {events.filter(event => event.day === day).map(event => (
                                <div
                                    key={event.id}
                                    className={`absolute left-0 w-full border rounded px-1 py-0.5 text-white text-center ${event.color}`}
                                    style={getEventStyle(event.startTime, event.endTime)}
                                >
                                    <span>{event.name}</span>
                                    <div className="absolute bottom-0 right-0 mr-3 mb-1">
                                        <span>{event.startTime} - {event.endTime}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Calendar;
