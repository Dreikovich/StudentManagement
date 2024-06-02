import React, {useState} from 'react';
import FormField from "../Forms/AddStudentForm/FormFields";
import loginService, {LoginResponse} from "../../services/LoginService";
import { useNavigate } from 'react-router-dom';
import {jwtDecode} from 'jwt-decode';

const Login: React.FC = () => {

    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');

    const navigate = useNavigate();

    const handleEmailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setLogin(event.target.value);
    };

    const handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(event.target.value);
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        const data = {login, password};
        loginService.verifyLogin(data).then(
            (response : LoginResponse) => {
                localStorage.setItem('token', response.token);
                const decodedToken: any = jwtDecode(response.token);
                const userRole = decodedToken.role;
                localStorage.setItem('RoleFromDecodedJWT', userRole);
                localStorage.setItem('role', response.role);
                setLogin('');
                setPassword('');
                navigate('/dashboard');
            })
            .catch((error) => {
            console.error(error);
        });
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 " >
            <div className="max-w-md w-full space-y-8 shadow-lg p-7 rounded-xl" style={{"marginTop": "-150px"}}>
                <div>
                    <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
                        Sign in to your account
                    </h2>
                    <p className="mt-2 text-center text-sm text-gray-600">
                        Welcome back, student! Sign in and let the knowledge empower your future.
                    </p>
                </div>
                <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
                    <input type="hidden" name="remember" value="true" />
                    <div className="rounded-md shadow-sm -space-y-px">
                        <FormField label="Login" id="login" type="text" value={login} onChange={(e) => setLogin(e.target.value)} required={true}/>
                        <FormField label="Password" id="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required={true}/>

                    </div>
                    <div>
                        <button type="submit" className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                            Sign in
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default Login;
