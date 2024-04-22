import React, {FC, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { RootState } from '../../store/store';
import {Action} from "redux";

interface DropdownProps {
    options: string[];
    dropdownName: string;
    type?: "checkbox" | "text";
    //stateSelector?: (state: RootState) => string[];
    stateSelector?: string[];
    addAction?: (option: string) => Action;
    removeAction?: (option: string) => Action;
}

const Dropdown:FC<DropdownProps> = ({ options , dropdownName, type, stateSelector, addAction, removeAction}) => {
    const [isOpen, setIsOpen] = useState(false);
    const dispatch = useDispatch();

    const selectedOptions: string[] = useSelector(
        (state: RootState) => state.tableColumns.columns ?? []
    );

    const handleOptionClick = (option: string) => {
        if(selectedOptions.includes(option) && removeAction){
            dispatch(removeAction(option));
        }
        else{
            if (addAction){
                dispatch(addAction(option));
            }
        }
    }

    return (
        <div className="relative inline-block text-left">
            <div>
                <button type="button" className="inline-flex justify-center w-full rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-100 focus:ring-indigo-500" onClick={() => setIsOpen(!isOpen)}>
                    {dropdownName}
                </button>
            </div>

            {isOpen && (
                <div className="origin-top-right absolute right-0 mt-2 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5">
                    <div className="py-1" role="menu" aria-orientation="vertical" aria-labelledby="options-menu">
                        {options.map((option, index) => (
                            type != undefined ?
                                <div className="flex ml-5 " onClick={()=>handleOptionClick(option)}>
                                    <input type={type} checked={selectedOptions.includes(option)}/>

                                    <p key={index}

                                       className="ml-2 block rounded-xl px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 cursor-pointer"
                                       role="menuitem">{option}
                                    </p>
                                </div>
                                :
                                <p key={index}
                                   className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 pointer cursor-pointer"
                                   role="menuitem">{option}
                                </p>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
};

export default Dropdown;
