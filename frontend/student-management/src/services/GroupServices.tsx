import {Student} from "./StudentService";
import attendanceService from "./AttendanceService";

interface Group {
    studentGroupID: number;
    students: Student[];
    groupName: string;
    academicYear: string;
}

export type {Group};

const API_URL = 'https://localhost:7075/api/StudentGroups';

const getGroups = async (): Promise<Group[]> => {
    try{
        const response = await fetch(API_URL);
        console.log(response)
        if(!response.ok){
            throw new Error('Something went wrong');
        }
        return await response.json();
    }
    catch (e){
        console.log(e);
        throw e;
    }
}

const groupService = {getGroups}
export default groupService;