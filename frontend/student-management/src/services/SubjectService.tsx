type Teacher = {
    teacherID: number;
    teacherFirstName: string;
    teacherLastName: string;
    teacherEmail: string;
    degree: string | null;
};

type subjectComponents = {
    typeID: number;
    typeName?: string;
    hours: number;
    teacher?: Teacher;
    teacherID?: number;
};

type Subject = {
    subjectID?: number;
    subjectName: string;
    subjectComponents: subjectComponents[];
};

type SubjectData = Subject[];

export type { SubjectData, Subject, subjectComponents };

const API_URL = 'https://localhost:7075/api/Subject';
const fetchSubjects = async (): Promise<SubjectData> => {
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

        const data: SubjectData = await response.json();
        return data;
    } catch (error) {
        console.error('Fetching subjects failed:', error);
        throw error;
    }
};

const postSubjectWithComponents = async (subject: Subject): Promise<Subject | void> => {
    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(subject)
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
            return;
        }
    } catch (e) {
        console.error('Error posting subject:', e);
        throw e;
    }
}

export const SubjectService = {
    fetchSubjects, postSubjectWithComponents
};