import React, { useState, FormEvent } from 'react';
import studentService from "../../../services/StudentService";
import {toast} from "react-toastify";
import {useDispatch} from "react-redux";
import {addStudent} from "../../../features/students/StudentSlice";
import {v4 as uuidv4} from 'uuid';
interface ModalProps {
    isOpen: boolean;
    closeModal: ()=>void;
}
const AddStudentForm: React.FC<ModalProps > = ({isOpen, closeModal}) => {
    const [firstName, setFirstName] = useState<string>('');
    const [lastName, setLastName] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [gender, setGender] = useState<string>('');
    const [status, setStatus] = useState<string>('')

    const dispatch = useDispatch();

    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {

        e.preventDefault();
        const studentUuid = uuidv4();
        const studentData = { firstName, lastName, email, gender, status, studentUuid };

        studentService.postStudent(studentData )
            .then((newStudent) => {
                dispatch(addStudent(newStudent));

                toast.success("Student created successfully!");
                // Reset the form fields
                setFirstName('');
                setLastName('');
                setEmail('');
                setGender('')
                setStatus('')
                closeModal()
            })
            .catch(() => {
                toast.error("Failed to create student.");
            });
    };

    return (
        <>
            {isOpen ?
                    (<div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full" >
                            <div className="flex items-center justify-center h-screen">
                                <div className="relative  w-full max-w-lg p-8 mx-auto bg-white rounded-md shadow-lg">
                                    <div className="mt-3 text-left">
                                        <div className="flex justify-between items-center mb-6">
                                            <h2 className="text-2xl font-semibold text-gray-800">Add Student</h2>
                                            <button onClick={closeModal}
                                                    className="text-gray-600 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg p-1.5 ml-auto inline-flex items-center">
                                                <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none"
                                                     viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                                                    <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12"/>
                                                </svg>
                                            </button>
                                        </div>
                                        <form onSubmit={handleSubmit}>
                                            <div className="mb-4">
                                                <label htmlFor="firstName"
                                                       className="block text-gray-700 text-sm font-semibold mb-2">
                                                    First Name
                                                </label>
                                                <input
                                                    type="text"
                                                    id="firstName"
                                                    value={firstName}
                                                    onChange={(e) => setFirstName(e.target.value)}
                                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                                    required
                                                />
                                            </div>
                                            <div className="mb-4">
                                                <label htmlFor="lastName" className="block text-gray-700 text-sm font-semibold mb-2">
                                                    Last Name
                                                </label>
                                                <input
                                                    type="text"
                                                    id="lastName"
                                                    value={lastName}
                                                    onChange={(e) => setLastName(e.target.value)}
                                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                                    required
                                                />
                                            </div>
                                            <div className="mb-6">
                                                <label htmlFor="email" className="block text-gray-700 text-sm font-semibold mb-2">
                                                    Email
                                                </label>
                                                <input
                                                    type="email"
                                                    id="email"
                                                    value={email}
                                                    onChange={(e) => setEmail(e.target.value)}
                                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                                    required
                                                />
                                            </div>
                                            <div className="mb-6">
                                                <label htmlFor="gender"
                                                       className="block text-gray-700 text-sm font-semibold mb-2">
                                                    Gender
                                                </label>
                                                <select
                                                    id="gender"
                                                    value={gender}
                                                    onChange={(e) => setGender(e.target.value)}
                                                    className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                                    required
                                                >
                                                    <option value="">Select Gender</option>
                                                    <option value="female">Female</option>
                                                    <option value="male">Male</option>
                                                    <option value="other">Other</option>
                                                </select>
                                            </div>
                                            <div className="mb-6">
                                                <label htmlFor="status"
                                                       className="block text-gray-700 text-sm font-semibold mb-2">
                                                    Status
                                                </label>
                                                <select
                                                    id="status"
                                                    value={status}
                                                    onChange={(e) => setStatus(e.target.value)}
                                                    className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                                    required
                                                >
                                                    <option value="">Select Status</option>
                                                    <option value="active">Active</option>
                                                    <option value="inactive">Inactive</option>
                                                </select>
                                            </div>
                                            <div className="flex items-center justify-between">
                                                <button
                                                    type="submit"
                                                    className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                                >
                                                    Add Student
                                                </button>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                         </div>
                    </div>)
            :null}

        </>

    );
};

export default AddStudentForm;
