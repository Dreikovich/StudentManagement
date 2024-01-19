import React from "react";
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";

interface ModalProps {
    isOpen: boolean;
    onClose: () => void;
    title : string;
    children: React.ReactNode;
}

const ModalComponent: React.FC<ModalProps> = ({ isOpen, onClose, title, children }) => {
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 overflow-y-auto bg-gray-500 bg-opacity-50 flex justify-center items-center">
            <div className="bg-white rounded-lg shadow-xl w-1/2">
                <div className="p-6">

                    <div className="flex justify-between items-center mb-4">
                        <h2 className="text-xl font-bold text-gray-700 ml-8 mt-5">{title}</h2>
                        <button onClick={onClose} className="text-gray-600 hover:text-gray-900">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                 fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"
                                 strokeLinejoin="round">
                                <line x1="18" y1="6" x2="6" y2="18"></line>
                                <line x1="6" y1="6" x2="18" y2="18"></line>
                            </svg>
                        </button>
                    </div>
                    {children}
                </div>
            </div>
        </div>

    );
};

export default ModalComponent;