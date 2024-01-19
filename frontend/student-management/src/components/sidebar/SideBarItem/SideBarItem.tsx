import React from 'react';
import { NavLink } from 'react-router-dom';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";

// Define the type for the props expected by the Sidebar component
type SidebarItemProps = {
    text: string;
    link: string;
    icon: any;
};

export type {SidebarItemProps};

// Define a single navigation item for the sidebar
const SidebarItem: React.FC<SidebarItemProps> = ({ text, link, icon }) => {
    return (
        <li className="mb-4">
            <NavLink
                to={link}
                className={({ isActive }) =>
                    `block p-3 rounded-lg ${
                        isActive ? 'bg-blue-500 text-white' : 'text-gray-700 hover:bg-gray-100'
                    } transition duration-300`
                }
            >
                <FontAwesomeIcon icon={icon} className="mr-3" />
                {text}
            </NavLink>
        </li>
    );
};

export default SidebarItem;