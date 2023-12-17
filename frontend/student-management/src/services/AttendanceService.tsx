import {Attendance} from "../interfaces/Attendance/Attendance";

const API_BASE_URL = 'https://localhost:7075/api/Attendance';
const getAttendance = async (groupName:string, academicYear:string):Promise<Attendance[]> => {
    try {
        const encodedGroupName = encodeURIComponent(groupName);
        const encodedAcademicYear = encodeURIComponent(academicYear);
        const dynamicUrl = `${API_BASE_URL}?group_name=${encodedGroupName}&academic_year=${encodedAcademicYear}`;
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