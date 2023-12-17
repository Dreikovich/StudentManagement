import React, {useEffect, useState} from 'react'
import attendanceService from "../../services/AttendanceService";
import {Attendance} from "../../interfaces/Attendance/Attendance";

const AttendanceList: React.FC =()=>{
    const [attendanceData, setAttendanceData] = useState<Attendance[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(()=>{
        attendanceService.getAttendance("IPAUT-171", "2023/2024")
            .then((attendance)=>setAttendanceData(attendance))
            .finally(()=>setLoading(false))

    },[])
    return (
        <div className="bg-gray-200 min-h-screen flex flex-col">
            <div className="bg-gray-800 text-white py-4">
                <h2 className="text-2xl font-bold ml-4">Attendance List</h2>
            </div>
            <div className="flex-1 overflow-y-auto container mx-auto p-4">
                {loading ? (
                    <p className="text-gray-600 text-center my-8">Loading attendance data...</p>
                    )  : (
                    <table className="min-w-full bg-white border rounded-lg overflow-hidden">
                        <thead className="bg-gray-200">
                        <tr>
                            <th className="py-2 px-4 text-left">Student ID</th>
                            <th className="py-2 px-4 text-left">Name</th>
                            <th className="py-2 px-4 text-left">Status</th>
                            <th className="py-2 px-4 text-left">Date</th>
                            <th className="py-2 px-4 text-left">Time</th>
                            <th className="py-2 px-4 text-left">Comments</th>
                        </tr>
                        </thead>
                        <tbody>
                        {attendanceData && attendanceData.map((attendance) => (
                            <tr key={attendance.attendanceID} className="hover:bg-gray-100">
                                <td className="py-4 px-4 border-b">{attendance.studentID}</td>
                                <td className="py-4 px-4 border-b">{`${attendance.student.firstName} ${attendance.student.lastName}`}</td>
                                <td className={`py-4 px-4 border-b ${
                                    attendance.status === 'Present' ? 'text-green-600' :
                                    attendance.status === 'Absent' ? 'text-red-600' :
                                    attendance.status === 'Late' ? 'text-yellow-600' :
                                    ''
                                }`}>
                                    {attendance.status}
                                </td>
                                <td className="py-4 px-4 border-b">{attendance.date}</td>
                                <td className="py-4 px-4 border-b">{attendance.time}</td>
                                <td className="py-4 px-4 border-b">{attendance.comments}</td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                )}
            </div>
        </div>
    );
}
export default AttendanceList