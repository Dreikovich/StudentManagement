    import React, { useState, useEffect } from 'react';
    import {Subject, SubjectData, SubjectService} from "../../services/SubjectService";
    import ModalComponent from "../modal/ModalComponent";
    import {TeacherService} from "../../services/TeacherService";
    import {Teacher} from "../../services/TeacherService";
    import {SubjectTypes, SubjectTypesService} from "../../services/SubjectTypesService";
    import {toast} from "react-toastify";
    import AssignGroupToSubject from "../Forms/AssignGroupToSubject/AssignGroupToSubject";

    type SubjectComponent = {
        typeID: string;
        hours: string;
        teacherID: string;
    };

    const SubjectsComponent = () => {
        const [subjects, setSubjects] = useState<SubjectData>([]);
        const [isAddSubjectModalOpen, setIsAddSubjectModalOpen] = useState(false);
        const [subjectName, setSubjectName] = useState('');
        const [subjectComponents, setSubjectComponents] = useState([
            { typeID: '', hours: '', teacherID: '' }
        ]);
        const [teachers, setTeachers] = useState<Teacher[]>();
        const [subjectTypes, setSubjectTypes] = useState<SubjectTypes[]>();

        useEffect(() => {
            const getSubjects = async () => {
                try {
                    const data = await SubjectService.fetchSubjects();
                    setSubjects(data);
                } catch (error) {
                    console.error(error);
                }
            };
            const getTeachers = async () => {
                try {
                    const data = await TeacherService.fetchTeachers();
                    setTeachers(data);
                } catch (error) {
                    console.error(error);
                }
            }
            const getSubjectTypes = async () => {
                try {
                    const data = await SubjectTypesService.fetchSubjectTypes();
                    setSubjectTypes(data);
                } catch (error) {
                    console.error(error);
                }
            }

            getSubjects().then(r => console.log(r));
            getTeachers().then(r => console.log(r));
            getSubjectTypes().then(r => console.log(r));
        }, []);


        const onClose = () => {
            setIsAddSubjectModalOpen(false);
        };

        const openModel = () => {
            setIsAddSubjectModalOpen(true);
        };
        console.log(subjects)
        const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault();
            console.log("Form Submission");

            const formattedData = formatData();
            try {
                await SubjectService.postSubjectWithComponents(formattedData);
                updateSubjectsList();
                resetForm();
                toast.success('Subject added successfully!');
            } catch (error) {
                toast.error('Failed to add subject.');
            }
        };

        const formatData = () => {
            return {
                subjectName,
                subjectComponents: subjectComponents.map(component => ({
                    typeID: Number(component.typeID),
                    hours: Number(component.hours),
                    teacherID: Number(component.teacherID),
                })),
            };
        };

        const updateSubjectsList = () => {
            const newSubjects = {
                subjectName,
                subjectComponents: subjectComponents.map(component => ({
                    typeID: Number(component.typeID),
                    typeName: subjectTypes?.find(type => type.typeID === Number(component.typeID))?.typeName,
                    hours: Number(component.hours),
                    teacherID: Number(component.teacherID),
                    teacher: teachers?.find(teacher => teacher.teacherID === Number(component.teacherID)),
                })),
            };
            setSubjects(prevSubjects => [...prevSubjects, newSubjects]);
        };

        const resetForm = () => {
            setSubjectComponents([{ typeID: '', hours: '', teacherID: '' }]);
            setSubjectName('');
            setIsAddSubjectModalOpen(false);
        };

        console.log(subjectName)
        console.log(subjectComponents)
        console.log(teachers)
        const handleSubjectComponentChange = (index: number, field: keyof SubjectComponent, value: string): void => {
            const newComponents: SubjectComponent[] = [...subjectComponents];
            newComponents[index] = { ...newComponents[index], [field]: value };
            setSubjectComponents(newComponents);
        };

        const addSubjectComponent = () => {
            setSubjectComponents([...subjectComponents, { typeID: '', hours: '', teacherID: '' }]);
        };

        return (
            <div className="p-4 space-y-6">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-2xl font-bold text-gray-900">Subjects</h2>
                    <div className="flex space-x-5">
                        <AssignGroupToSubject/>
                        <button
                            onClick={openModel}
                            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md transition duration-200 ease-in-out"
                        >
                            Add Subject
                        </button>
                    </div>

                    <ModalComponent isOpen={isAddSubjectModalOpen} onClose={onClose} title={"Add new subject"}>
                        <form onSubmit={onSubmit} className="bg-white rounded-lg p-8 space-y-4">

                            <div>
                            <label htmlFor="subjectName" className="block text-gray-700 font-medium mb-2">
                                    Subject Name
                                </label>
                                <input
                                    id="subjectName"
                                    type="text"
                                    value={subjectName}
                                    onChange={(e) => setSubjectName(e.target.value)}
                                    className="block w-full px-4 py-2 border rounded-md shadow-sm focus:border-blue-300"
                                    placeholder="Enter subject name"
                                    required
                                />
                            </div>

                            {subjectComponents.map((component, index) => (
                                <div key={index} className="grid grid-cols-3 gap-4">
                                    <div>
                                        <label className="block text-gray-700 font-medium mb-2">
                                            Type
                                        </label>
                                        <select
                                            value={component.typeID}
                                            onChange={(e) => handleSubjectComponentChange(index, 'typeID', e.target.value)}
                                            className="block w-full px-4 py-2 border rounded-md shadow-sm focus:border-blue-300"
                                            required
                                        >
                                            <option value="">Select Type</option>
                                            {subjectTypes && subjectTypes.map((type) => (
                                                <option key={type.typeID} value={type.typeID}>
                                                    {type.typeName}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                    <div>
                                        <label className="block text-gray-700 font-medium mb-2">
                                            Hours
                                        </label>
                                        <input
                                            type="number"
                                            value={component.hours}
                                            onChange={(e) => handleSubjectComponentChange(index, 'hours', e.target.value)}
                                            className="block w-full px-4 py-2 border rounded-md shadow-sm focus:border-blue-300"
                                            placeholder="Enter hours"
                                            required
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-gray-700 font-medium mb-2">
                                            Teacher
                                        </label>
                                        <select
                                            value={component.teacherID}
                                            onChange={(e) => handleSubjectComponentChange(index, 'teacherID', e.target.value)}
                                            className="block w-full px-4 py-2 border rounded-md shadow-sm focus:border-blue-300"
                                            required
                                        >
                                            <option value="">Select Teacher</option>
                                            {teachers && teachers.map((teacher) => (
                                                <option key={teacher.teacherID} value={teacher.teacherID}>
                                                    {teacher.teacherFirstName} {teacher.teacherLastName}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>
                            ))}

                            <div className="text-right">
                                <button
                                    type="button"
                                    onClick={addSubjectComponent}
                                    className="text-blue-600 hover:underline"
                                >
                                    + Add Component
                                </button>
                            </div>

                            <div className="text-right mt-4">
                                <button
                                    type="submit"
                                    className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                >
                                    Submit
                                </button>
                            </div>
                        </form>
                    </ModalComponent>
                </div>
                {subjects.map((subject) => (
                    <div key={subject.subjectID} className="bg-white p-6 rounded-lg shadow-md space-y-3">
                        <h3 className="text-xl font-semibold text-gray-900">{subject.subjectName}</h3>
                        {subject.subjectComponents && subject.subjectComponents.map((session) => (
                            <div key={session.typeID} className="grid grid-cols-3 gap-4 items-center">
                                <span className="text-lg font-medium text-gray-700">{session.typeName}:</span>
                                <span className="text-lg text-gray-600">{session.hours} hours</span>
                                {session.teacher && (
                                    <span className="text-right text-lg text-gray-500">
                                    Teacher: <span
                                        className="text-gray-900">{session.teacher.teacherFirstName} {session.teacher.teacherLastName}</span>
                                </span>
                                )}

                            </div>
                        ))}
                    </div>
                ))}
            </div>
        );
    };

    export default SubjectsComponent;
