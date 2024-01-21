import React, { useState, useEffect } from 'react';
import {Student} from "../../services/StudentService";
import {Subject, subjectComponents, SubjectData, SubjectService} from "../../services/SubjectService";
import groupService, {Group} from "../../services/GroupServices";
import {SubjectGroupAssignment, subjectGroupAssignmentService} from "../../services/SubjectGroupAssignmentService";
import SubjectsComponent from "../subject/SubjectComponent";
import {AttendanceRecord} from "../../interfaces/Attendance/Attendance";
import attendanceService from "../../services/AttendanceService";
import {toast} from "react-toastify";


const AttendanceComponent = () => {
    const [students, setStudents] = useState<Student[]>([]);
    const [subjects, setSubjects] = useState<Subject[]>([]);
    const [assignedSubjects, setAssignedSubjects] = useState<Subject[]>([]);
    const [groups, setGroups] = useState<Group[]>([]);
    const [subjectGroupAssignments, setSubjectGroupAssignments] = useState<SubjectGroupAssignment[]>([]);
    const [teachers, setTeachers] = useState([]);
    const [selectedGroup, setSelectedGroup] = useState("");
    const [selectedSubject, setSelectedSubject] = useState('');
    const [selectedTeacher, setSelectedTeacher] = useState('');
    const [selectedSessionType, setSelectedSessionType] = useState('');
    const [selectedDate, setSelectedDate] = useState(new Date().toISOString().substr(0, 10));
    const [selectedTime, setSelectedTime] = useState('');

    const [studentStatuses, setStudentStatuses] = useState<{ [key: number]: string }>({});
    const [attendance, setAttendance] = useState<AttendanceRecord[]>([]);
    const [comments, setComments] = useState<{[key: number]: string}>({})
    const statusClasses = {
        absent: "bg-red-100 text-red-800",
        late: "bg-yellow-100 text-yellow-800",
        present: "bg-green-100 text-green-800",
    };
    const getStatusClassName = (studentID: number | undefined) => {
        if(studentID === undefined) return "";
        const status = studentStatuses[studentID]
        if (status === 'absent' || status === 'late' || status === 'present' ) {
            return statusClasses[status];
        }
        return "";
    };
    const fetchStudents = async () => {
        try {
            const data = await SubjectService.fetchSubjects();
            setSubjects(data);
        } catch (error) {
            console.error(error);
        }
    }


    const fetchSubjects = async () => {
        try {
            const data = await SubjectService.fetchSubjects();
            setSubjects(data);
        } catch (error) {
            console.error(error);
        }
    }

    const fetchGroups = async () => {
        try {
            const data = await groupService.getGroups();
            setGroups(data);
        } catch (error) {
            console.error(error);
        }
    }

    console.log(subjectGroupAssignments)
    const onHandleChangeSelectedGroups = (groupID:string) => {
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
    console.log(attendance)
    const findTheTeacherBasedOnSubjectComponent = (sessionType:string) => {
        const subject = assignedSubjects.find(s => s.subjectID === Number(selectedSubject));
        console.log(subject)
        console.log(selectedSessionType)
        if (subject) {
            const subjectComponent = subject.subjectComponents.find(component => component.typeID === Number(sessionType));
            console.log(subjectComponent)
            if (subjectComponent?.teacher?.teacherID !== undefined) {

                setSelectedTeacher(subjectComponent.teacher.teacherID.toString());
            }
        }
    }
    console.log(selectedTeacher)

    const fetchSubjectGroupAssignments = async () => {
        try {
            const data = await subjectGroupAssignmentService.fetchSubjectGroupAssignments();
            setSubjectGroupAssignments(data);
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        fetchGroups().then(r => console.log(r));
        fetchSubjectGroupAssignments().then(r => console.log(r));
        fetchSubjects().then(r => console.log(r));
    }, []);

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

    const onStatusChange = (status:string, studentID:number| undefined) => {
        console.log(status)
        if (studentID !== undefined) {
            studentStatuses[studentID] = status;
            setAttendance((prevAttendance: AttendanceRecord[]) => {
                const studentExists = prevAttendance.some(record => record.studentID === studentID);

                if (studentExists) {
                    return prevAttendance.map(record =>
                        record.studentID === studentID ? { ...record, status: status } : record
                    );
                } else {
                    const newAttendanceRecord: AttendanceRecord = {
                        subjectID: Number(selectedSubject),
                        groupID: Number(selectedGroup),
                        typeID: Number(selectedSessionType),
                        studentID: studentID,
                        teacherID: Number(selectedTeacher),
                        date: selectedDate,
                        status: studentStatuses[studentID],
                        comments: comments[studentID],
                        auditorium: "301",
                    };

                    return [...prevAttendance, newAttendanceRecord];
                }
            });
        }
    }
    console.log(selectedTeacher)
    const handeSaveAttendance = () => {
        console.log(attendance)
        setAttendance((prevAttendance: AttendanceRecord[]) => {
            return prevAttendance.map((record) => {
                if (comments[record.studentID] !== undefined) {
                    record.comments = comments[record.studentID];
                }
                else{
                    record.comments = "";
                }
                return record;
            });
        })
        attendance.map((record) => {
            attendanceService.postAttendance(record)
                .then(() => {
                    console.log("success")
                    toast.success("Attendance saved successfully!")
                })
                .catch(() => {
                    console.log("error")
                    toast.error(`Attendance of  save failed!`)
                })
        })
        console.log("attendance")
        setAttendance([])
    }

    const handleSessionTypeChange = (sessionType:string) => {
        setSelectedSessionType(sessionType);
        findTheTeacherBasedOnSubjectComponent(sessionType);
    }

    const onHandleCommentsChange = (comment:string, studentID:number| undefined) => {
        if(studentID!== undefined){
            setComments((prevComments: {[key: number]: string}) => {
                return {...prevComments, [studentID]: comment}
            })
        }
    }

    const findStudentsInGroup = ()=>{
        if(!selectedGroup) return -1;
        return groups.findIndex((group)=>group.studentGroupID === Number(selectedGroup));
    }
    return (
        <div className="bg-white p-6 rounded-lg shadow ">
            <div className="flex justify-between items-center">
                <div className="mb-4 flex gap-4 flex-grow">
                    <select
                        value={selectedGroup}
                        onChange={(e) => onHandleChangeSelectedGroups(e.target.value)}
                        className="block appearance-none w-100 bg-gray-100 border border-gray-300 text-gray-700 py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    >
                        <option value="">Select Group</option>
                        {groups.map((group) => (
                            <option key={group.studentGroupID} value={group.studentGroupID}>{group.groupName}</option>
                        ))}
                    </select>
                    <select
                        value={selectedSubject}
                        onChange={(e) => setSelectedSubject(e.target.value)}
                        className="block appearance-none w-100 bg-gray-100 border border-gray-300 text-gray-700 py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    >
                        <option value="">Select Subject</option>
                        {assignedSubjects.map((subject: Subject) => (
                            <option key={subject.subjectID} value={subject.subjectID}>{subject.subjectName}</option>
                        ))}
                    </select>
                    <select
                        value={selectedSessionType}
                        onChange={(e) => handleSessionTypeChange(e.target.value)}
                        className="block appearance-none w-100 bg-gray-100 border border-gray-300 text-gray-700 py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    >
                        <option value="">Select Session Type</option>
                        {renderSubjectComponents()}

                    </select>
                    {/*data picker*/}
                    <input
                        type="date"
                        value={selectedDate}
                        onChange={(e) => setSelectedDate(e.target.value)}
                        className="bg-gray-100 border border-gray-300 text-gray-700 py-2 px-4 rounded focus:outline-none focus:bg-white focus:border-gray-500"
                    />
                    {/*time picker*/}
                    <input
                        type="time"
                        value={selectedTime}
                        onChange={(e) => setSelectedTime(e.target.value)}
                        className="bg-gray-100 border border-gray-300 text-gray-700 py-2 px-4 rounded focus:outline-none focus:bg-white focus:border-gray-500"
                    />

                </div>
                <div>
                    <button
                        onClick={handeSaveAttendance}
                        type="button"
                        className="px-6 py-2 border border-transparent text-base leading-6 font-medium rounded-md text-white bg-blue-600 shadow hover:bg-blue-500 focus:outline-none transition ease-in-out duration-150"
                    >
                        Save
                    </button>
                </div>
            </div>


            <div className="overflow-x-auto">
                <table className="min-w-full">
                    <thead>
                    <tr>
                        <th className="px-6 py-3 border-b-2 border-gray-200 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                            Student Name
                        </th>
                        <th className="px-6 py-3 border-b-2 border-gray-200 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                            Status
                        </th>
                        <th className="px-6 py-3 border-b-2 border-gray-200 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                            Comments
                        </th>

                    </tr>
                    </thead>
                    <tbody>
                    {groups && selectedGroup && selectedSessionType && selectedSubject && groups[findStudentsInGroup()].students.map((student: Student) => (
                        <tr key={student.studentID} className="bg-white">
                            <td className="px-6 py-4 whitespace-no-wrap border-b border-gray-200">
                                {student.firstName} {student.lastName}
                            </td>

                            <td className="px-6 py-4 whitespace-no-wrap border-b border-gray-200">
                                <select
                                    className={`block w-1/2 h-8 border-2 text-base rounded-lg shadow-sm focus:ring-2 focus:ring-indigo-200 ${getStatusClassName(student.studentID)} transition-colors duration-300`}
                                    defaultValue=""
                                    onChange={(e) => {
                                        onStatusChange(e.target.value, student.studentID)
                                    }}
                                >
                                    <option value="">Select Status</option>
                                    <option className="bg-white text-gray-900" value="present">Present</option>
                                    <option className="bg-white text-gray-900" value="absent">Absent</option>
                                    <option className="bg-white text-gray-900" value="late">Late</option>
                                </select>

                            </td>

                            <td className="px-6 py-4 whitespace-no-wrap border-b border-gray-200">
                                <input
                                    type="text"
                                    className="form-input rounded-md shadow-sm mt-1 block w-full"
                                    placeholder="Add a comment..."
                                    onChange={(e) => onHandleCommentsChange(e.target.value, student.studentID)}

                                />
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default AttendanceComponent;