import React from 'react'
import studentService,{ Student } from "../../services/StudentService";

// @ts-ignore
const StudentList: React.FC = () =>{
    const [students, setStudents] = React.useState<Student[]>([]);
    React.useEffect(() => {
        studentService.getStudents().then((students) => setStudents(students));
    }, []);
    return (
        <div>
            <h3 className="text-xl font-bold mb-2">Student List</h3>

            <div className="overflow-x-auto">
                <table className="min-w-full bg-white border rounded-lg overflow-hidden">
                    <thead className="bg-gray-200">
                    <tr>
                        <th className="py-2 px-4 text-left">ID</th>
                        <th className="py-2 px-4 text-left">Name</th>
                        <th className="py-2 px-4 text-left">LastName</th>
                        <th className="py-2 px-4 text-left">GroupName</th>

                    </tr>
                    </thead>
                    <tbody>
                    {students.map((student) => (
                        <tr key={student.id} className="hover:bg-gray-100">
                            <td className="py-4 px-4 border-b">{student.id}</td>
                            <td className="py-4 px-4 border-b">{student.firstName}</td>
                            {/*<td className="py-4 px-4 border-b">{student.isActive?"active":"not active"}</td>*/}
                            <td className="py-4 px-4 border-b">{student.lastName}</td>
                            <td className="py-4 px-4 border-b">{student.groupName}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default StudentList;
