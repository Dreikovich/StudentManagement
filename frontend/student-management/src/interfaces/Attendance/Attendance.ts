interface Attendance {
    attendanceID: number;
    studentID: number;
    teacherID: number;
    sessionID: number;
    student: {
        firstName: string;
        lastName: string;
        email: string;
    };
    subjectID: number;
    subjectName: string;
    sessionName: string;
    groupID: number;
    groupName: string;
    date: string;
    time: string;
    status: 'Present' | 'Late' | 'Absent'
    comments: string;
    auditorium: string;
    teacher: {
        teacherFirstName: string;
        teacherLastName: string;
        teacherEmail: string;
        degree: string;
    };
}

interface AttendanceRecord {
    subjectID: number;
    groupID: number;
    typeID: number;
    studentID: number;
    teacherID: number;
    date: string; // or Date ???
    status: string;
    comments: string;
    auditorium: string;
}

export type {Attendance, AttendanceRecord};