import React, {useState} from 'react'
import {faEdit, faEye, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useSelector} from "react-redux";
import {RootState} from "../../store/store";
import {Column} from "../../interfaces/Column/Column";

const StudentsTable: React.FC = () =>{
    const students = useSelector((state: RootState) => state.students.students);
    const columns = useSelector((state: RootState) => state.tableColumns.columns) as Column[]

    return(
        <>
            <div className="bg-white shadow-md rounded-lg px-6 py-4 mb-4 flex flex-col my-2">
                <div className="overflow-x-auto sm:-mx-6 lg:-mx-8">
                    <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
                        <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
                            {columns.length >0?
                            <table className="min-w-full divide-y divide-gray-200">
                                <thead className="bg-gray-50">
                                <tr>
                                        {columns.map((column) => (
                                            <th key={column} className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">{column}</th>
                                        ))}
                                </tr>
                                </thead>

                                <tbody className="bg-white divide-y divide-gray-200">
                                {students.map((student, idx) => (
                                    <tr key={idx} className="hover:bg-gray-50">
                                        {columns.includes("Name") &&
                                            <div className="flex items-center ml-3">
                                                <img
                                                    className="h-10 w-10 rounded-full object-cover"
                                                    src={student.gender === 'male' ? "https://www.pngkey.com/png/detail/114-1149878_setting-user-avatar-in-specific-size-without-breaking.png" : "https://www.pngkey.com/png/detail/55-556528_pic-avatars-for-jira-star-wars.png"}
                                                    alt="avatar icon"
                                                />
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{student.firstName} {student.lastName}</td>
                                            </div>
                                        }
                                        {columns.includes("GroupName") &&
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{student.groupName}</td>}
                                        {columns.includes("Gender") &&
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{student.gender}</td>}
                                        {columns.includes("Email") &&
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{student.email}</td>}
                                        {columns.includes("Actions") &&
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                                <a href="#" className="text-indigo-600 hover:text-indigo-700">
                                                    <FontAwesomeIcon icon={faEdit} className="mr-3"/>
                                                </a>
                                                <a href="#" className="text-red-600 hover:text-red-700 ml-4">
                                                    <FontAwesomeIcon icon={faTrashAlt} className="mr-3"/>
                                                </a>
                                                <a href="#"
                                                   className="text-green-600 hover:text-green-700 ml-4">
                                                    <FontAwesomeIcon icon={faEye} className="mr-3"/>
                                                </a>
                                            </td>
                                        }
                                    </tr>
                                ))}
                                </tbody>
                            </table>
                            :
                            <div className="flex justify-center items-center h-screen" style={{"marginTop": "-150px"}}>
                                <p>No columns selected</p>
                            </div>
                            }
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default StudentsTable;