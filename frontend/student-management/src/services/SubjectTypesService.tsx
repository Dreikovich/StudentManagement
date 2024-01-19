type SubjectTypes = {
    typeID: number;
    typeName: string;
}

export type {SubjectTypes}

const API_URL = 'https://localhost:7075/api/SubjectTypes';

const fetchSubjectTypes = async ():Promise<SubjectTypes[]> =>{
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

        const data: SubjectTypes[] = await response.json();
        return data;
    } catch (error) {
        console.error('Fetching subject types failed:', error);
        throw error;
    }
}

export const SubjectTypesService = {fetchSubjectTypes}