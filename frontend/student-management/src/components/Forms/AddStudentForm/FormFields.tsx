import React, {ChangeEvent, useState} from 'react';
import { faEye, faEyeSlash } from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";


interface FormFieldProps {
    label: string;
    id: string;
    type: 'text' | 'password' | 'select';
    value: string;
    onChange: (event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => void;
    options?: { value: string; label: string }[];
    required?: boolean;
}

const FormField: React.FC<FormFieldProps> = ({
                                                 label,
                                                 id,
                                                 type,
                                                 value,
                                                 onChange,
                                                 options,
                                                 required = false,
                                             }) => {
    const [passwordVisible, setPasswordVisible] = useState(false);
    const togglePasswordVisibility = () => {
        setPasswordVisible(!passwordVisible);
    };

    return (
        <div className="mb-4">
            <label htmlFor={id}
                   className="block text-gray-700 text-sm font-semibold mb-2">
                {label}
            </label>
            {type === 'select' ? (
                <select
                    id={id}
                    value={value}
                    onChange={onChange}
                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                    required={required}>
                    {options?.map((option) => (
                        <option key={option.value} value={option.value}>
                            {option.label}
                        </option>
                    ))}
                </select>
            ) : type ==='password'?(
                <div className="relative ">
                    <input
                        type={passwordVisible?'text':'password'}
                        id={id}
                        value={value}
                        onChange={onChange}
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        required={required}/>
                    <button
                        onClick={togglePasswordVisibility}
                        type="button"
                        className="absolute  right-0  top-3 pr-3 flex items-center text-sm "
                    >
                        <FontAwesomeIcon icon={passwordVisible ?  faEye:faEyeSlash } />
                    </button>
                </div>) : (
                <input
                    type={type}
                    id={id}
                    value={value}
                    onChange={onChange}
                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                    required={required}/>)
            }

        </div>
    );
};

export default FormField;