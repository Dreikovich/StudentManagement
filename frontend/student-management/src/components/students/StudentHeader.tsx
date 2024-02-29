import React from 'react';
import {Student} from "../../services/StudentService";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faSearch } from '@fortawesome/free-solid-svg-icons';


interface StudentHeaderProps {
    studentsCount: number;
}

const StudentHeader: React.FC<StudentHeaderProps> = ({ studentsCount}) => {
    return (
        <div className="bg-white py-4 px-8 flex justify-between items-center border-b-2">
            <span className="text-lg font-bold">Students ({studentsCount})</span>
            <div className="flex space-x-4">
                <div className="flex items-center space-x-2">
                    <input
                        type="text"
                        placeholder="Search"
                        className="border-2 rounded-lg p-2"
                    />
                    <button className="text-gray-500 focus:outline-none">
                        <FontAwesomeIcon icon={faSearch}/>
                    </button>
                </div>
                <button
                    className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded transition duration-300 ease-in-out">
                    Add Student
                </button>
            </div>
        </div>

    );
}

export default StudentHeader;