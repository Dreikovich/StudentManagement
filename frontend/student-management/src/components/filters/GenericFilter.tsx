import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../../store/store';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faFilter } from '@fortawesome/free-solid-svg-icons';
import GenericFilterModal from "./GenericFilterModal";
import {setIsFilterOpen} from "../../features/students/FilterModalSlice";

const GenericFilter:React.FC = () => {
    const dispatch = useDispatch();
    const isFilterOpen = useSelector((state: RootState) => state.filterModal.isFilterOpen); // assuming RootState is your state type
    const toggleFilter = () => {
        dispatch(setIsFilterOpen(!isFilterOpen));
    };
    return (
        <div className="relative">
            <button onClick={toggleFilter} className="text-gray-600 hover:text-gray-800 focus:outline-none">
                <FontAwesomeIcon className='ml-2 mr-2 pointer-events-auto' icon={faFilter} />
            </button>
            <div className="relative">
                {isFilterOpen? <GenericFilterModal>
                    <div className="p-4 bg-white border rounded shadow w-max">
                        <div className="mb-2">Filter modal</div>
                        <div className="text-sm text-gray-500">
                            This is for the student filter
                        </div>
                    </div>
                </GenericFilterModal> : null}
            </div>


        </div>
    )
}

export default GenericFilter;