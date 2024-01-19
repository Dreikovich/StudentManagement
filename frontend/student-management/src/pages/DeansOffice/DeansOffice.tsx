import Sidebar from "../../components/sidebar/SideBar";
import { faUser, faUsers, faBook, faChalkboardTeacher} from '@fortawesome/free-solid-svg-icons';
import {faBookReader} from "@fortawesome/free-solid-svg-icons/faBookReader";

const navigationItems = [
    { text: 'Students', link: '/deans-office/students', icon: faUser },
    { text: 'Assign Groups', link: '/deans-office/assign-groups', icon: faUsers },
    { text: 'Add Subject', link: '/deans-office/add-subject', icon: faBook },
    { text: 'Subject Teacher Assignment', link: '/deans-office/subject-group-assignment', icon: faChalkboardTeacher },
    { text:' Subject Student Assignment', link:'/deans-office/subject-student-assignment', icon: faBookReader}

];

function DeansOffice() {
    return (
        <div className="flex">
            <Sidebar items={navigationItems} />

        </div>
    );
}

export default DeansOffice;