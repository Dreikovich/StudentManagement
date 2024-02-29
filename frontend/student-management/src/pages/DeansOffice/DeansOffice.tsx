import Sidebar from "../../components/sidebar/SideBar";
import { faUser, faUsers, faBook, faChalkboardTeacher} from '@fortawesome/free-solid-svg-icons';
import {faBookReader} from "@fortawesome/free-solid-svg-icons/faBookReader";
import {faUserCheck} from "@fortawesome/free-solid-svg-icons/faUserCheck";
import {faEdit} from "@fortawesome/free-solid-svg-icons/faEdit";

const navigationItems = [
    { text: 'Students', link: '/deans-office/students', icon: faUser },
    { text: 'Assign Groups', link: '/deans-office/assign-groups', icon: faUsers },
    { text: 'Add Subject', link: '/deans-office/add-subject', icon: faBook },
    { text: 'Subject Teacher Assignment', link: '/deans-office/subject-group-assignment', icon: faChalkboardTeacher },
    { text:' Subject Student Assignment', link:'/deans-office/subject-student-assignment', icon: faBookReader},
    { text: 'Attendance', link: '/deans-office/attendance', icon: faUserCheck },
    { text: 'Grades', link: '/deans-office/grades', icon: faEdit },


];

function DeansOffice() {
    return (
        <div className="flex">
            <Sidebar items={navigationItems} />

        </div>
    );
}

export default DeansOffice;