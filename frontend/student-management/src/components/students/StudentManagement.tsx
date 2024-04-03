import React from 'react';
import StudentsTable from "./StudentsTable";
import StudentHeader from "./StudentHeader";
import studentService, {Student} from "../../services/StudentService";
import { useDispatch } from 'react-redux';
import { setStudents } from '../../features/students/StudentSlice'

const StudentManagement:React.FC = () => {
    const dispatch = useDispatch();
    React.useEffect(() => {
        studentService.getStudents().then((students) => dispatch(setStudents(students)));
    }, [dispatch]);
    return (
        <div>
            <StudentHeader />
            <StudentsTable />
        </div>

    )
}

export default StudentManagement;