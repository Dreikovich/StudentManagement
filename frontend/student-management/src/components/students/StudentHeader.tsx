import React, {useEffect, useState} from 'react';
import {Student} from "../../services/StudentService";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../store/store";
import {setColumns, addColumn, removeColumn} from "../../features/students/TableColumnSlice";
import AddStudentForm from "../Forms/AddStudentForm/AddStudentForm";
import GenericFilter from "../filters/GenericFilter";
import Dropdown from "../Dropdown/Dropdown";


const StudentHeader: React.FC = () => {
    const studentsCount = useSelector((state:RootState) => state.students.students.length);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const dispatch = useDispatch();
    useEffect(() => {
        //dispatch  - it wat we use to send the actions of the reducer to the store
        dispatch(setColumns(["Name", "GroupName", "Gender", "Email", "Actions"]));
    }, [dispatch]);

    const columns = useSelector((state: RootState) => state.tableColumns.columns);
    console.log(columns)

    const closeModal = () => {
        setIsModalOpen(false);
    };

    return (
        <div className="bg-white py-4 px-8 flex justify-between items-center border-b-2">
            <span className="text-lg font-bold">Students ({studentsCount})</span>
            <div className="flex space-x-4">
                <div className="flex items-center">
                    <Dropdown options={["Name", "GroupName", "Gender", "Email", "Actions"]}
                              dropdownName={"Columns"} type={"checkbox"}
                              stateSelector={columns}
                              addAction={addColumn}
                              removeAction={removeColumn}/>
                    <div className="flex items-center max-w-md mx-auto bg-white rounded-full ml-3">
                        <input
                            type="text"
                            placeholder="Search"
                            className="border-2 rounded-lg p-2"
                        />
                        <button className="bg-purple-600 text-white rounded-full p-2 hover:bg-purple-700 focus:outline-none w-10 h-10 ml-1 flex items-center justify-center">
                            <FontAwesomeIcon icon={faSearch}/>
                        </button>
                    </div>

                    <GenericFilter/>
                </div>
                <button onClick={() => setIsModalOpen(true)}
                        className="bg-purple-600 text-white font-semibold py-2 px-4 rounded-lg shadow-md hover:bg-purple-700 transition duration-300 ease-in-out flex items-center">
                Add Student
                </button>

            </div>
            <AddStudentForm isOpen = {isModalOpen} closeModal={closeModal}/>
        </div>
    );
}

export default StudentHeader;