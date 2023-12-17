// @ts-ignore
interface Student {
    studentID: number;
    firstName: string;
    lastName: string;
    groupName: string;
}

export type {Student};

const API_URL = 'https://localhost:7075/api/Students';

const getStudents = async (): Promise<Student[]> => {
    try{
        const response = await fetch(API_URL);
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

const studentService = {getStudents}
export default studentService;