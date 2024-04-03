// @ts-ignore
interface Student {
    studentID?: number;
    firstName: string;
    lastName: string;
    email: string;
    gender: string;
    groupName?: string;
    status:string;
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

const postStudent = async (student: Student): Promise<Student | void> => {
    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(student)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        // Check if the response has content
        const contentLength = response.headers.get('Content-Length');
        if (contentLength && Number(contentLength) > 0) {
            const data = await response.json();
            return data;
        } else {
            // Handle the case where there's no content
            console.log('POST successful, no content returned');
            return student;
        }
    } catch (e) {
        console.error('Error posting student:', e);
        throw e;
    }
}

const studentService = {getStudents, postStudent}
export default studentService;