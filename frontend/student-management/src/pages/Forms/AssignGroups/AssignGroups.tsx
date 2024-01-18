import React, { useState, useEffect } from 'react';
import {Group} from "../../../services/GroupServices";
import studentService, {Student} from "../../../services/StudentService";
import groupService from "../../../services/GroupServices";
import StudentGroupAssignmentService, {StudentGroupAssignment} from "../../../services/StudentGroupAssignmentService";
import {toast} from "react-toastify";
export function AssignGroups() {
    const [students, setStudents] = useState<Student[]>([]);
    const [groups, setGroups] = useState<Group[]>([]);
    const [unassignedStudents, setUnassignedStudents] = useState<Student[]>()
    const [assignedStudents, setAssignedStudents] = useState<Student[]>([]);
    const [selectedGroupId, setSelectedGroupId] = useState<number>();
    const groupSizeLimit = 20;


    const onClickAssignStudentToTheGroup=(student:Student, selectedGroupId:number | undefined)=>{
        if(selectedGroupId && student.studentID){
            const assignment: StudentGroupAssignment = {
                StudentID: student.studentID,
                StudentGroupID: selectedGroupId
            };
            StudentGroupAssignmentService.addStudentGroupAssignment(assignment)
            .then(() => {
                toast.success(`${student.firstName} has been assigned to the group!`);
                setStudents(prevStudents =>
                    prevStudents.map(s =>
                        s.studentID === student.studentID ? { ...s, groupName: groups.find(g => g.studentGroupID === selectedGroupId)?.groupName} : s
                    )
                );

            })
            .catch(error => {
                toast.error('Failed to assign student to group.');

            });
        }
    }

    useEffect(() => {
        if (selectedGroupId !== null && selectedGroupId != undefined) {

            const groupName = groups.find(group => group.studentGroupID === selectedGroupId)?.groupName;

            const updatedAssignedStudents = students.filter(student => student.groupName === groupName);

            setAssignedStudents(updatedAssignedStudents);
        }
    }, [selectedGroupId, students, groups]);

    useEffect(() => {
        groupService.getGroups().then((groups) => setGroups(groups))
        studentService.getStudents().then(students=>setStudents(students))
    }, []);


    useEffect(() => {
        setUnassignedStudents(students.filter(s => !s.groupName));
    }, [students]);


    console.log(assignedStudents)
    return (
        <div className="flex flex-wrap -mx-3">
            <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                <div className="bg-white shadow-lg rounded-lg p-6 h-full flex flex-col">
                    <h2 className="text-xl font-semibold mb-3">Unassigned Students</h2>
                    <div className="flex-grow overflow-y-auto max-h-[500px]">
                        <ul className="list-none p-0 space-y-2">
                            {unassignedStudents && unassignedStudents.map((student) => (
                                <li
                                    key={student.studentID}
                                    className="p-2 bg-gray-50 hover:bg-gray-100 rounded-lg cursor-pointer transition duration-150 ease-in-out"
                                    onClick={() => onClickAssignStudentToTheGroup(student, selectedGroupId)}
                                >
                                    {student.firstName} {student.lastName}
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
            <div className="w-full md:w-1/2 px-3">
                <div className="bg-white shadow-lg rounded-lg p-6 h-full flex flex-col">
                    <div className="mb-4">
                        <select
                            className="w-full p-2 border rounded-lg mb-3"
                            onChange={(e) => setSelectedGroupId(Number(e.target.value))}
                            defaultValue=""
                        >
                            <option value="" disabled>Select a group</option>
                            {groups.map((group) => (
                                <option key={group.studentGroupID} value={group.studentGroupID}>
                                    {group.groupName}
                                </option>
                            ))}
                        </select>
                        <div className="mb-4 text-sm font-semibold">Seats left: {selectedGroupId ? groupSizeLimit - assignedStudents.length : ''}</div>
                    </div>
                    <div className="flex-grow overflow-y-auto max-h-[500px]">
                        <ul className="list-none p-0 space-y-2">
                            {assignedStudents.map((student) => (
                                <li key={student.studentID} className="p-2 bg-gray-50 hover:bg-gray-100 rounded-lg transition duration-150 ease-in-out">
                                    {student.firstName} {student.lastName}
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    );
}