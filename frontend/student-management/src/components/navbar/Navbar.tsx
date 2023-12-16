import React from 'react';
import { Link } from "react-router-dom";

const Navbar: React.FC = () => {
    return (
        <nav className="bg-blue-500 p-4">
            <div className="container mx-auto flex justify-between items-center">
                <div className="text-white text-lg font-bold">Student Management</div>

                <div className="space-x-4">
                    <a href="#" className="text-white hover:text-gray-300">Dashboard</a>
                    <Link to="/students" className="text-white hover:text-gray-300">
                        Students
                    </Link>
                    <a href="#" className="text-white hover:text-gray-300">Enrollments</a>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;