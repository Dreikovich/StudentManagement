import React from 'react';
import './App.css';

import {BrowserRouter, Routes, Route} from 'react-router-dom'
import Navbar from "./components/navbar/Navbar";
import Students from "./pages/students/Students";
import AttendanceList from "./pages/attendance/AttendanceList";
import AddStudentForm from "./pages/Forms/AddStudentForm/AddStudentForm";
import {AssignGroups} from "./pages/Forms/AssignGroups/AssignGroups";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import DeansOffice from "./pages/DeansOffice/DeansOffice";
import SubjectsComponent from "./components/subject/SubjectComponent";
import AssignGroupToSubject from "./pages/Forms/AssignGroupToSubject/AssignGroupToSubject";
import AttendanceComponent from "./components/attendance/AttendanceComponent";
import GradesComponent from "./components/grades/GradesComponent";
import StudentManagement from "./components/students/StudentManagement";



function App() {
  return (
      <BrowserRouter>

          <div className="bg-gray-200 min-h-screen flex flex-col">
              <Navbar/>
              <ToastContainer/>
              <div className="container mx-auto p-4 ">
                  <Routes>
                      <Route path="/students" element={<StudentManagement />} />
                      <Route path="/attendance" element={<AttendanceList />} />
                      <Route path="/" element={<StudentManagement />} />
                      <Route path="/deans-office" element={<DeansOffice />} />
                      <Route path="/deans-office/assign-groups" element={<AssignGroups />} />
                      <Route path="/deans-office/subject-group-assignment" element={<SubjectsComponent/>} />
                      <Route path="/deans-office/subject-student-assignment" element={<AssignGroupToSubject/>} />
                      <Route path="/deans-office/attendance" element={<AttendanceComponent />} />
                      <Route path="/deans-office/grades" element={<GradesComponent />} />
                  </Routes>

              </div>
          </div>
      </BrowserRouter>

  );
}

export default App;
