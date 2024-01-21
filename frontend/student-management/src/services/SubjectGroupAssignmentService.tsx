type SubjectGroupAssignment = {
    subjectID: number;
    studentGroupID: number;
    subjectName?: string;
    groupName?: string;
}

export type {SubjectGroupAssignment};

const API_URL = 'https://localhost:7075/api/SubjectGroupAssignment'

const fetchSubjectGroupAssignments = async (): Promise<SubjectGroupAssignment[]> => {
    try {
        const response = await fetch(API_URL, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data: SubjectGroupAssignment[] = await response.json();
        return data;
    } catch (error) {
        console.error('Fetching subject group assignments failed:', error);
        throw error;
    }
}

const postSubjectGroupAssignment = async (subjectGroupAssignment:SubjectGroupAssignment):Promise<SubjectGroupAssignment | void> => {

    try{
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(subjectGroupAssignment)
        });
    }
    catch (error){
        console.error('Posting subject group assignment failed:', error);
        throw error;
    }
}

export const subjectGroupAssignmentService = {fetchSubjectGroupAssignments, postSubjectGroupAssignment}