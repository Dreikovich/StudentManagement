import React, {useState} from 'react'
import {Link} from 'react-router-dom'
import { HiOutlineUsers, HiOutlineCalendar, HiOutlineDocument, HiOutlineAcademicCap, HiOutlineClipboardCheck } from 'react-icons/hi';
import SidebarItem, {SidebarItemProps} from "./SideBarItem/SideBarItem";
// const SideBar:React.FC = () =>{
//
//     const iconSize=28;
//     const links = [
//         { to: '/students', icon: <HiOutlineUsers size={iconSize} />, text: 'Student List' },
//         { to: '/', icon: <HiOutlineCalendar size={iconSize} />, text: 'Schedule' },
//         { to: '/attendance', icon: <HiOutlineClipboardCheck size={iconSize} />, text: 'Attendance' },
//         { to: '/Ranking', icon: <HiOutlineAcademicCap size={iconSize} />, text: 'Ranking' },
//     ];
//
//     return (
//         <div className="bg-white text-gray-500 p-4 w-1/6">
//             <ul className="mt-5">
//                 {links.map((link, index) => (
//                     <li key={index} className="flex items-center h-12 transition duration-300 ease-in-out hover:bg-gray-200">
//                         <Link to={link.to} className="flex text-xl  transition duration-300 ease-in-out hover:text-gray-900">
//                             {link.icon}
//                             <span>{link.text}</span>
//                         </Link>
//                     </li>
//                 ))}
//             </ul>
//         </div>
//     );
// }
type SidebarProps = {
    items: SidebarItemProps[];
};

// Define the sidebar component
const Sidebar: React.FC<SidebarProps> = ({ items }) => {
    return (
        <aside className="w-64 h-screen bg-white shadow-md">
            <nav className="p-5">
                <ul>
                    {items.map((item, index) => (
                        <SidebarItem key={index} {...item} />
                    ))}
                </ul>
            </nav>
        </aside>
    );
};

export default Sidebar;
