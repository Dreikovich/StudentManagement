import React from 'react'
import StudentList from "../../components/students/StudentList";
import SideBar from "../../components/sidebar/SideBar";

const Students: React.FC =()=>{
    return(
        <div className="flex">
            {/*<SideBar></SideBar>*/}
            <div className="flex-1 p-4">
                <h2 className='text-2xl font-bold mb-4'>Students</h2>
                <StudentList/>
            </div>

        </div>
    )
}
export default Students