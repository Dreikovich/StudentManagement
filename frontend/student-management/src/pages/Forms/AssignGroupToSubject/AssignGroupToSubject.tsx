import React, { useState, useEffect } from 'react';

import ModalComponent from "../../../components/modal/ModalComponent";
import {SubjectService, SubjectData, Subject} from "../../../services/SubjectService";
import groupService, {Group} from "../../../services/GroupServices";

const AssignGroupToSubject = () => {
    const [subjects, setSubjects] = useState<SubjectData>([]);
    const [groups, setGroups] = useState<Group[]>();
    const [selectedSubject, setSelectedSubject] = useState('');
    const [selectedGroup, setSelectedGroup] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);

    useEffect(() => {
        fetchSubjects().then(r => console.log(r));
        fetchGroups().then(r => console.log(r));
    }, []);

    const fetchSubjects = async () => {
        try {
            const data = await SubjectService.fetchSubjects();
            setSubjects(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchGroups = async () => {
        try {
            const data = await groupService.getGroups();
            setGroups(data);
        } catch (error) {
            console.error(error);
        }
    };
    console.log(subjects);
    console.log(groups)
    const handleAssign = async () => {
        try {
            // await GroupService.assignSubjectToGroup(selectedSubject, selectedGroup);
            // Update UI accordingly
            setIsModalOpen(false);
            // You might want to fetch the updated subjects again or update the state directly
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div>
            <button
                onClick={() => setIsModalOpen(true)}
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md transition duration-200 ease-in-out"
            >
                Assign Group to Subject
            </button>

            <ModalComponent isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} title="Assign Group to Subject">
                <div className="bg-white rounded-lg overflow-hidden">
                    <div className="px-6 py-4">
                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2">
                                Subject
                            </label>
                            <select
                                value={selectedSubject}
                                onChange={(e) => setSelectedSubject(e.target.value)}
                                className="block appearance-none w-full bg-white border border-gray-300 text-gray-700 py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:bg-white focus:border-blue-500"
                            >
                                <option value="">Select Subject</option>
                                {subjects.map((subject) => (
                                    <option key={subject.subjectID} value={subject.subjectID}>
                                        {subject.subjectName}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2">
                                Group
                            </label>
                            <select
                                value={selectedGroup}
                                onChange={(e) => setSelectedGroup(e.target.value)}
                                className="block appearance-none w-full bg-white border border-gray-300 text-gray-700 py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:bg-white focus:border-blue-500"
                            >
                                <option value="">Select Group</option>
                                {groups && groups.map((group) => (
                                    <option key={group.studentGroupID} value={group.studentGroupID}>
                                        {group.groupName}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="flex justify-end pt-2">
                            <button
                                onClick={handleAssign}
                                className="px-4 py-2 bg-blue-500 hover:bg-blue-700 text-white text-sm font-bold rounded-md"
                            >
                                Assign
                            </button>
                        </div>
                    </div>
                </div>
            </ModalComponent>
        </div>
    );
};

export default AssignGroupToSubject;
