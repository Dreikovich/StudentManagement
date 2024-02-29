type GradeRecord = {
    studentID: number;
    subjectID: number;
    typeID: number;
    teacherID: number;
    gradeValue: string;
}

export type {GradeRecord};

const API_URL = 'https://localhost:7075/api/Grades';

const postGrade = async (grade: GradeRecord): Promise<GradeRecord | void> => {
    try {
        const response: Response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(grade),
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

        return await response.json();
    } catch (error) {
        console.error('Posting grade failed:', error);
        throw error;
    }
};

const gradeService = { postGrade };

export default gradeService;
