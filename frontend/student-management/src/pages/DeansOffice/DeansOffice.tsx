import React from 'react';
import {BrowserRouter as Router, Route, Link, Routes} from 'react-router-dom';

import AddStudentForm from "../Forms/AddStudentForm/AddStudentForm";
import {AssignGroups} from "../Forms/AssignGroups/AssignGroups";
// Import other components and services you need

const DeansOffice = () => {
    return (

            <div className="flex">
                <div className="w-64 h-screen bg-gray-800 text-white p-5">
                    <h1 className="text-xl mb-6">Dean's Office</h1>
                    <nav>
                        <ul>
                            <li><Link to="/deans-office/students">Students</Link></li>
                            <li><Link to="/deans-office/assign-groups">Assign Groups</Link></li>
                            <li><Link to="/deans-office/add-subject">Add Subject</Link></li>

                        </ul>
                    </nav>
                </div>

            </div>
        
    );
};

export default DeansOffice;
