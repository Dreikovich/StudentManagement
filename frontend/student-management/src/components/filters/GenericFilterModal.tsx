import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { setIsFilterOpen } from '../../features/students/FilterModalSlice';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

type GenericFilterModalProps = {
    children: React.ReactNode;
}
const GenericFilterModal:React.FC<GenericFilterModalProps> =({children}) =>{
    const dispatch = useDispatch();

    return (
            <div className="absolute top-0 right-0 m-4">
                <div className="">
                    {children}
                </div>
                <button
                    onClick={() => dispatch(setIsFilterOpen(false))}
                    className=""
                >
                    <FontAwesomeIcon icon="times"/>
                </button>
            </div>

    )
}

export default GenericFilterModal;