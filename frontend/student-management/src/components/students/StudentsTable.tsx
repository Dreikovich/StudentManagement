import React from 'react'
import studentService, {Student} from "../../services/StudentService";
import {faEdit, faEye, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useSelector} from "react-redux";
import {RootState} from "../../store/store";

const StudentsTable: React.FC = () =>{
    const students = useSelector((state: RootState) => state.students.students);
    return(
        <div className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
            <table className="min-w-full leading-normal">
                <thead>
                <tr>
                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                        Name
                    </th>
                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                        Group
                    </th>
                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                        Gender
                    </th>
                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                        Email
                    </th>
                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                        Actions
                    </th>
                </tr>
                </thead>
                <tbody>
                {students.map(student => (
                    <tr key={student.studentID} className="hover:bg-gray-600 transition-colors duration-150">
                        <td className="px-5 py-4 border-b border-gray-200 bg-white text-sm">
                            <div className="flex items-center">
                                <div className="flex-shrink-0">
                                    <img
                                        className="h-10 w-10 rounded-full object-cover"
                                        src={student.gender === 'male' ? "https://www.pngkey.com/png/detail/114-1149878_setting-user-avatar-in-specific-size-without-breaking.png" : "https://www.pngkey.com/png/detail/55-556528_pic-avatars-for-jira-star-wars.png"}
                                        alt="avatar icon"
                                    />

                                </div>
                                <div className="ml-3">
                                    <p className="text-gray-900 whitespace-no-wrap">
                                    {student.firstName} {student.lastName}
                                    </p>
                                </div>
                            </div>
                        </td>
                        <td className="px-5 py-4 border-b border-gray-200 bg-white text-sm">
                            <p className="text-gray-900 whitespace-no-wrap">{student.groupName}</p>
                        </td>
                        <td className="px-5 py-4 border-b border-gray-200 bg-white text-sm">
                            <p className="text-gray-900 whitespace-no-wrap">
                                {student.gender}
                            </p>
                        </td>
                        <td className="px-5 py-4 border-b border-gray-200 bg-white text-sm">
                            <p className="text-gray-900 whitespace-no-wrap">{student.email}</p>
                        </td>

                        <td className="px-5 py-4 border-b border-gray-200 bg-white text-sm items-center space-x-3">
                            <a href="#"
                               className="text-blue-600 hover:text-blue-900 transition duration-300 ease-in-out">
                                <FontAwesomeIcon icon={faEdit} className="mr-3"/>
                            </a>
                            <a href="#" className="text-red-600 hover:text-red-900 transition duration-300 ease-in-out">
                                <FontAwesomeIcon icon={faTrashAlt} className="mr-3"/>
                            </a>
                            <a href="#"
                               className="text-green-600 hover:text-green-900 transition duration-300 ease-in-out">
                                <FontAwesomeIcon icon={faEye} className="mr-3"/>
                            </a>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    )
}

export default StudentsTable;