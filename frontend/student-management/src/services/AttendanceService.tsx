import {Attendance} from "../interfaces/Attendance/Attendance";

const API_BASE_URL = 'https://localhost:7075/api/Attendance';
const getAttendance = async (groupName:string, academicYear:string, subjectName: string, sessionTypeName: string):Promise<Attendance[]> => {
    try {
        const encodedGroupName = encodeURIComponent(groupName);
        const encodedAcademicYear = encodeURIComponent(academicYear);
        const encodedSubjectName = encodeURIComponent(subjectName);
        const encodedSessionTypeName = encodeURIComponent(sessionTypeName);
        const dynamicUrl = `${API_BASE_URL}?group_name=${encodedGroupName}&academic_year=${encodedAcademicYear}&subject_name=${encodedSubjectName}&session_type=${encodedSessionTypeName}`;
        const response = await fetch(dynamicUrl);
        console.log(response)
        if (!response.ok) {
            throw new Error('Something went wrong');
        }
        return await response.json();
    }
    catch (e) {
        console.log(e);
        throw e;
    }
};
const attendanceService = { getAttendance };
export default attendanceService;