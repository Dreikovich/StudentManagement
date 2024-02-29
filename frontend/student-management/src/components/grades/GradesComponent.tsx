import React, { useState, useEffect } from 'react';
import { Student } from "../../services/StudentService";
import { Subject, SubjectService } from "../../services/SubjectService";
import groupService, { Group } from "../../services/GroupServices";

import { toast } from "react-toastify";
import  {GradeRecord} from "../../services/GradeService";
import gradeService from "../../services/GradeService";
import {SubjectGroupAssignment, subjectGroupAssignmentService} from "../../services/SubjectGroupAssignmentService";


const GradesComponent = () => {
    const [students, setStudents] = useState<Student[]>([]);
    const [subjects, setSubjects] = useState<Subject[]>([]);
    const [groups, setGroups] = useState<Group[]>([]);
    const [selectedGroup, setSelectedGroup] = useState<string | null>(null);
    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);
    const [selectedSessionType, setSelectedSessionType] = useState<string | null>(null);
    const [grades, setGrades] = useState<GradeRecord[]>([]);
    const [assignedSubjects, setAssignedSubjects] = useState<Subject[]>([]);
    const [subjectGroupAssignments, setSubjectGroupAssignments] = useState<SubjectGroupAssignment[]>([]);

    useEffect(() => {

        groupService.getGroups().then(setGroups).catch(e => toast.error(e.message));
        SubjectService.fetchSubjects().then(setSubjects).catch(e => toast.error(e.message));
        fetchSubjectGroupAssignments();
    }, []);

    useEffect(() => {
        if (selectedGroup) {
            groupService.getGroups().then(setGroups).catch(e => toast.error(e.message));
        }
    }, [selectedGroup]);

    const handleSaveGrades = async () => {
        try {
            console.log(grades);
            await Promise.all(grades.map(grade => gradeService.postGrade(grade)));

            toast.success("Grades saved successfully!");
            setGrades([]);
        } catch (error) {
            console.log(error);
            // If any request fails, show error message
            toast.error("Failed to save grades!");
        }
    };

    const fetchSubjectGroupAssignments = async () => {
        try {
            const data = await subjectGroupAssignmentService.fetchSubjectGroupAssignments();
            setSubjectGroupAssignments(data);
        } catch (error) {
            console.error(error);
        }
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        handleSaveGrades();
    };
    const onHandleChangeSelectedGroups = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const groupID = event.target.value;
        setSelectedGroup(groupID);
        const assignedSubjects = subjectGroupAssignments
            .filter(
                (assignment) => assignment.studentGroupID === Number(groupID))
            .map((assignment) => {
                const subject = subjects.find(subj => subj.subjectID === assignment.subjectID);
                return { subjectID: assignment.subjectID, subjectName: assignment.subjectName? assignment.subjectName : '', subjectComponents: subject?.subjectComponents? subject.subjectComponents : [] };
            });

        setAssignedSubjects(assignedSubjects);
    };

    const handleSubjectChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedSubject(event.target.value);
    };

    const handleSessionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedSessionType(event.target.value);
    };

    const handleGradeChange = (studentId:number | undefined, newValue:string) => {
        if (studentId === undefined) {
            console.error('studentId is undefined');
            return;
        }
        setGrades(currentGrades => {
            const newGrades = [...currentGrades];
            const gradeIndex = newGrades.findIndex(g => g.studentID === studentId);
            if (gradeIndex >= 0) {
                newGrades[gradeIndex].gradeValue = newValue;
            } else {
                newGrades.push({
                    studentID: studentId,
                    subjectID: Number(selectedSubject),
                    typeID: Number(selectedSessionType),
                    teacherID: 1,
                    gradeValue: newValue
                });
            }
            return newGrades;
        });
    };
    const findStudentsInGroup = ()=>{
        if(!selectedGroup) return -1;
        return groups.findIndex((group)=>group.studentGroupID === Number(selectedGroup));
    }
    const renderSubjectComponents = () => {
        const subject = assignedSubjects.find(s => s.subjectID === Number(selectedSubject));
        if (subject) {
            return subject.subjectComponents.map((component) => (
                <option key={component.typeID} value={component.typeID}>
                    {component.typeName}
                </option>
            ));
        }
        return null;
    }
    console.log(grades)

    return (
        <div className="bg-white p-6 rounded-lg shadow-lg w-3/4 mx-auto">
            <form onSubmit={handleSubmit} className="space-y-6">
                <div className="flex justify-between items-center">
                    <div className="flex space-x-4 items-center">
                        <div>
                            <label htmlFor="group-select" className="block text-sm font-medium text-gray-700">
                                Select Group
                            </label>
                            <select
                                id="group-select"
                                onChange={onHandleChangeSelectedGroups}
                                value={selectedGroup || ""}
                                className="mt-1 block pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
                            >
                                <option value="">Select Group</option>
                                {groups.map(group => (
                                    <option key={group.studentGroupID} value={group.studentGroupID}>
                                        {group.groupName}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div>
                            <label htmlFor="subject-select" className="block text-sm font-medium text-gray-700">
                                Select Subject
                            </label>
                            <select
                                id="subject-select"
                                onChange={handleSubjectChange}
                                value={selectedSubject || ""}
                                className="mt-1 block pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
                            >
                                <option value="">Select Subject</option>
                                {assignedSubjects.map((subject: Subject) => (
                                    <option key={subject.subjectID} value={subject.subjectID}>{subject.subjectName}</option>
                                ))}
                            </select>
                        </div>


                        <div>
                            <label htmlFor="session-type-select" className="block text-sm font-medium text-gray-700">
                                Select Session Type
                            </label>
                            <select
                                id="session-type-select"
                                onChange={handleSessionTypeChange}
                                value={selectedSessionType || ""}
                                className="mt-1 block pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
                            >
                                <option value="">Select Session Type</option>
                                {renderSubjectComponents()}
                            </select>
                        </div>
                    </div>

                    <button
                        type="submit"
                        className="px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                    >
                        Save Grades
                    </button>
                </div>

                <div className="flex flex-col space-y-4">
                    {groups && selectedGroup && selectedSessionType && selectedSubject && groups[findStudentsInGroup()].students.map((student: Student) => (
                        <div key={student.studentID} className="flex justify-between items-center">
                    <span className="block text-sm font-medium text-gray-700">
                        {student.firstName} {student.lastName}
                    </span>
                            <select
                                id={`grade-${student.studentID}`}
                                onChange={(e) => handleGradeChange(student.studentID, e.target.value)}
                                value={grades.find(g => g.studentID === student.studentID)?.gradeValue || ''}
                                className="mt-1 block pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
                            >
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="3.5">3.5</option>
                                <option value="4">4</option>
                                <option value="4.5">4.5</option>
                                <option value="5">5</option>
                            </select>
                        </div>
                    ))}
                </div>
            </form>
        </div>

    );
};

export default GradesComponent;
