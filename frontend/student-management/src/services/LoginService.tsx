type Login = {
    studentUuid?: string;
    login: string;
    password: string;
}

export type {Login};

type LoginResponse = {
    token: string;
    role: string;
}

export type {LoginResponse};

const API_URL = 'https://localhost:7075/api/Login';

const postLogin = async (login: Login): Promise<Login | void> => {
    try {
        const response = await fetch(`${API_URL}/AddLogin`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(login)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const contentLength = response.headers.get('Content-Length');
        if (contentLength && Number(contentLength) > 0) {
            const data = await response.json();
            return data;
        } else {
            console.log('POST successful, no content returned');
            return login;
        }
    } catch (e) {
        console.error('Error posting login:', e);
        throw e;
    }
}

const verifyLogin = async (login: Login): Promise<LoginResponse> => {
    try {
        const response = await fetch(`${API_URL}/VerifyLogin`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(login)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const responseJson = await response.json();
        return responseJson as LoginResponse;

    } catch (e) {
        console.error('Error posting login:', e);
        throw e;
    }
}

const loginService = {postLogin, verifyLogin}
export default loginService;
