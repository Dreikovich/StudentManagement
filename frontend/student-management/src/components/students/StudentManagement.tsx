import React from 'react';
import StudentsTable from "./StudentsTable";
import StudentHeader from "./StudentHeader";
import studentService, {Student} from "../../services/StudentService";

const StudentManagement:React.FC = () => {
    const [students, setStudents] = React.useState<Student[]>([]);
    React.useEffect(() => {
        studentService.getStudents().then((students) => setStudents(students));
        console.log(students)
    }, []);
    return (
        <div>
            <StudentHeader studentsCount={students.length}/>
            <StudentsTable students={students}/>
        </div>

    )
}

export default StudentManagement;