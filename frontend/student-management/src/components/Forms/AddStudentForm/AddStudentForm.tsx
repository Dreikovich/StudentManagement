import React, { useState, FormEvent } from 'react';
import studentService from "../../../services/StudentService";
import {toast} from "react-toastify";
import {useDispatch} from "react-redux";
import {addStudent} from "../../../features/students/StudentSlice";
import {v4 as uuidv4} from 'uuid';
import FormField from "./FormFields";
import loginService from "../../../services/LoginService";
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
    const [isStudentInfoTab, setIsStudentInfoTab] = useState<boolean>(true);
    const [login, setLogin] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    const dispatch = useDispatch();

    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {

        e.preventDefault();
        const studentUuid = uuidv4();
        const studentData = { firstName, lastName, email, gender, status, studentUuid };
        const loginData = {login, password, studentUuid};

        studentService.postStudent(studentData )
            .then((newStudent) => {
                dispatch(addStudent(newStudent));

                toast.success("Student created successfully!");
                // Reset the form fields
                setFirstName('');
                setLastName('');
                setEmail('');
                setGender('');
                setStatus('');
                closeModal();
            })
            .catch(() => {
                toast.error("Failed to create student.");
            });

        loginService.postLogin(loginData)
            .then((newLogin) => {
                toast.success("Login and password created successfully!");
                setLogin('');
                setPassword('');
            })
            .catch(() => {
                toast.error("Failed to create login.");
            });
    };

    const generateLogin = (email: string, name:string) => {
        if(email){
            return email;
        }
        else if(name){
            return name+Math.floor(Math.random()*1000).toString();
        }
        else{
            const randomString = Math.random().toString(36).substring(2, 7);
            const randomNumber = Math.floor(Math.random() * 1000).toString();
            return randomString + randomNumber;
        }
    }

    const generatePassword = () => {
        const length = 12;
        const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+~`|}{[]:;?><,./-=";
        let password = "";
        for(let i=0, n=charset.length; i<length; i++){
            password += charset.charAt(Math.floor(Math.random()*n));
        }
        return password;
    }

    const handleGenerateCredentials = () => {
        setLogin(generateLogin(email, firstName));
        setPassword(generatePassword());
    }

    return (
        <>
            {isOpen ?
                    (<div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full" >
                            <div className="flex items-center justify-center min-h-screen">
                                <div  className=" w-full max-w-3xl h-fit p-8 mx-auto bg-white rounded-md shadow-lg">
                                    <div className="mt-3 text-left relative">
                                        <div className="flex  justify-between items-center mb-6">
                                            <h2 className="text-2xl font-semibold text-gray-800">Add Student Form</h2>
                                            <button onClick={closeModal}
                                                    className="text-gray-600 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg p-1.5 ml-auto inline-flex items-center">
                                                <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none"
                                                     viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                                                    <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12"/>
                                                </svg>
                                            </button>
                                        </div>
                                        <form onSubmit={handleSubmit} style={{ minHeight: '530px'}}>
                                            <div className="tabs flex gap-5 mb-5">
                                                <div  style={{
                                                    textDecoration: isStudentInfoTab ? 'underline' : 'none',
                                                    textDecorationThickness: isStudentInfoTab ? '2px' : 'initial',
                                                    textUnderlineOffset: isStudentInfoTab ? '3px' : 'initial'
                                                    }} className='StudentInfoTab'>
                                                    <p className="text-2xl font-bold cursor-pointer" onClick={()=>setIsStudentInfoTab(true)}>Student Info</p>
                                                </div>
                                                <div style={{
                                                    textDecoration: !isStudentInfoTab ? 'underline' : 'none',
                                                    textDecorationThickness: !isStudentInfoTab ? '2px' : 'initial',
                                                    textUnderlineOffset: !isStudentInfoTab ? '3px' : 'initial'
                                                }}
                                                     className="CredentialsTab">
                                                    <p className="text-2xl font-bold cursor-pointer" onClick={()=>setIsStudentInfoTab(false)}>Generate Credentials</p>
                                                </div>

                                            </div>

                                            {isStudentInfoTab ?
                                                <>
                                                    <FormField label="First Name" id="firstName" type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} required={true}/>
                                                    <FormField label="Last Name" id="lastName" type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} required={true}/>
                                                    <FormField label="Email" id="email" type="text" value={email} onChange={(e) => setEmail(e.target.value)} required={true}/>
                                                    <FormField label="Gender" id="gender" type="select" value={gender} onChange={(e) => setGender(e.target.value)}
                                                    options={[
                                                        { value: 'Select Gender', "label":"Select Gender"},
                                                        { value: 'Male', label:"male"},
                                                        { value: 'Female', label:"female"}

                                                    ]}
                                                    />
                                                    <FormField label="Status" id="status" type="select" value={status} onChange={(e) => setStatus(e.target.value)}
                                                    options={[
                                                        { value: 'Select Status', "label":"Select Status"},
                                                        { value: 'active', label:"Active"},
                                                        { value: 'inactive', label:"Inactive"}]}
                                                    />

                                                </>:
                                                <>
                                                    <FormField label="Login" id="login" type="text" value={login} onChange={(e) => setLogin(e.target.value)} required={true}/>
                                                    <FormField label="Password" id="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required={true}/>
                                                </>
                                            }
                                            <div
                                                className="flex items-center justify-between absolute bottom-0 right-0 mb-0">
                                                {!isStudentInfoTab ?
                                                    <button type="button" onClick={handleGenerateCredentials}
                                                            className=" mr-4 bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
                                                        Generate Credentials
                                                    </button> :null
                                                }
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
                : null}

        </>

    );
};

export default AddStudentForm;
