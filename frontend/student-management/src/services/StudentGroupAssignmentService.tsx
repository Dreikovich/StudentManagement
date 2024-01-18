interface StudentGroupAssignment{
    StudentID: number,
    StudentGroupID: number
}

export type {StudentGroupAssignment}

const API_URL = 'https://localhost:7075/api/StudentGroupAssignment';

const addStudentGroupAssignment = async (studentGroupAssignment:StudentGroupAssignment ):Promise<StudentGroupAssignment|void> =>{
    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(studentGroupAssignment)
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
        console.error('Error posting student:', e);
        throw e;
    }
}

const studentGroupAssignmentService = {addStudentGroupAssignment}
export default studentGroupAssignmentService;


