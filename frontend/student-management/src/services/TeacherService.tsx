type Teacher = {
    teacherID : number,
    teacherFirstName : string,
    teacherLastName : string,
    teacherEmail: string,
    degree : string
}

export type {Teacher}

const API_URL = 'https://localhost:7075/api/Teachers';

const fetchTeachers = async ():Promise<Teacher[]> =>{
    try {
        const response = await fetch(API_URL, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data: Teacher[] = await response.json();
        return data;
    } catch (error) {
        console.error('Fetching teachers failed:', error);
        throw error;
    }
}

export const TeacherService = {fetchTeachers}
