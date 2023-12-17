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

export type {Attendance};